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
        [Display(Name="Username")]
        public String username { get; set; }
        
        [Required]
        [Display(Name = "Password")]
        public String password { get; set; }

        [Required]
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