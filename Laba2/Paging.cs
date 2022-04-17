using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba2
{
    public class Paging // Доделать ПОГЕРнацию - мб нормальная смена кол-ва отображения
                        // как на сайте
    {
        public int PageIndex { get; set; }

        public List<UBI> SetPaging(List<UBI> ListToPage, int RecordsPerPage)
        {
            int PageGroup = PageIndex * RecordsPerPage;
            return ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList();
        }

        public List<UBI> Next(List<UBI> ListToPage, int RecordsPerPage)
        {
            PageIndex++;
            if (PageIndex >= ListToPage.Count / RecordsPerPage)
            {
                PageIndex = ListToPage.Count / RecordsPerPage;
            }
            return SetPaging(ListToPage, RecordsPerPage);
        }

        public List<UBI> Previous(List<UBI> ListToPage, int RecordsPerPage)
        {
            PageIndex--;
            if (PageIndex <= 0)
            {
                PageIndex = 0;
            }
            return SetPaging(ListToPage, RecordsPerPage);
        }
    }
}
