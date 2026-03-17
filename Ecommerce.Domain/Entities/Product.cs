using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Entitties
{
    public class Product : ISoftDelete
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public Guid CategoryId { get; private set; }
        public bool IsActive { get; set; } = true;
        public byte[] RowVersion { get; private set; } = null!; // Para control de concurrencia optimista

        public void Desactivate() => IsActive = false;
        //Linea para que funcione el Include en el DTO
        //Definir FK explicitamente
        //Vincular con el atributo
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; protected set; } = null!;

        //Consrtructor privado para forzar el uso de metodos de fabrica
        // o logica controlada
        private Product() { }
        public Product(string name, string description, decimal price, int stock, Guid categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre es obligatorio", nameof(name));
            if (price <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero", nameof(price));
            if (stock <= 0)
                throw new ArgumentException("El stock debe ser mayor a cero", nameof(stock));
            if (categoryId == Guid.Empty)
                throw new ArgumentException("La categoría es obligatoria", nameof(categoryId));

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
            IsActive = true; //Por defecto al crearla
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("El nuevo precio debe ser mayor a cero", nameof(newPrice));
            Price = newPrice;
        }
        public void AddStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La nueva cantidad a agregar debe ser mayor a cero", nameof(quantity));
            Stock += quantity;
        }
        public void ReduceStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La nueva cantidad a reducir debe ser mayor a cero", nameof(quantity));
            if (quantity > Stock)
                throw new InsufficientStockException(Name, quantity, Stock);
            Stock -= quantity;
        }
    }

}
