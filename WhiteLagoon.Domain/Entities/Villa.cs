using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class Villa
    {
        //Write [Key] to specify the primary key of the table, but Id is already recognized as the primary key
        public int Id { get; set; }
        //Write required for required properties
        [MaxLength(50)] //Write [MaxLength] to specify the maximum length of the property
        public required string Name { get; set; }
        //Write var? for not required properties
        public string? Description { get; set; }
        [Display(Name = "Price per night")]
        [Range(10, 10_000)] //Write [Range] to specify the range of the property
        public double Price { get; set; }
        [Display(Name = "Square Feet")] //Write [Display] to specify the display name of the property
        public int Sqft { get; set; }
        [Range(1, 10)] //Write [Range] to specify the range of the property
        public int Occupancy { get; set; }
        [Display(Name = "Image URL")] //Write [Display] to specify the display name of the property
        //Write [Url] to specify that the property is a URL
        [NotMapped] //Write [NotMapped] to specify that the property is not mapped to the database
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; } //Write var? for not required properties
        //Write var? for not required properties
        public DateTime? CreatedDate { get; set; }
        //Write var? for not required properties
        public DateTime? UpdatedDate { get; set; }

        [ValidateNever] //Write [ValidateNever] to specify that the property is not validated
        public IEnumerable<Amenity> VillaAmenities { get; set; } //used to load in the amenities for the villa

        [NotMapped] //Write [NotMapped] to specify that the property is not mapped to the database
        public bool isAvailable { get; set; } = true; //used to check if the villa is available
    }
}

//Write [EmailAddress] to specify that the property is an email address
//Write [Phone] to specify that the property is a phone number
//Write [CreditCard] to specify that the property is a credit card number
//Write [EnumDataType] to specify that the property is an enumeration
//Write [FileExtensions] to specify the file extensions of the property
//Write [ImageExtensions] to specify the image extensions of the property
//Write [VideoExtensions] to specify the video extensions of the property
//Write [AudioExtensions] to specify the audio extensions of the property
//Write [DocumentExtensions] to specify the document extensions of the property
//Write [Password] to specify that the property is a password
//Write [DataType] to specify the data type of the property
//Write [RegularExpression] to specify the regular expression of the property
//Write [Compare] to specify the property to compare with
//Write [StringLength] to specify the length of the property
//Write [MinLength] to specify the minimum length of the property
//Write [Required] to specify that the property is required
//Write [DisplayFormat] to specify the format of the property