using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Order
    {
        private readonly List<OrderItem> _items = new();
        public Guid Id { get; private set; }
        public DateTime OrderDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public orderStatus Status { get; private set; } 
        public virtual IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        private Order() { }
        public Order(List<(Guid productId, int quantity, decimal price)> items)
        {
            if (items == null || !items.Any())
                throw new ArgumentException("El pedido debe contener al menos un artículo.", nameof(items));
            Id = Guid.NewGuid();
            OrderDate = DateTime.UtcNow;
            Status = orderStatus.Pending;
            foreach (var item in items)
            {
                var orderItem = new OrderItem(item.productId, item.quantity, item.price);
                _items.Add(orderItem);
                TotalAmount += item.quantity * item.price;
            }
        }
        public void AddOrderItem(Guid prdoductId, int quantity, decimal price)
        {
            var OrderIten = new OrderItem(prdoductId, quantity, price);
            _items.Add(OrderIten);
            //Recalculamos el total internamente para mantener integridad
            TotalAmount += quantity * price;
        }
        public void TransitionToProcessing()
        {
            if (Status != orderStatus.Pending)
                throw new InvalidOperationException("Solo se pueden procesar órdenes pendientes.");
            Status = orderStatus.Processing;
        }
    }
    public enum orderStatus
    {
        Pending = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5
    }
}
