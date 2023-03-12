using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS_Demo.Application.Pagination
{
    public class PagedResultDto<T>
    {
        public int TotalCount { get; set; }

        public int RecordCount { get; set; }

        public List<T> Items { get; set; }
    }
}
