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
    
    public partial class GooglePlacesAPICategories
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int UserCategoryId { get; set; }
    
        public virtual UserCategory UserCategory { get; set; }
    }
}