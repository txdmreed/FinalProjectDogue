//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dogue.EF.DATA
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservation
    {
        public int ReservationID { get; set; }
        public int OwnerAssetID { get; set; }
        public int LocationID { get; set; }
        public System.DateTime ReservationDate { get; set; }
        public int ServiceID { get; set; }
    
        public virtual Location Location { get; set; }
        public virtual OwnerAsset OwnerAsset { get; set; }
        public virtual Service Service { get; set; }
    }
}
