using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entitties;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Ecommerce.Application.Common.Interfaces;

namespace Ecommerce.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //DbSet para tus entidades de dominio
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Order> Orders => Set<Order>(); 
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Regla Senior: No configures tablas aqui
            //Esta linea busca todas las clases "Configuration" en este proyecto y las aplica
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            
            // Le decimos explícitamente: 
            // Un producto tiene UNA categoría, una categoría tiene MUCHOS productos.
            // Y LA LLAVE ES CategoryId (sin el 1).
            modelBuilder.Entity<Product>(entity =>
            {
                entity
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Evita borrados accidentales en cadena
            });
            // Filtro global para soft delete: Solo trae los productos activos
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.IsActive);

            //Recomdacion: base.OnModelCreating(modelBuilder) al final, para que no sobreescriba nada
             base.OnModelCreating(modelBuilder);
        }
    }
}
