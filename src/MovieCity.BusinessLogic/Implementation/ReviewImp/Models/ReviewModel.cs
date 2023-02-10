using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.ReviewImp.Models
{
    public class ReviewModel
    {
        public Guid Id { get; set; }
        public string ReviewText { get; set; } = null!;
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public string UserImage { get; set; }
        public bool HasAvailableImage { get; set; }
    }
}
