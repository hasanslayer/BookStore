using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Book book, int quantity = 1)
        {
            CartLine cartLine = lineCollection.FirstOrDefault(b => b.Book.ISBN == book.ISBN);

            if (cartLine == null)
            {
                lineCollection.Add(new CartLine { Book = book, Quantity = quantity });
            }
            else
            {
                cartLine.Quantity += quantity;
            }
        }

        public void RemoveItem(Book book)
        {
            lineCollection.RemoveAll(b => b.Book.ISBN == book.ISBN);
        }

        public decimal ComputerTotalValue()
        {
            return lineCollection.Sum(b => b.Book.Price * b.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> CartLines
        {
            get { return lineCollection; }
        }

    }

    public class CartLine
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }

    }
}
