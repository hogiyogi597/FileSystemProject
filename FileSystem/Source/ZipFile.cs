using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    class ZipFile : Entity
    {
        // children can be 0 or more

        public ZipFile(Entity _parent, string _name, string _filePath) : base(_parent, _name, _filePath)
        {
            if (_parent == null)
            {
                throw new Exception("Zip files' parents cannot be null");
            }
            size = CalculateSize();
        }

        public override int CalculateSize()
        {
            size = base.CalculateSize();
            size /= 2;
            return size;
        }
    }
}
