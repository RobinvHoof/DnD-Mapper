using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DnD_Mapper___v2.Models
{
    public class SignupUserModel
    {
        [Required]
        [Display(Name = "Username")]
        [DataType(DataType.Text)]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "Username cant be longer than 50 characters")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password, ErrorMessage = "Password is an incorect type")]
        [MinLength(3, ErrorMessage = "Password must be at least 3 characters")]
        [MaxLength(256, ErrorMessage = "Password cant be longer than 256 characters")]        
        public string Password { get; set; }

        [Required]
        [Display(Name = "Re-enter Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ComparePassword { get; set; }
    }
}
