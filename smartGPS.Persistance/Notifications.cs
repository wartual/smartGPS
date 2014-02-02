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
    
    public partial class Notifications
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string CategoryId { get; set; }
        public string UserId { get; set; }
        public bool Active { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public string Address { get; set; }
        public long ThumbsUp { get; set; }
        public long ThumbsDown { get; set; }
    
        public virtual NotificationCategory NotificationCategory { get; set; }
        public virtual User User { get; set; }
    }
}
