using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    class Folder : Entity
    {
        public Folder(Entity _parent, string _name, string _filePath) : base(_parent, _name, _filePath)
        {
            if (_parent == null)
            {
                throw new Exception("Folders' parents cannot be null");
            }
        }
    }
}
