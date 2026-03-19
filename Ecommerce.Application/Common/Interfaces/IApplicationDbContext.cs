using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entitties;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        IQueryable<Product> Products { get; }
        IQueryable<Category> Categories { get; }
        IQueryable<Order> Orders { get; }
        void Add<T>(T entity) where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
