using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Common
{
    public class InsufficientStockException : Exception
    {
        public InsufficientStockException(string name, int requested, int availableStock)
            : base($"No hay suficiente stock para el producto \"{name}\". Cantidad solicitada: {requested}, Stock disponible: {availableStock}.")
        {
        }
    }
}
