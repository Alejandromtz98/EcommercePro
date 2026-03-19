using AutoMapper;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.DTOs;
using Ecommerce.Domain.Entitties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            //Salida: ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name));
            //Entrada: CreateProductCommand
            CreateMap<CreateProductDto, CreateProductCommand>();
            //Persistencia: CreateProductCommand -> Product
            CreateMap<CreateProductCommand, Product>();
        }
    }
}
