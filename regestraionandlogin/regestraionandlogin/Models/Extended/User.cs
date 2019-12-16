using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace regestraionandlogin.Models
{
    [MetadataType(typeof(UsermetaData))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }

    }
    public class UsermetaData
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name required")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name required")]
        public string LastName { get; set; }


        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "minimum characters shuold be 8 ")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Passowrd required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = " password dosen't match ")]
        public string ConfirmPassword { get; set; }
    }
}