using MovieCity.BusinessLogic.Implementation.Account.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.FeedImp.Models
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public UserModel User { get; set; }
        public string CommentText { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
