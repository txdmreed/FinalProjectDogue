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
    
    public partial class Seminar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Seminar()
        {
            this.MySeminars = new HashSet<MySeminar>();
        }
    
        public int SeminarID { get; set; }
        public int LocationID { get; set; }
        public byte SeminarReservationLimit { get; set; }
        public string SeminarName { get; set; }
        public System.DateTime SeminarDate { get; set; }
        public string SeminarNotes { get; set; }
        public string SeminarSpeaker { get; set; }
    
        public virtual Location Location { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MySeminar> MySeminars { get; set; }
    }
}