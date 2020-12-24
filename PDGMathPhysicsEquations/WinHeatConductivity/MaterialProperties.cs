using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinHeatConductivity
{
    class MaterialProperties
    {
        public string Name { get; set; }
        public double A { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
