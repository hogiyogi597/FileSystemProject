using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    public abstract class Entity
    {
        protected Entity parent;
        protected List<Entity> children;
        protected string name;
        protected string filePath;
        protected int size;

        abstract public int CalculateSize();

        public virtual Entity Move(string srcPath, string destPath)
        {
            if(srcPath == filePath)
            {
                return this;
            }
            Entity targetItem = null;
            // Traverse the path to get to the object.
            foreach (Entity e in children)
            {
                if (srcPath.Contains(e.filePath))
                {
                    targetItem = e.Move(srcPath, destPath);
                }
            }
            // Modifies that object's path and parent
            targetItem.filePath = destPath;
            // Somehow need to recalculate the size of the entity

            // Change the paths of the children? Or is this down automatically?
            return null;
        }

        public virtual int WriteToFile(string srcPath, string newContent)
        {
            int temp = size;
            size = 0;

            // Traverse the path to get to the object
            foreach (Entity e in children)
            {
                if (srcPath.Contains(e.filePath))
                {
                    try
                    {
                        size += e.WriteToFile(srcPath, newContent);
                    }
                    catch (Exception)
                    {
                        size = temp;
                        throw new Exception("File not found!");
                    }

                }
            }
            return size;
        }
    }
}
