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
    
    public partial class FacebookProfile
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public string JsonPersonalDataAndFriends { get; set; }
        public string JsonUserAndFriendsCheckins { get; set; }
        public string JsonUserAndFriendsLikes { get; set; }
    
        public virtual User User { get; set; }
    }
}
