using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProEventos.Persistence.Models
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PageList() { }

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static async Task<PageList<T>> CreateAsync(
            IQueryable<T> source, int pageNumber, int pageSize
        )
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize) // aqui ele vai pular (pageNumber - 1) vezes a quantidade de itens
                                    //ou seja, se cada pagina deve ter 5 itens, e vc estiver na pagina 3, ele vai pular 2 vezes, para pular 10 itens
                                    .Take(pageSize) // ai ele pega os proximos 5 itens que terá o retorno da pagina
                                    .ToListAsync(); // aqui ele vai retornar os 5 itens
            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}