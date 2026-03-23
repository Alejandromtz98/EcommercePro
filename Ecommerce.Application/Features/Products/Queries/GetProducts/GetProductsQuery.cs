using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Common.Models;
using Ecommerce.Application.Features.Products.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Ecommerce.Application.Features.Products.Queries.GetProducts;


public record GetProductsQuery(
        string? SearchTerm = null,
        int PageNumber = 1,
        int PageSize = 10
    ) : IRequest<PaginatedList<ProductDto>>;