using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationAPI15_SecondStageTS_.dto;

namespace WebApplicationAPI15_SecondStageTS_.Services
{
	public static class QueryableExtension
	{
        //PagedResult<T>
        //тут тоже страшный тип возвращяемых данных, написан ради того чтобы дальше в не пришлось согласовывыть
        public static Task<ActionResult<IEnumerable<AnnouncementDTO>>> GetPaged<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCoutnt = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCoutnt);

            var skip = (page - 1) * pageSize;
            // result.Results = query.Skip(skip).Take(pageSize).ToList();


            //здесь формирую query для маппера
            return result.PGmapp(query.Skip(skip).Take(pageSize));
            // но теперь в result нет нужных данных!или я их не там ищю
            //  return result;

        }
    }
}
