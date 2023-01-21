using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.FriendImp.Models
{
    public class FriendSuggestionsAndSearchUsersModel
    {
        public List<ListFriendsModel> FriendsSuggestions { get; set; } = new List<ListFriendsModel>();
        public List<ListFriendsModel> SearchUsers { get; set; } = new List<ListFriendsModel>();
    }
}
