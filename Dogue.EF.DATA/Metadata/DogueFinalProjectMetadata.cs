using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dogue.EF.DATA
{
    [MetadataType(typeof(LocationMetaData))]
    public partial class Location { }

    public class LocationMetaData
    {
        [Required(ErrorMessage = "*Location Required")]
        [StringLength(50, ErrorMessage = "*50 Character limit reached. Please, contact system adminisistrator if more room is needed.")]
        [Display(Name = "Location")]
        public string LocationName { get; set; }
        [Required(ErrorMessage = "*Location Required")]
        [StringLength(50, ErrorMessage = "*50 Character limit reached. Please, contact system adminisistrator if more room is needed.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "*Location Required")]
        public string City { get; set; }
        [Required(ErrorMessage = "*Location Required")]
        [StringLength(2, ErrorMessage = "*2 Character Limit Reached.")]
        public string State { get; set; }
        [Required(ErrorMessage = "*Location Required")]
        [StringLength(10, ErrorMessage = "*10 Character limit reached. Please, contact system adminisistrator if more room is needed.")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Display(Name = "Trainer Reservation Limit")]
        public byte TrainerReservationLimit { get; set; }
        [Display(Name = "Photo Reservation Limit")]
        public byte PhotoReservationLimit { get; set; }
        [Display(Name = "Groomer Reserervation Limit")]
        public byte GroomerReservationLimit { get; set; }
    }

    [MetadataType(typeof(OwnerAssetMetaData))]
    public partial class OwnerAsset { }

    public class OwnerAssetMetaData
    {
        [DisplayFormat(NullDisplayText = "*Not Available")]
        [StringLength(50, ErrorMessage = "*50 Character limit reached. Please, contact system adminisistrator if more room is needed.")]
        [Display(Name = "Location")]
        public string AssetRegisteredName { get; set; }
        public string AssetCallName { get; set; }
        public string AssetSpecies { get; set; }
        public string AssetBreed { get; set; }
        public string AssetAge { get; set; }
        public Nullable<int> AssetSizeID { get; set; }
        public bool AssetTrainerCertified { get; set; }
        public string OwnerID { get; set; }
        public byte[] AssetPhoto { get; set; }
        public string SpecialNotes { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime DateAdded { get; set; }
    }

    [MetadataType(typeof(OwnerInformationMetaData))]
    public partial class OwnerInformation { }

    public class OwnerInformationMetaData
    {
        public string UserNameID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MainPhoneNumber { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool TransactionFileUpToDate { get; set; }
    }

    [MetadataType(typeof(ReservationMetaData))]
    public partial class Reservation {}

    public class ReservationMetaData
    {
        public int OwnerAssetID { get; set; }
        public int LocationID { get; set; }
        public System.DateTime ReservationDate { get; set; }

    }














}


