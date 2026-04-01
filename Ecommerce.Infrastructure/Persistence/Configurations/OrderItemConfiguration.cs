using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations
{
    public class OrderItemConfiguration: IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
         {
             builder.ToTable("OrderItems"); //Nombre de la tabla
             builder.HasKey(oi => oi.Id); //Clave primaria
             builder.Property(oi => oi.Price)
                .HasColumnType("decimal()");
             builder.Property(oi => oi.Quantity)
                .IsRequired();
        }
    }
}
