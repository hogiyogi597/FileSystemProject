/*
 * ZipFile class.
 * Zip files must have a parent. Their size comes from the summation of the size of all the entities it contains divided by 2.
 * 
 * Created by Stephen Hogan - 10/23/17
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    class ZipFile : Entity
    {
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
