﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;




namespace Dogue.UI.MVC.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    
   
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Main Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "*Valid Phone Number Required: 123-456-789")]
        [StringLength(13, ErrorMessage = "*13 Character limit reached. Please.")]
        public string MainPhoneNumber { get; set; }

        [DisplayFormat(NullDisplayText = "*Not Available")]
        [Display(Name = "Secondary Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "*Valid Phone Number Required: 123-456-789")]
        [StringLength(13, ErrorMessage = "*13 Character limit reached.")]
        public string SecondaryPhoneNumber { get; set; } 
        
        [Required]
        [StringLength(50, ErrorMessage = "*50 Character limit reached.")]
        public string Address { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "*50 Character limit reached.")]
        public string City { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "*2 Character limit reached.")]
        public string State { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "*10 Character limit reached.")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}