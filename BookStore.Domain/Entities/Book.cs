using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Domain.Entities
{
    public class Book
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ISBN { get; set; }
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Specialization { get; set; }

    }
}
