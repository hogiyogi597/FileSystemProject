/*
 * Base class for an Entity. Used as an abstract class to be derived from in each entity's class.
 * Holds the basic characteristics and methods to help with creation, deletion, moving, and writing.
 * 
 * Created by Stephen Hogan - 10/23/17
 */

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
        protected string alphanumericPattern = "^[A-Za-z0-9_]*$"; // Forces names to be alphanumerical
        protected Entity parent; // References the parent of the entity
        protected Dictionary<string, Entity> children; // Holds the children of the entity (file path, Entity)
        protected string name;
        protected string filePath;
        protected int size;

        public Entity(Entity _parent, string _name, string _filePath)
        {
            
            parent = _parent;
            children = new Dictionary<string, Entity>();
            if(!Regex.IsMatch(_name, alphanumericPattern))
            {
                throw new Exception("Name can only be alphanumerical characters");
            }
            name = _name;
            filePath = _filePath;
            size = CalculateSize();
            if(parent != null && parent.children != null)
            {
                if (_parent.children.ContainsKey(_filePath))
                {
                    throw new Exception("Cannot add duplicate names!");
                }
                parent.children.Add(filePath, this);
            }
        }

        public virtual int CalculateSize()
        {
            size = 0;
            foreach (KeyValuePair<string, Entity> e in children)
            {
                size += e.Value.CalculateSize();
            }
            return size;
        }

        public void RemoveFromFileSystem()
        {
            if(this.parent != null)
            {
                this.parent.children.Remove(this.filePath);
            }
        }

        public virtual void ChangeParent(Entity newParent)
        {
            if(this is Drive && newParent != null)
            {
                throw new Exception("Drives cannot have a parent!");
            }
            if((this is Folder || this is ZipFile) && newParent == null)
            {
                throw new Exception("Folders and Zip files must be contained in a Drive, Folder, or Zip!");
            }
            if (newParent is TextFile)
            {
                throw new Exception("Text files cannot hold entities!");
            }

            RemoveFromFileSystem();
            if(newParent != null)
            {
                this.filePath = String.Concat(newParent.filePath + "\\" + this.name);
                newParent.children.Add(this.filePath, this);
            }
        }

        public virtual void Write(string newContent) { }

        public Entity GetEntityAtPath(string path)
        {
            if (path == this.filePath)
            {
                return this;
            }
            foreach(KeyValuePair<string, Entity> e in this.children)
            {
                if (path.Contains(e.Key))
                {
                    return e.Value.GetEntityAtPath(path);
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
            foreach (KeyValuePair<string, Entity> e in this.children)
            {
                paths.AddRange(e.Value.GetAllPaths());
            }
            return paths;
        }
    }
}
