using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestniZadatak.Models
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage ="Product Name cannot be more than 100 characters")]
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 50, ErrorMessage = "Short Description needs to be between 50 and 300 characters")]
        public string ShortDescription { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 100, ErrorMessage = "Full Description needs to be between 100 and 2000 characters")]
        public string FullDescription { get; set; }

        public DateTime DatePublished { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public  int CategoryId { get; set; }
        public virtual Category Category { get; set; }


    }
}
