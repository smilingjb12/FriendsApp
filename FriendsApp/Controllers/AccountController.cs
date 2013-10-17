using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendsApp.Controllers
{
    public class AccountController : Controller
    {
        public ViewResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(int x)
        {
            return null;
        }
    }
}
