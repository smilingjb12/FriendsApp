using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FriendsApp.ViewModels;
using FriendsApp.Models;

namespace FriendsApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppEntities db;
        private readonly EmailSender mailSender;

        public AccountController(AppEntities db, EmailSender mailSender)
        {
            this.db = db;
            this.mailSender = mailSender;
        }

        public RedirectToRouteResult Activate(string code)
        {
            User user = db.Users.FirstOrDefault(u => u.ActivationCode == code);
            if (user == null)
            {
                TempData["danger"] = "No user with given activation code was found";
                return RedirectToAction("Index", "Home");
            }
            user.IsActivated = true;
            db.SaveChanges();
            TempData["success"] = "Your account has been activated. You can sign in now";
            return RedirectToAction("Index", "Home");
        }

        public ViewResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            User existingUser = db.Users.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Email is already taken. Choose another");
                return View(model);
            }
            string activationCode = Guid.NewGuid().ToString();
            try
            {
                mailSender.SendActivationCode(model.Email, activationCode);
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Email", "Email format is incorrect");
                return View(model);
            }
            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "SHA1");
            User newUser = new User();
            newUser.Email = model.Email;
            newUser.Password = hashedPassword;
            newUser.ActivationCode = activationCode;
            newUser.IsActivated = false;
            newUser.Role = Role.User;
            db.Users.Add(newUser);
            db.SaveChanges();
            TempData["success"] = string.Format("An email with activation link has been sent to {0}. Check it out",
                model.Email);
            return RedirectToAction("Index", "Home");
        }
    }
}
