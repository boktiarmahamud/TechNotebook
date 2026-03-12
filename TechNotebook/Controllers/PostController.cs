using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechNotebook.Data;
using TechNotebook.Models;
using TechNotebook.Models.ViewModels;

namespace TechNotebook.Controllers
{
	[Authorize]
	public class PostController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly ILogger<PostController> _logger;
		private readonly string[] _allowedExtension = { ".jpg", ".jpeg", ".png", ".gif" };

		public PostController(AppDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<PostController> logger)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
			_logger = logger;
		}
        public DbSet<Comments> Comments { get; set; }
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Index(int? categoryId)
		{
			var PostQuery = _context.Posts.Include(p => p.Category).AsQueryable();
			if (categoryId.HasValue)
			{
				PostQuery = PostQuery.Where(p =>p.CategoryId == categoryId);

			}
			var post = PostQuery.ToList();
			ViewBag.Categories = _context.Categories.ToList();
			return View(post);
		}
		[HttpGet]
		
		public async Task<IActionResult> Detail(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var post = _context.Posts.Include(p => p.Category).Include(p => p.Comments)
				.FirstOrDefault(p => p.Id == id);

			if(post == null)
			{
				return NotFound();
			}
			return View(post);
		}

		[HttpGet]
		[Authorize(Roles ="Admin")]
		public IActionResult Create()
		{
			var model = new PostViewModel
			{
				Post = new Post(),
				Categories = _context.Categories
					.Select(c => new SelectListItem
					{
						Value = c.Id.ToString(),
						Text = c.Name
					})
					.ToList()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create(PostViewModel model)
		{
			// Re-populate categories when returning view
			model.Categories = _context.Categories
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				})
				.ToList();	

			// Validate image first
			if (model.FeatureImage == null || model.FeatureImage.Length == 0)
			{
				ModelState.AddModelError("FeatureImage", "Please select an image.");
			}

			if (!ModelState.IsValid)
				return View(model);

			var inputExtension = Path.GetExtension(model.FeatureImage.FileName).ToLowerInvariant();
			if (!_allowedExtension.Contains(inputExtension))
			{
				ModelState.AddModelError("FeatureImage", "Only jpg, jpeg, png, gif allowed.");
				return View(model);
			}

			var uploadedUrl = await UploadFileToFolder(model.FeatureImage);

			if (string.IsNullOrEmpty(uploadedUrl))
			{
				ModelState.AddModelError("FeatureImage", "Image upload failed. Check server logs for details.");
				return View(model);
			}

			model.Post.FeaturedImageUrl = uploadedUrl;

			_context.Posts.Add(model.Post);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			if(id == null)
			{
				return NotFound();
			}
			var postFromDb =await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
			if (postFromDb == null) 
			{
				return NotFound();
			}
			PostViewModel obj = new PostViewModel

			{
				Post = postFromDb,
				Categories = _context.Categories
					.Select(c => new SelectListItem
					{
						Value = c.Id.ToString(),
						Text = c.Name
					}).ToList()
			};
			return View(obj);

		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(EditViewModel editViewModel)
		{
			if(!ModelState.IsValid)
			{
				return View(editViewModel);
			}
			var fromPostDb = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == editViewModel.Post.Id);
			if(fromPostDb == null)
			{
				return NotFound();
			}

			if(editViewModel.FeatureImage != null)
			{
				var inputExtension = Path.GetExtension(editViewModel.FeatureImage.FileName).ToLowerInvariant();
				if (!_allowedExtension.Contains(inputExtension))
				{
					ModelState.AddModelError("FeatureImage", "Only jpg, jpeg, png, gif allowed.");
					return View(editViewModel);
				}
				var ExistingImagePath = Path.Combine(
						_webHostEnvironment.WebRootPath, "images", Path.GetFileName(
							fromPostDb.FeaturedImageUrl
							)
					);

				if(System.IO.File.Exists(ExistingImagePath))
				{
					System.IO.File.Delete(ExistingImagePath);
				}

				editViewModel.Post.FeaturedImageUrl = await UploadFileToFolder(editViewModel.FeatureImage);
			}

			else
			{
				editViewModel.Post.FeaturedImageUrl = fromPostDb.FeaturedImageUrl;
			}
			_context.Posts.Update(editViewModel.Post);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var postFromDb =await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
			if(postFromDb == null)
			{
				return NotFound();

			}
			return View(postFromDb);
		}
		
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeletePost(int id)
		{
			var postFromDb = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
			if(string.IsNullOrEmpty(postFromDb.FeaturedImageUrl))
			{
				var ExistingImagePath = Path.Combine(
						_webHostEnvironment.WebRootPath, "images", Path.GetFileName(
							postFromDb.FeaturedImageUrl
							)
					);

				if (System.IO.File.Exists(ExistingImagePath))
				{
					System.IO.File.Delete(ExistingImagePath);
				}
				 
			}
			_context.Posts.Remove(postFromDb);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		[Authorize]		
		public JsonResult AddComment([FromBody] Comments comment)
		{
			if (string.IsNullOrEmpty(comment.UserName))
			{
				return Json(new { error = "UserName is required" });
			}

			comment.CommnetDate = DateTime.Now;

			_context.Comment.Add(comment);
			_context.SaveChanges();

			return Json(new
			{
				userName = comment.UserName,
				commentDate = comment.CommnetDate.ToString("MMMM dd,yyyy"),
				content = comment.Content
			});
		}
		[HttpGet]
		public async Task<IActionResult> AccessDenied()
		{
			return View();
		}

		private async Task<string> UploadFileToFolder(IFormFile file)
		{
			try
			{
				if (file == null || file.Length == 0)
				{
					_logger.LogWarning("UploadFileToFolder called with null or empty file.");
					return null;
				}

				var extension = Path.GetExtension(file.FileName);
				var fileName = Guid.NewGuid().ToString("N") + extension;

				// Ensure a valid web root path
				var wwwRootPath = _webHostEnvironment.WebRootPath;
				if (string.IsNullOrEmpty(wwwRootPath))
				{
					wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
					_logger.LogWarning("WebRootPath was null or empty. Falling back to {FallbackPath}", wwwRootPath);
				}

				// Ensure wwwroot exists
				if (!Directory.Exists(wwwRootPath))
				{
					Directory.CreateDirectory(wwwRootPath);
					_logger.LogInformation("Created wwwroot at {Path}", wwwRootPath);
				}

				var imageFolderPath = Path.Combine(wwwRootPath, "images");
				imageFolderPath = Path.GetFullPath(imageFolderPath);

				_logger.LogDebug("Saving uploaded image to folder: {ImageFolderPath}", imageFolderPath);

				if (!Directory.Exists(imageFolderPath))
				{
					Directory.CreateDirectory(imageFolderPath);
					_logger.LogInformation("Created images directory at {ImageFolderPath}", imageFolderPath);
				}

				var filePath = Path.Combine(imageFolderPath, fileName);

				// Attempt to save file
				await using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
				{
					await file.CopyToAsync(fileStream);
					await fileStream.FlushAsync();
				}

				_logger.LogInformation("Saved uploaded image to {FilePath}", filePath);

				// Return a web path (use forward-slash)
				return "/images/" + fileName;
			}
			catch (Exception ex)
			{
				// Log detailed exception to diagnose permission/path issues
				_logger.LogError(ex, "Failed to save uploaded image");
				return null;
			}
		}
	}
}
