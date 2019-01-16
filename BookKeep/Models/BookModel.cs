using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookKeep.Models
{
    public class BookModel
    {
        [Key]
        public int Id { get; set; }
        public long BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsRead { get; set; }
    }
}
