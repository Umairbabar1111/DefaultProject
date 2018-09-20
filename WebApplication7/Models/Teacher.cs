using System;
using System.Collections.Generic;

namespace WebApplication7.Models
{
    public partial class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Experiance { get; set; }
        public string Image { get; set; }
        public string Cv { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
