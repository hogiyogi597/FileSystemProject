using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    public class FileExplorer
    {
        public enum EntityTypes { Drive, Folder, ZipFile, TextFile };
        List<Drive> drives;
        //Dictionary<string, Drive> something;

        public FileExplorer()
        {
            drives = new List<Drive>();
        }

        public Entity Create(EntityTypes type, string name, string pathToParent)
        {
            Entity parent = null;
            HashSet<string> pathsOfDrives = new HashSet<string>();
            foreach(Drive d in drives)
            {
                parent = d.GetEntityAtPath(pathToParent);
                pathsOfDrives.Add(d.GetFilePath());
            }
            if(type == EntityTypes.Drive)
            {
                if (pathsOfDrives.Contains(String.Concat("\\", name)))
                {
                    throw new Exception("Cannot create a drive with a duplicate name!");
                }
                Drive d = new Drive(parent, name, String.Concat("\\" + name));
                this.drives.Add(d);
                return d;
            }
            else if (type == EntityTypes.Folder)
            {
                return new Folder(parent, name, String.Concat(pathToParent, "\\", name));
            }
            else if (type == EntityTypes.ZipFile)
            {
                return new ZipFile(parent, name, String.Concat(pathToParent, "\\", name));
            }
            else if (type == EntityTypes.TextFile)
            {
                return new TextFile(parent, name, String.Concat(pathToParent, "\\", name), "");
            }
            else
            {
                throw new Exception("Invalid type");
            }
        }

        public void Delete(string path)
        {
            Entity entityToDelete = null;
            foreach (Drive d in drives)
            {
                if(d.GetFilePath() == path)
                {
                    drives.Remove(d);
                    return;
                }
                entityToDelete = d.GetEntityAtPath(path);
                if (entityToDelete != null)
                {
                    break;
                }
            }
            if(entityToDelete != null)
            {
                entityToDelete.RemoveFromFileSystem();
            }
        }

        public void Move(string srcPath, string destPath)
        {
            Entity entityToMove = null;
            Entity newParent = null;
            foreach(Drive d in drives)
            {
                entityToMove = d.GetEntityAtPath(srcPath);
                newParent = d.GetEntityAtPath(destPath);
            }
            entityToMove.ChangeParent(newParent);
        }

        public void WriteToFile(string srcPath, string newContent)
        {
            Entity targetEntity = null;
            Drive driveContainer = null;
            foreach(Drive d in drives)
            {
                targetEntity = d.GetEntityAtPath(srcPath);
                if(targetEntity != null)
                {
                    driveContainer = d;
                    break;
                }
            }
            if (targetEntity == null)
            {
                // Create file or throw an exception
                throw new Exception("Text file does not exist");
            }
            targetEntity.Write(newContent);
            driveContainer.CalculateSize();
        }

        // methods used for testing.
        public int GetSize()
        {
            int size = 0;
            foreach (Drive d in drives)
            {
                size += d.CalculateSize();
            }
            return size;
        }

        public List<string> GetPathsOfAllEntities()
        {
            List<string> paths = new List<string>();
            foreach(Drive d in drives)
            {
                paths.AddRange(d.GetAllPaths());
            }
            return paths;
        }
    }
}