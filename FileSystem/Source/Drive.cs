/*
 * Drive class.
 * Drives cannot have a parent. Their size comes from the summation of the size of all the entities it contains.
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
    class Drive : Entity
    {

        public Drive (Entity _parent, string _name, string _filePath) : base(_parent, _name, _filePath)
        {
            if (_parent != null)
            {
                throw new Exception("Drives cannot have a parent");
            }
        }
    }
}
