//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace smartGPS.Persistance
{
    using System;
    using System.Collections.Generic;
    
    public partial class users
    {
        public users()
        {
            this.profile = new HashSet<profile>();
        }
    
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateLastLogin { get; set; }
        public string FacebookId { get; set; }
        public string TwitterId { get; set; }
    
        public virtual ICollection<profile> profile { get; set; }
    }
}
