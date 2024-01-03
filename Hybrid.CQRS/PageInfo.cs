using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.CQRS
{
    public class PageInfo
    {
        public String endCursor { get; set; }
        public String startCursor { get; set; }
        public bool hasNextPage { get; set; } = false;
        public bool hasPreviousPage { get; set; } = false;
    }
}
