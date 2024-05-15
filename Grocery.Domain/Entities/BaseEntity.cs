using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Domain.Entities.Identity;

namespace Grocery.Domain.Entities
{
    public class BaseEntity   // this parent class of entities  or db of business
    {
        public int Id { get; set; }
    }

}
