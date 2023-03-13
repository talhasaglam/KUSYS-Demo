using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS_Demo.Application.Pagination
{
    public class GetPagePropDto
    {
        public string SearchText { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
