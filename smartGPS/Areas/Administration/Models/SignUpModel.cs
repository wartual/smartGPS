using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartGPS.Areas.Administration.Models
{
    public class SignUpModel
    {
        [Required]
        [Display(Name = "Username")]
        public String username { get; set; }

        [Required]
        [MinLength(8)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public String password { get; set; }

        [Required]
        [MinLength(8)]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public String confirmPassword { get; set; }

        [Required]
        [Display(Name = "Name")]
        public String name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public String surname { get; set; } 
    }
}