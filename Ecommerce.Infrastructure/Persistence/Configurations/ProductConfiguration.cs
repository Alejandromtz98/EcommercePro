using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entitties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration: IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
         {
             builder.ToTable("Products"); //Nombre de la tabla
             builder.HasKey(p => p.Id); //Clave primaria
             builder.Property(p => p.Name)
                 .IsRequired()
                 .HasMaxLength(150);
             builder.Property(p => p.Description)
                 .HasMaxLength(1000);
            //Precision para dinero : 18 digitos en total, 2 decimales
            builder.Property(p => p.Price)
                 .HasColumnType("decimal(18,2)")
                 .IsRequired();
             builder.Property(p => p.CategoryId)
                 .IsRequired();
             builder.Property(p => p.Stock)
                 .IsRequired();
            builder.Property(p => p.RowVersion)
                .IsRowVersion();// EF lo usa para detectar cambios
        }
    }
}
