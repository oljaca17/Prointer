using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestniZadatak.Dtos
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

        public decimal Price { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }

        public DateTime DatePublished { get; set; }
    }
}
