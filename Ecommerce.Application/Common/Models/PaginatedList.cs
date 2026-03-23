using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public IReadOnlyCollection<T> Items { get; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedList(IReadOnlyCollection<T> items, int pageNumber, int count, int pageSize)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        //El metodo estatico que hace la magia con IQueryable, recibe la consulta original, el numero de pagina y el tamaño de pagina
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync(); //Cuenta el total de elementos en la consulta original
            var items= await source
                .Skip((pageNumber - 1) * pageSize) //Salta los elementos de las paginas anteriores
                .Take(pageSize) //Toma solo los elementos de la pagina actual
                .ToListAsync(); //Convierte el resultado a una lista

            //Crea una nueva instancia de PaginatedList con los elementos obtenidos y la información de paginación
            return new PaginatedList<T>(items, pageNumber, count, pageSize);

        }
    }
}
