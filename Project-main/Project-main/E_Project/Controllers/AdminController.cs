using E_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using E_Project.Data;


namespace E_Project.Controllers
{

    public class AdminController : Controller
    {

        private readonly FullWebsiteContext _context;
        public string env;


        public AdminController(FullWebsiteContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            env = Path.Combine(webHostEnvironment.WebRootPath, "assets/uploadsimages");

        }

        public IActionResult Dashboard()
        {

			var username = HttpContext.Session.GetString("Username");
			var userRole = HttpContext.Session.GetString("Role");
			if (string.IsNullOrEmpty(username) || userRole == "User")
			{
                return RedirectToAction("Index", "Home");
			}
			else
			{
                ViewBag.SessionMessage = $"{username}";
            }
			return View("Dashboard");
        }
        public IActionResult Template()
        {
            var username = HttpContext.Session.GetString("Username");
            var userRole = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(username) || userRole == "User")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.SessionMessage = $"{username}";
            }
            ViewBag.Template = _context.Templates.ToList(); 
            return View("Template");
        }
        [HttpPost]
        public IActionResult Template(CombinedView model)
        {
            var filePath = Path.Combine(env, model.C_Template.TemplateImage.FileName);

            using (var imagefile = new FileStream(filePath, FileMode.Create))
            {
                model.C_Template.TemplateImage.CopyTo(imagefile);
            }
            model.C_Template.ImagePath = model.C_Template.TemplateImage.FileName;


            _context.Templates.Add(model.C_Template);
            _context.SaveChanges();
            return RedirectToAction("Template");

        }
        [HttpGet]
        public IActionResult UpdateTemplate(int Id)
        {
            ViewBag.UpdateTemplate = _context.Templates.Find(Id);
            return View();
        }
        [HttpPost]
        public IActionResult UpdateTemplate(CombinedView model)
        {
            if (model.C_Template.TemplateImage != null && model.C_Template.TemplateImage.Length > 0)
            {
                try
                {
                    var filePath = Path.Combine(env, model.C_Template.TemplateImage.FileName);

                    Console.WriteLine($"Saving image to: {filePath}");
                    var directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    using (var imagefile = new FileStream(filePath, FileMode.Create))
                    {
                        model.C_Template.TemplateImage.CopyTo(imagefile);
                    }

                    model.C_Template.ImagePath = model.C_Template.TemplateImage.FileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving image: {ex.Message}");
                }
            }
            else
            {
            }

            _context.Templates.Update(model.C_Template);
            _context.SaveChanges();

            return RedirectToAction("Template");
        }



        [HttpPost]
        [HttpGet]
        public IActionResult DeleteTemplate(int Id)
        {
            var Tempdel = _context.Templates.Find(Id);
            _context.Templates.Remove(Tempdel);
            _context.SaveChanges();
            return RedirectToAction("Template");
        }
        public IActionResult Feedback()
        {
            var username = HttpContext.Session.GetString("Username");
            var userRole = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(username) || userRole == "User")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.SessionMessage = $"{username}";
            }

            ViewBag.Project = _context.Feedbacks.ToList();
            return View("Feedback");
        }
        [HttpPost]
        [HttpGet]
        public IActionResult DeleteFeedback(int Id)
        {
            var Tempdel = _context.Feedbacks.Find(Id);
            _context.Feedbacks.Remove(Tempdel);
            _context.SaveChanges();
            return RedirectToAction("Feedback");
        }

        public IActionResult contact()
        {
            var username = HttpContext.Session.GetString("Username");
            var userRole = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(username) || userRole == "User")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.SessionMessage = $"{username}";
            }
            ViewBag.contact = _context.Contacts.ToList();
            return View("contact");
        }

        [HttpPost]
        [HttpGet]
        public IActionResult Deletecontact(int Id)
        {
            var Tempdel = _context.Contacts.Find(Id);
            _context.Contacts.Remove(Tempdel);
            _context.SaveChanges();
            return RedirectToAction("contact");
        }
        public IActionResult Users()
        {
            var username = HttpContext.Session.GetString("Username");
            var userRole = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(username) || userRole == "User")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.SessionMessage = $"{username}";
            }
            ViewBag.User = _context.Users.ToList();
            return View("Users");
        }

        [HttpPost]
        [HttpGet]
        public IActionResult DeleteUsers(int Id)
        {
            var Tempdel = _context.Users.Find(Id);
            _context.Users.Remove(Tempdel);
            _context.SaveChanges();
            return RedirectToAction("Users");
        }
    public IActionResult Subscribers()
    {
            var username = HttpContext.Session.GetString("Username");
            var userRole = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(username) || userRole == "User")
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.SessionMessage = $"{username}";
            }
            ViewBag.Subscribe = _context.SubscriptionDetails.ToList();
        return View("Subscribers");
    }

        [HttpPost]
        [HttpGet]
        public IActionResult DeleteSubscribers(int Id)
        {
            var Tempdel = _context.SubscriptionDetails.Find(Id);
            _context.SubscriptionDetails.Remove(Tempdel);
            _context.SaveChanges();
            return RedirectToAction("Subscribers");
        }

    }
}

