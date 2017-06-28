using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnsupportedPackageWriter
{
    class IdComparer : IEqualityComparer<String>
    {
        public bool Equals(string x, string y)
        {
            return String.Equals(x, y, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return obj.ToLower().GetHashCode();
        }
    }
}
