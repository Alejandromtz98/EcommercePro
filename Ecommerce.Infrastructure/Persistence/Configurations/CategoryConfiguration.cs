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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories"); //Nombre de la tabla
            builder.HasKey(c => c.Id); //Clave primaria
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.Description)
                .HasMaxLength(500);

            //Configuracion Senior: Mapeo de la coleccion privada
            builder.HasMany(c => c.Products)
                .WithOne() //No necesitamos la propiedad de navegación inversa
                .HasForeignKey(p => p.CategoryId) //Llave foranea en Product
                .OnDelete(DeleteBehavior.Restrict);

            //Le decimos al EF que use backing field '_products' para la navegacion
            var navigation = builder.Metadata.FindNavigation(nameof(Category.Products));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
