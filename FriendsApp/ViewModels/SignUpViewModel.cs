using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FriendsApp.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@".+\@.+\..+", ErrorMessage = "Please input valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}