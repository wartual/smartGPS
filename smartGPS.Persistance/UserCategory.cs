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
    
    public partial class UserCategory
    {
        public UserCategory()
        {
            this.FacebookProccesedEntries = new HashSet<FacebookProccesedEntries>();
            this.FoursquareVenuesCategories = new HashSet<FoursquareVenuesCategories>();
            this.GooglePlacesAPICategories = new HashSet<GooglePlacesAPICategories>();
            this.Profile = new HashSet<Profile>();
        }
    
        public int Id { get; set; }
        public string Category { get; set; }
    
        public virtual ICollection<FacebookProccesedEntries> FacebookProccesedEntries { get; set; }
        public virtual ICollection<FoursquareVenuesCategories> FoursquareVenuesCategories { get; set; }
        public virtual ICollection<GooglePlacesAPICategories> GooglePlacesAPICategories { get; set; }
        public virtual ICollection<Profile> Profile { get; set; }
    }
}
