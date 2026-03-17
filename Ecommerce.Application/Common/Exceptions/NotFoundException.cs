using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
            public NotFoundException(string name, object key)
                : base($"La entidad \"{name}\" ({key}) no fue encontrada.")
            {
        }
    }
}
