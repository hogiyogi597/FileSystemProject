using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    class TextFile : Entity
    {
        //parent cannot be null
        //cannot have children

        private string content;

        public override int CalculateSize()
        {
            size = content.Length;
            return size;
        }

        public override int WriteToFile(string srcPath, string newContent)
        {
            // Base case : At text file
            if (srcPath != filePath)
            {
                throw new Exception("File not found!");
            }

            content = newContent;
            // Modifies the TextFile.content to be newContent

            // Recalculate size
            return CalculateSize();
        }
    }
}
