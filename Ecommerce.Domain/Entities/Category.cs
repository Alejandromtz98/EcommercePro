using System    ;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entitties
{
    public class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        //Consrtructor privado para forzar el uso de metodos de fabrica
        // o logica controlada
        private Category() { }
        //Constructor que garantiza que el objeto nace "Valido"
        public Category(string name, string description)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre es obligatorio", nameof(name));
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            IsActive = true; //Por defecto al crearla
        }
        //Exponemos la lista como lectura para evitar que alguien haga .Products.Add() desde fura
        private readonly List<Product> _products = new();
        public virtual IReadOnlyCollection<Product> Products => _products.AsReadOnly();
        //Método de comportamiento (No anemico)
        public void UpdateDetails(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre es obligatorio", nameof(name));
            Name = name;
            Description = description;
        }
        public void Deactivate() => IsActive = false;
    }
}
