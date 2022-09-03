using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyAccountingSystem
{
    internal class Warehouse
    {
        public int Id { get; set; }
        public int PharmacyId { get; set; }
        public string? Name { get; set; }
    }
}
