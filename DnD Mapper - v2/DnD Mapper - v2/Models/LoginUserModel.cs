using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DnD_Mapper___v2.Models
{
    public class LoginUserModel
    {
        [Required, 
            MinLength(3, ErrorMessage = "Username must be at least 3 characters"), 
            MaxLength(50, ErrorMessage = "Username cant be longer than 50 characters")]
        public string Username { get; set; }

        [Required, 
            MinLength(3, ErrorMessage = "Password must be at least 3 characters"), 
            MaxLength(256, ErrorMessage = "Password cant be longer than 256 characters"), 
            DataType(DataType.Password, ErrorMessage = "Password is an incorect type")]
        public string Password { get; set; }
    }
}
