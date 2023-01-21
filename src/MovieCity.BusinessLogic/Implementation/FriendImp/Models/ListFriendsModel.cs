using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.FriendImp.Models
{
    public class ListFriendsModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string? Image { get; set; }
        public DateTime Date { get; set; }
    }
}
