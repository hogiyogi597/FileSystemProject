using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileSystem;
using System.Collections.Generic;

namespace FileSystemTests
{
    [TestClass]
    public class FileSystemTests
    {
        [TestMethod]
        public void BasicFileSystemFunctionality()
        {
            FileExplorer fs = new FileExplorer();
            fs.Create(FileExplorer.EntityTypes.Drive, "C", "");
            Assert.AreEqual(fs.GetSize(), 0);
            fs.Create(FileExplorer.EntityTypes.Folder, "Desktop", "\\C");
            Assert.AreEqual(fs.GetSize(), 0);
            fs.Create(FileExplorer.EntityTypes.ZipFile, "zip1", "\\C\\Desktop");
            Assert.AreEqual(fs.GetSize(), 0);

            // Adding a text file to a zip should half the size of the string
            fs.Create(FileExplorer.EntityTypes.TextFile, "file1", "\\C\\Desktop\\zip1");
            fs.WriteToFile("\\C\\Desktop\\zip1\\file1", "hello world!");
            Assert.AreEqual(fs.GetSize(), 6);

            // Creating an empty text file should not change the size
            fs.Create(FileExplorer.EntityTypes.TextFile, "file1", "\\C\\Desktop");
            Assert.AreEqual(fs.GetSize(), 6);
            // The size should not be compressed 
            fs.WriteToFile("\\C\\Desktop\\file1", "hello, Mark!");
            Assert.AreEqual(fs.GetSize(), 18);
            // Moving a file should change the size because it is no longer compressed
            fs.Move("\\C\\Desktop\\zip1\\file1", "\\C");
            Assert.AreEqual(fs.GetPathsOfAllEntities().Count, 5);
            Assert.AreEqual(fs.GetSize(), 24);

            fs.Delete("\\C\\Desktop");
            Assert.AreEqual(fs.GetPathsOfAllEntities().Count, 2);
            Assert.AreEqual(fs.GetSize(), 12);
        }

        [TestMethod]
        public void ErrorsAndExceptions()
        {
            FileExplorer fs = new FileExplorer();
            fs.Create(FileExplorer.EntityTypes.Drive, "D", "");
            bool passed = false;
            try
            {
                fs.Create(FileExplorer.EntityTypes.Drive, "C", "\\D");
            }
            catch
            {
                passed = true;
            }
            if(!passed)
            {
                Assert.Fail();
            }

            passed = false;
            try
            {
                fs.Create(FileExplorer.EntityTypes.Drive, "X!@#$!@$", "");
            }
            catch
            {
                passed = true;
            }
            if (!passed)
            {
                Assert.Fail();
            }

            passed = false;
            try
            {
                fs.Create(FileExplorer.EntityTypes.Drive, "D", "");
            }
            catch
            {
                passed = true;
            }
            if (!passed)
            {
                Assert.Fail();
            }

            passed = false;
            try
            {
                fs.Create(FileExplorer.EntityTypes.Folder, "folder1", "");
            }
            catch
            {
                passed = true;
            }
            if (!passed)
            {
                Assert.Fail();
            }
            passed = false;
            try
            {
                fs.Create(FileExplorer.EntityTypes.TextFile, "file1", "");
            }
            catch
            {
                passed = true;
            }
            if (!passed)
            {
                Assert.Fail();
            }
            passed = false;
            try
            {
                fs.Create(FileExplorer.EntityTypes.ZipFile, "zip1", "");
            }
            catch
            {
                passed = true;
            }
            if (!passed)
            {
                Assert.Fail();
            }

            fs.Create(FileExplorer.EntityTypes.TextFile, "file1", "\\D");
            passed = false;
            try
            {
                // I should not be able to add a text file as a child of a text file.
                fs.Create(FileExplorer.EntityTypes.TextFile, "file2", "\\D\\file1");
            }
            catch
            {
                passed = true;
            }
            if (!passed)
            {
                Assert.Fail();
            }
            

            fs.Delete("\\D");



        }
    }
}
