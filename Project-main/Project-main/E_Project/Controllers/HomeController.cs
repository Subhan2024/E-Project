using E_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Hosting.Internal;
using E_Project.Data;
using System.Net.Mail;
using System.Net;

namespace E_Project.Controllers
{

    public class HomeController : Controller
    {
        private readonly FullWebsiteContext _context;

        public HomeController(FullWebsiteContext context, IWebHostEnvironment hostingEnvironment, ILogger<HomeController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        public IActionResult EditImage(int id)
        {
            var template = _context.Templates.Find(id);
            if (template == null)
            {
                return NotFound();
            }
            ViewBag.ImagePath = Url.Content("~/assets/uploadsimages/" + template.ImagePath);
            return View(new { ImagePath = ViewBag.ImagePath });
        }

       [HttpPost]
    public async Task<IActionResult> SaveEditedImage([FromBody] UserTemplateImage model)
    {
        if (model == null)
        {
            return BadRequest(new { success = false, message = "Model is null" });
        }

        try
        {
            if (!string.IsNullOrEmpty(model.UserTempImage))
            {
                Console.WriteLine(model.UserTempImage);
            }
            var base64Data = model.UserTempImage.Replace("data:image/jpeg;base64,", "");

            var userId = 4; // get the user id from session or another source
            var userTemplateImage = new UserTemplateImage
            {
                UserTempImage = base64Data,
                UserId = userId
            };

            _context.UserTemplateImages.Add(userTemplateImage);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return RedirectToAction("PersonalSavedCards", "Home");
            }
            else
            {
                return Json(new { success = false, message = "Error saving image." });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    public IActionResult PersonalSavedCards()
    {
        var userId = 4;
        var savedImages = _context.UserTemplateImages
                                   .Where(img => img.UserId == userId)
                                   .Select(img => new UserTemplateImage { Id = img.Id, UserTempImage = img.UserTempImage })
                                   .ToList();

        return View(savedImages);
    }

    public IActionResult SendToRelatives(int id)
    {
        var image = _context.UserTemplateImages
                            .Where(img => img.Id == id)
                            .Select(img => new UserTemplateImage { Id = img.Id, UserTempImage = img.UserTempImage })
                            .FirstOrDefault();

        if (image == null)
        {
            return NotFound();
        }

        return View(image);
    }

    public IActionResult CarsSend(int id)
    {
        return View();
    }

    private async Task SendEmailAsync(string email, string base64Image)
    {
        var message = new MailMessage();
        message.To.Add(new MailAddress(email));
        message.From = new MailAddress("usamariz1997@gmail.com"); // replace with your email
        message.Subject = "Your Custom Card";
        message.Body = "Please find your custom card attached.";
        message.IsBodyHtml = true;

        byte[] imageBytes = Convert.FromBase64String(base64Image.Replace("data:image/jpeg;base64,", ""));
        using (var ms = new MemoryStream(imageBytes))
        {
            var attachment = new Attachment(ms, "custom_card.jpg", "image/jpeg");
            message.Attachments.Add(attachment);

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "usamariz1997@gmail.com", // replace with your email
                    Password = "cwaf qkuq aixt moqz" // replace with your email password
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com"; // replace with your SMTP host
                smtp.Port = 587; // replace with your SMTP port
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }
    }

    [HttpPost]
    public async Task<IActionResult> SendCard(string emailList, int imageId)
    {
        if (string.IsNullOrEmpty(emailList))
        {
            return BadRequest("Email list is empty.");
        }

        var emails = emailList.Split(',').ToList();
        var image = _context.UserTemplateImages.Find(imageId);

        if (image == null)
        {
            return NotFound("Image not found.");
        }

        foreach (var email in emails)
        {
            await SendEmailAsync(email, image.UserTempImage);
        }

        var sentEmails = new SendEmailList
        {
            Email = emailList,
            UserId = 4,
            TempImageId = imageId
        };

        _context.SendEmailLists.Add(sentEmails);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Emails sent successfully.";
        return RedirectToAction("SendToRelatives", new { id = imageId });
    }

        public IActionResult Index()
        {
            ViewBag.Template = _context.Templates.ToList();
            var username = HttpContext.Session.GetString("Username");
            ViewBag.SessionMessage = string.IsNullOrEmpty(username) ? null : username;
            return View();
        }


        [HttpPost]
        public IActionResult Contact(CombinedView model)
        {
            _context.Contacts.Add(model.C_Contact);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Feedback(CombinedView model)
        {
            model.C_Feedback.FeedbackDate = DateTime.Now;
            _context.Feedbacks.Add(model.C_Feedback);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Subscribe(CombinedView model)
        {
            model.C_Subscribe.Status = "Subscribe";
            model.C_Subscribe.SubscriptionStartDate = DateOnly.FromDateTime(DateTime.Now);
            model.C_Subscribe.SubscriptionEndDate = DateOnly.FromDateTime(DateTime.Now.AddYears(1));

            _context.SubscriptionDetails.Add(model.C_Subscribe);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Register(CombinedView model)
        {
            model.C_User.Role = "User";
            model.C_User.SubscriptionStatus = "Active";

            _context.Users.Add(model.C_User);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Addcarts()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CombinedView model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.C_User.Email && u.Password == model.C_User.Password);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Username or Password is incorrect";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
    public class EditedImageModel
    {
        public string ImageData { get; set; }
    }
}
