using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Data
{
    public partial class User
    {
        public string Password { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
