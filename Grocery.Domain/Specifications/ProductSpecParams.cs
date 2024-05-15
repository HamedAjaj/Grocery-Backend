using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain.Specifications
{
    public class ProductSpecParams
    {

        // possible maxpageSize is 10 
        private const int maxPageSize = 10;

        private int _pageSize=5;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value; }
        }
        private string search;
        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

        public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; }
        public int? brandId { get; set; }
        public int? typeId { get; set; }
    }
}
