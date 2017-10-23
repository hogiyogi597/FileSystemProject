/*
 * A representation of a file system with four different types of entities: Drives, Folders, Zip Files, and Text Files.
 * There are four operations available: Create, Delete, Move, and WriteToFile.
 * 
 * Created by Stephen Hogan - 10/23/2017
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    public class FileExplorer
    {
        public enum EntityTypes { Drive, Folder, ZipFile, TextFile }; // Enums the four entity types
        private Dictionary<string, Drive> drives; // holds each drive (file path, Drive)

        public FileExplorer()
        {
            drives = new Dictionary<string, Drive>();
        }

        public void Create(EntityTypes type, string name, string pathToParent)
        {
            Entity parent = null;
            foreach(KeyValuePair<string, Drive> d in drives)
            {
                parent = d.Value.GetEntityAtPath(pathToParent);
            }

            switch(type)
            {
                case EntityTypes.Drive:
                    if (drives.ContainsKey(String.Concat("\\", name)))
                    {
                        throw new Exception("Cannot create a drive with a duplicate name!");
                    }
                    Drive d = new Drive(parent, name, String.Concat("\\" + name));
                    this.drives.Add(String.Concat("\\" + name), d);
                    break;
                case EntityTypes.Folder:
                    new Folder(parent, name, String.Concat(pathToParent, "\\", name));
                    break;
                case EntityTypes.ZipFile:
                    new ZipFile(parent, name, String.Concat(pathToParent, "\\", name));
                    break;
                case EntityTypes.TextFile:
                    new TextFile(parent, name, String.Concat(pathToParent, "\\", name), "");
                    break;
                default:
                    throw new Exception("Invalid type");
            }
        }

        public void Delete(string path)
        {
            Entity entityToDelete = null;
            foreach (KeyValuePair<string, Drive> d in drives)
            {
                if(d.Key == path)
                {
                    drives.Remove(d.Key);
                    return;
                }
                entityToDelete = d.Value.GetEntityAtPath(path);
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
            foreach (KeyValuePair<string, Drive> d in drives)
            {
                if(entityToMove == null)
                {
                    entityToMove = d.Value.GetEntityAtPath(srcPath);
                }
                if(newParent == null)
                {
                    newParent = d.Value.GetEntityAtPath(destPath);
                }
            }
            entityToMove.ChangeParent(newParent);
        }

        public void WriteToFile(string srcPath, string newContent)
        {
            Entity targetEntity = null;
            Drive driveContainer = null;
            foreach (KeyValuePair<string, Drive> d in drives)
            {
                targetEntity = d.Value.GetEntityAtPath(srcPath);
                if(targetEntity != null)
                {
                    driveContainer = d.Value;
                    break;
                }
            }
            if (targetEntity == null)
            {
                // Create file or throw an exception
                throw new Exception("Text file does not exist");
            }
            if(!(targetEntity is TextFile))
            {
                throw new Exception("Cannot write to a file of type " + targetEntity.GetType() + "!");
            }
            targetEntity.Write(newContent);
            driveContainer.CalculateSize();
        }




        // methods used for testing.
        public int GetSize()
        {
            int size = 0;
            foreach (KeyValuePair<string, Drive> d in drives)
            {
                size += d.Value.CalculateSize();
            }
            return size;
        }

        public List<string> GetPathsOfAllEntities()
        {
            List<string> paths = new List<string>();
            foreach (KeyValuePair<string, Drive> d in drives)
            {
                paths.AddRange(d.Value.GetAllPaths());
            }
            return paths;
        }
    }
}