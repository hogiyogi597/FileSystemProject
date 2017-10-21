using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    class ZipFile : Entity
    {
        // parent must be null... somehow we need to restrict this.
        // children can be 0 or more

        public override int CalculateSize()
        {
            size = 0;
            foreach (Entity e in children)
            {
                size += e.CalculateSize();
            }
            size /= 2;
            return size;
        }
    }
}
