using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Exceptions
{
    public abstract class ApplicationException : Exception
    {
        protected ApplicationException(string message) : base(message)
        {
        }

        public class NotFoundException : ApplicationException
        {
            public NotFoundException(string name, object key)
                : base($"La entidad \"{name}\" ({ key}) no fue encontrada.")
            {
            }
        }
    }
}
