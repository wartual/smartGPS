﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartGPS.Models.Administration
{
    public class SignInModel
    {
        
        [Required]
        [Display(Name="Username")]
        public String username{get; set;}

        [Required]
        [Display(Name = "Password")]
        public String password { get; set;}

        [Display(Name = "Remember me")]
        public Boolean remeberMe { get; set; }
    }
}