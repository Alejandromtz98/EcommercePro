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
    internal class OrderConfiguration: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders"); //Nombre de la tabla
            builder.HasKey(o => o.Id); //Clave primaria
            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");
            builder.Property(o => o.Status)
                .HasConversion<int>(); //Almacena el enum como int en la base de datos

             //Mapeo del backing field '_items'
             builder.HasMany(o => o.Items)
                .WithOne() //No necesitamos la propiedad de navegación inversa
                .HasForeignKey("OrderId") //Llave foranea en OrderItem
                .OnDelete(DeleteBehavior.Cascade); //Si se borra una orden, se borran sus items

            var navigation = builder.Metadata.FindNavigation(nameof(Order.Items));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
