using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileSystem
{
    public abstract class Entity
    {
        protected string alphanumericPattern = "^[A-Za-z0-9_]*$";
        protected Entity parent;
        protected List<Entity> children;
        protected string name;
        protected string filePath;
        protected int size;

        public Entity(Entity _parent, string _name, string _filePath)
        {
            parent = _parent;
            children = new List<Entity>();
            if(!Regex.IsMatch(_name, alphanumericPattern))
            {
                throw new Exception("Name can only be alphanumerical characters");
            }
            name = _name;
            filePath = _filePath;
            size = CalculateSize();
            if(parent != null && parent.children != null)
            {
                parent.children.Add(this);
            }
        }

        public string GetFilePath()
        {
            return filePath;
        }

        public virtual int CalculateSize()
        {
            size = 0;
            foreach (Entity e in children)
            {
                size += e.CalculateSize();
            }
            return size;
        }

        public void RemoveFromFileSystem()
        {
            if(this.parent != null)
            {
                this.parent.children.Remove(this);
            }
        }

        public void ChangeParent(Entity newParent)
        {
            if(this.parent != null)
            {
                this.parent.children.Remove(this);
            }
            if(newParent != null)
            {
                this.filePath = String.Concat(newParent.GetFilePath() + "\\" + this.name);
                newParent.children.Add(this);
            }
        }

        public virtual void Write(string newContent)
        {
        }

        public Entity GetEntityAtPath(string path)
        {
            if (path == this.filePath)
            {
                return this;
            }
            foreach (Entity e in this.children)
            {
                if (path.Contains(e.GetFilePath()))
                {
                    return e.GetEntityAtPath(path);
                }
            }
            return null;
        }

        // Methods used for testing
        public List<string> GetAllPaths()
        {
            List<string> paths = new List<string>();
            paths.Add(filePath);
            if(children == null)
            {
                return paths;
            }
            foreach(Entity e in children)
            {
                paths.AddRange(e.GetAllPaths());
            }
            return paths;
        }
    }
}
