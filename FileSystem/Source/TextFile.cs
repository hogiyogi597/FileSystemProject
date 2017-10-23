/*
 * TextFile class.
 * Text files must have a parent. Their size comes from the length of their content. Cannot have children.
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
    class TextFile : Entity
    {
        private string content;

        public TextFile(Entity _parent, string _name, string _filePath, string _content) : base(_parent, _name, _filePath)
        {
            if(_parent == null)
            {
                throw new Exception("Text files' parents cannot be null");
            }
            if(_parent.GetType() == this.GetType())
            {
                throw new Exception("Text files cannot hold other entities");
            }
            children = null;
            content = _content;
            size = 0;
        }

        public override int CalculateSize()
        {
            if(content == null)
            {
                return 0;
            }
            size = content.Length;
            return size;
        }

        public override void Write(string newContent)
        {
            content = newContent;
        }
    }
}
