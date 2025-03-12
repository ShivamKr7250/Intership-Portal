using Microsoft.AspNetCore.Mvc;
using Internship_Portal.Model;
using Microsoft.AspNetCore.Authorization;
using Internship_Portal.Model.VM;
using System.Security.Claims;
using System;
using Internship_Portal.Data_Access.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using static Internship_Portal.Controllers.UserController;
using Internship_Portal.Controllers.Service;
using Microsoft.AspNetCore.Mvc.Rendering;
using Internship_Portal.Data_Access.Repository;
using Internship_Portal.Utility;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Internship_Portal.Controllers
{
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly BlogPostService _blogPostService;

        public BlogController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult BlogIndex()
        {
            return View();
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            // Retain the search string in pagination
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            // Sorting options
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["AuthorSortParm"] = sortOrder == "author" ? "author_desc" : "author";

            var posts = _unitOfWork.BlogPost.GetAll(includeProperties: "ApplicationUser,Comments");

            // Search functionality
            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(p => p.Title.Contains(searchString) || p.ShortDescription.Contains(searchString));
            }

            // Sorting functionality
            switch (sortOrder)
            {
                case "title_desc":
                    posts = posts.OrderByDescending(p => p.Title);
                    break;
                case "date":
                    posts = posts.OrderBy(p => p.PublicationDate);
                    break;
                case "date_desc":
                    posts = posts.OrderByDescending(p => p.PublicationDate);
                    break;
                case "author":
                    posts = posts.OrderBy(p => p.ApplicationUser.Name);
                    break;
                case "author_desc":
                    posts = posts.OrderByDescending(p => p.ApplicationUser.Name);
                    break;
                default:
                    posts = posts.OrderBy(p => p.Title);
                    break;
            }

            int pageSize = 5;
            var paginatedPosts = await PaginatedList<BlogPost>.CreateAsync(posts.AsNoTracking(), pageNumber ?? 1, pageSize);

            return View(paginatedPosts);
        }

        public IActionResult RecentPost()
        {
            var recentPosts = _unitOfWork.BlogPost.GetAll()
                .OrderByDescending(post => post.PublicationDate); // Take only the top 5 posts

            return View(recentPosts);
        }

        [Authorize]
        public IActionResult Upsert(int? id)
        {
            BlogVM blogVM = new BlogVM()
            {
                BlogPost = new BlogPost(),
                CategoryList = _unitOfWork.BlogCategory.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CategoryId.ToString()
                }),
                Tags = new List<string>()
            };

            if (id != null && id > 0)
            {
                // Fetch existing blog post for update
                blogVM.BlogPost = _unitOfWork.BlogPost.Get(u => u.PostId == id, includeProperties: "BlogCategory");
                if (blogVM.BlogPost == null)
                {
                    return NotFound();
                }
            }

            return View(blogVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BlogVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userDetail = _unitOfWork.User.Get(u => u.Id == userId);

            BlogPost blogPost;

            if (model.BlogPost.PostId == 0)
            {
                // New Blog Post (Create)
                blogPost = model.BlogPost;
                blogPost.UserId = userDetail.Id;
                blogPost.AuthorName = userDetail.Name;
                blogPost.PublicationDate = DateTime.Now;
            }
            else
            {
                // Existing Blog Post (Update)
                blogPost = _unitOfWork.BlogPost.Get(b => b.PostId == model.BlogPost.PostId);
                if (blogPost == null)
                {
                    return NotFound();
                }

                // Update existing fields
                blogPost.Title = model.BlogPost.Title;
                blogPost.CompanyName = model.BlogPost.CompanyName;
                blogPost.ShortDescription = model.BlogPost.ShortDescription;
                blogPost.Content = model.BlogPost.Content;
                blogPost.CategoryId = model.BlogPost.CategoryId;
                blogPost.Batch = model.BlogPost.Batch;
                blogPost.Course = model.BlogPost.Course;
                blogPost.ApplicationDeadline = model.BlogPost.ApplicationDeadline;

                // Handle Tags
                if (!string.IsNullOrEmpty(model.BlogPost.Tags))
                {
                    blogPost.Tags = string.Join(",", model.BlogPost.Tags);
                }
            }

            // Handle Image Upload
            if (model.BlogPost.Image != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.BlogPost.Image.FileName);
                string imagePath = Path.Combine(wwwRootPath, "images", "blogThumbnails");

                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                // Delete old image if exists
                if (!string.IsNullOrEmpty(blogPost.BlogThumbnail))
                {
                    string oldImagePath = Path.Combine(wwwRootPath, blogPost.BlogThumbnail.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string fullImagePath = Path.Combine(imagePath, fileName);
                using (var fileStream = new FileStream(fullImagePath, FileMode.Create))
                {
                    model.BlogPost.Image.CopyTo(fileStream);
                }

                blogPost.BlogThumbnail = "/images/blogThumbnails/" + fileName;
            }

            if (model.BlogPost.PostId == 0)
            {
                _unitOfWork.BlogPost.Add(blogPost);
            }
            else
            {
                _unitOfWork.BlogPost.Update(blogPost);
            }

            _unitOfWork.Save();
            TempData["success"] = model.BlogPost.PostId == 0 ? "The Blog has been created successfully." : "The Blog has been updated successfully.";

            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        [Authorize]
        public IActionResult Details(int blogId)
        {
            // Fetch blog post details, including related entities
            var blogDetails = _unitOfWork.BlogPost.Get(
                u => u.PostId == blogId,
                includeProperties: "ApplicationUser,Comments.ApplicationUser,BlogCategory"
            );

            if (blogDetails == null)
            {
                return NotFound();
            }

            // Ensure Comments is initialized to prevent null reference issues
            blogDetails.Comments ??= new List<BlogComment>();

            // Get the logged-in user details
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userDetail = _unitOfWork.User.Get(u => u.Id == userId);

            // Initialize student details only if the user is a student
            Student studentDetail = null;
            bool hasAlreadyApplied = false;

            if (User.IsInRole(SD.Role_Student))
            {
                studentDetail = _unitOfWork.StudentData.Get(s => s.UserId == userId);

                if (studentDetail != null)
                {
                    hasAlreadyApplied = _unitOfWork.AppliedDrive.Any(a => a.StudentId == studentDetail.StudentId && a.DriveId == blogId);
                }
            }

            // Prepare the ViewModel
            var model = new BlogVM
            {
                BlogPost = blogDetails,
                BlogComment = new BlogComment
                {
                    UserId = userId,
                    ApplicationUser = userDetail,
                    PostId = blogId
                },
                Comments = blogDetails.Comments.Select(comment => new BlogComment
                {
                    UserId = comment.UserId,
                    ApplicationUser = comment.ApplicationUser ?? new ApplicationUser(), // Prevent null reference
                    PostId = comment.PostId,
                    Content = comment.Content,
                    Timestamp = comment.Timestamp
                }).ToList(),
                User = userDetail,
                Student = studentDetail,
                HasAlreadyApplied = hasAlreadyApplied // NEW: Track if the student has applied
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Student)]
        public IActionResult Apply(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userDetail = _unitOfWork.User.Get(u => u.Id == userId);

            var student = _unitOfWork.StudentData.Get(s => s.UserId == userId);

            if (student == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect if not logged in
            }

            var alreadyApplied = _unitOfWork.AppliedDrive
                .Any(a => a.StudentId == student.StudentId && a.DriveId == postId);

            if (alreadyApplied)
            {
                TempData["Error"] = "You have already applied for this drive.";
                return RedirectToAction("Details","Blog", new { id = postId });
            }

            var application = new AppliedDrive
            {
                StudentId = student.StudentId,
                DriveId = postId
            };

            _unitOfWork.AppliedDrive.Add(application);
            _unitOfWork.Save(); // Synchronous save

            TempData["Success"] = "Application submitted successfully!";
            return RedirectToAction("Details","Blog", new { id = postId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment(BlogVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.BlogComment.UserId = userId;
            model.BlogComment.Timestamp = DateTime.Now;
            _unitOfWork.BlogComment.Add(model.BlogComment);
            _unitOfWork.Save();
            TempData["success"] = "The Comment has been added successfully.";
            return RedirectToAction("Details", new { blogId = model.BlogComment.PostId });

            //var blogDetails = _unitOfWork.BlogPost.Get(u => u.PostId == model.BlogComment.PostId, includeProperties: "ApplicationUser,Comments");
            //model.BlogPost = blogDetails;
            //return View("Details", model);
        }

        [HttpPost]
        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Json(new { success = false });
            }

            var results = _unitOfWork.BlogPost.SearchBlogPosts(searchTerm)
                .Select(b => new
                {
                    b.PostId,
                    b.Title,
                    b.ShortDescription,
                    b.PublicationDate
                })
                .ToList();

            return Json(new { success = true, data = results });
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch only the blog posts created by the specific user
            var blogPosts = _unitOfWork.BlogPost
                .GetAll(b => b.UserId == userId, includeProperties: "BlogCategory,ApplicationUser")
                .Select(post => new
                {
                    post.PostId,
                    post.Title,
                    AuthorName = post.ApplicationUser != null ? post.ApplicationUser.Name : "Unknown",
                    post.PublicationDate,
                    CategoryName = post.BlogCategory != null ? post.BlogCategory.Name : "Uncategorized",
                    post.CompanyName
                })
                .ToList();

            return Json(new { data = blogPosts });
        }



        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Invalid blog post ID" });
            }

            // Retrieve the blog post to be deleted
            var blogToBeDeleted = _unitOfWork.BlogPost.Get(u => u.PostId == id);
            if (blogToBeDeleted == null)
            {
                return Json(new { success = false, message = "Blog post not found" });
            }

            // Retrieve and delete all comments related to the blog post
            var commentsToBeDeleted = _unitOfWork.BlogComment.GetAll(c => c.PostId == id).ToList();
            foreach (var comment in commentsToBeDeleted)
            {
                _unitOfWork.BlogComment.Remove(comment);
            }

            // Delete the blog post
            _unitOfWork.BlogPost.Remove(blogToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted successfully" });
        }

        #endregion
    }
}
