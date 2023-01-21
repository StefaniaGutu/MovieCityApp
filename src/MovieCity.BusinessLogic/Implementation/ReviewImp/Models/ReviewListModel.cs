using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.ReviewImp.Models
{
    public class ReviewListModel
    {
        public Guid Id { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public string MovieTitle { get; set; }
        public Guid MovieId { get; set; }
        public bool IsSeries { get; set; }
    }
}
