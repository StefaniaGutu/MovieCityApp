using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.Common.DTOs
{
    public class CurrentUserDTO
    {
        public CurrentUserDTO()
        {
            Roles = new List<string>();
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsAuthenticated { get; set; }
        //public string Image { get; set; }

        public string Token { get; set; }

        public List<string> Roles { get; set; }
    }
}
