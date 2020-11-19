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
    public partial class OwnerAsset
    {
        [Display(Name = "Client Animal Quick Look")]
        public string AssetInfo
        {
            get { return string.Format("{0} -- {1} -- {2} -- (3}", AssetSpecies, AssetCallName, (AssetTrainerCertified == true) ? "Training Certified" : "Training Unknown", AssetSize);}
        }
    }


    public class OwnerAssetMetaData
    {
        [DisplayFormat(NullDisplayText = "*Not Available")]
        [StringLength(50, ErrorMessage = "*50 Character limit reached. Please, contact system adminisistrator if more room is needed.")]
        [Display(Name = "Registered Name")]
        public string AssetRegisteredName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "*50 Character limit reached.")]
        [Display(Name = "Call Name")]
        public string AssetCallName { get; set; }
        [Required]
        [Display(Name = "Species")]
        public string AssetSpecies { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "*50 Character limit reached.")]
        [Display(Name = "Breed")]
        public string AssetBreed { get; set; }
        [DisplayFormat(NullDisplayText = "*Not Available")]
        [StringLength(50, ErrorMessage = "*50 Character limit reached. Please, contact system adminisistrator if more room is needed.")]
        [Display(Name = "Age")]
        public string AssetAge { get; set; }
        [Required]
        [Range(0, 200, ErrorMessage = "Invalid Value")]
        [Display(Name = "Size (see table for breakdown)")]
        public string AssetSize { get; set; }
        [Display(Name ="Trainer Certified")]
        public bool AssetTrainerCertified { get; set; }
        [Required]
        public int OwnerID { get; set; }
        [DisplayFormat(NullDisplayText = "*Not Available")]
        public byte[] AssetPhoto { get; set; }
        [DisplayFormat(NullDisplayText = "*Not Available")]
        [UIHint("MulilineText")]
        public string SpecialNotes { get; set; }
        [Display(Name ="Active")]
        public bool IsActive { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name = "Date Added")]
        public System.DateTime DateAdded { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "*50 Character limit reached.")]
        [Display(Name = "Descriptive Color Profile")]
        public string DescriptiveColorProfile { get; set; }

    }

    [MetadataType(typeof(OwnerInformationMetaData))]
    public partial class OwnerInformation
    {
        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return ($"{FirstName} {LastName}"); }
        }
    }


    public class OwnerInformationMetaData
    {

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Main Phone Number")]
        [RegularExpression("phone", ErrorMessage ="*Valid Phone Number Required: 123-456-789")]
        [StringLength(13, ErrorMessage = "*13 Character limit reached. Please.")]
        public string MainPhoneNumber { get; set; }
        [DisplayFormat(NullDisplayText = "*Not Available")]
        [Display(Name = "Secondary Phone Number")]
        [RegularExpression("phone", ErrorMessage = "*Valid Phone Number Required: 123-456-789")]
        [StringLength(13, ErrorMessage = "*13 Character limit reached.")]
        public string SecondaryPhoneNumber { get; set; }
        [Required]
        [Display(Name = "Email")]
        [RegularExpression("Email", ErrorMessage = "*Valid Email Required: email@example.com")]
        [StringLength(50, ErrorMessage = "*50 Character limit reached.")]
        public string Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "*50 Character limit reached.")]
        public string Address { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "*50 Character limit reached.")]
        public string City { get; set; }
        [Required]
        [StringLength(2, ErrorMessage = "*2 Character limit reached.")]
        public string State { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "*10 Character limit reached.")]
        public string ZipCode { get; set; }
        [Display(Name ="Payment Options Up-To-Date")]
        public bool TransactionFileUpToDate { get; set; }
    }

    [MetadataType(typeof(ReservationMetaData))]
    public partial class Reservation {}

    public class ReservationMetaData
    {
        public int OwnerAssetID { get; set; }
        public int LocationID { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name = "Date Added")]
        public System.DateTime ReservationDate { get; set; }

    }
   [MetadataType(typeof(SiteUserMetaData))]
   public partial class SiteUser { }

    public class SiteUserMetaData
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserID { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name ="Last Name")]
        public string LastName { get; set; }

    }














}


