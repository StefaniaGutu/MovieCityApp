using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.Common.DTOs
{
    public class LoggedUserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Image { get; set; }

        public string Token { get; set; }
    }
}
