using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id  { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; } //Precio capturado al momento de la venta

        private OrderItem() { }
        internal OrderItem (Guid productId, int quantity, decimal price)
        {
            if(quantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(quantity));
            if (price < 0)
                throw new ArgumentException("El precio no puede ser negativo", nameof(price));
            Id = Guid.NewGuid();
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }
    }
}
