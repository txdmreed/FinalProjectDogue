using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dogue.UI.MVC.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage ="Sender name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage ="A valid email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="A subject is required.")]
        public string Subject { get; set; }
        [Required(ErrorMessage ="A message is required.")]
        [UIHint("MultilineText")]
        public string Message { get; set; }

    }
}