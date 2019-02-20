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
        [Required(ErrorMessage = "Book title is required.")]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Book description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Book price is required.")]
        [Range(0.1, 9999.99, ErrorMessage = "Please enter the valid price")]
        public decimal Price { get; set; }
        [Required]
        public string Specialization { get; set; }

    }
}
