namespace BashSoft.IO
{
    using Exceptions;
    using Static_data;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class IOManager
    {
        public void TraverseDirectory(int depth)
        {
            OutputWriter.WriteEmptyLine();
            var initialIdentation = SessionData.CurrentPath.Split('\\').Length;
            var subFolders = new Queue<string>();
            subFolders.Enqueue(SessionData.CurrentPath);

            while (true)
            {
                if (subFolders.Count == 0)
                {
                    break;
                }

                var currentPath = subFolders.Dequeue();
                var identation = currentPath.Split('\\').Length - initialIdentation;

                if (depth - identation < 0)
                {
                    break;
                }

                OutputWriter.WriteMessageOnNewLine($"{new string('-', identation)}{currentPath}");

                try
                {
                    foreach (var file in Directory.GetFiles(currentPath))
                    {
                        var indexOfLastSlash = file.LastIndexOf("\\");
                        var filename = file.Substring(indexOfLastSlash);
                        OutputWriter.WriteMessageOnNewLine(new string('-', indexOfLastSlash) + filename);
                    }

                    var subDirectories = Directory.GetDirectories(currentPath);

                    foreach (var subDir in subDirectories)
                    {
                        subFolders.Enqueue(subDir);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    OutputWriter.DisplayException(ExceptionMessage.UnauthorizedAccessException);
                }
            }
        }

        public void ChangeCurrentDirectoryRelative(string relativePath)
        {
            if (relativePath == "..")
            {
                try
                {
                    var currentPath = SessionData.CurrentPath;
                    var indexOfLastSlash = currentPath.LastIndexOf("\\");
                    var newPath = currentPath.Substring(0, indexOfLastSlash);
                    SessionData.CurrentPath = newPath;
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException("indexOfSlash", ExceptionMessage.InvalidDestionation);
                }
            }
            else
            {
                var currentPath = SessionData.CurrentPath;
                currentPath += "\\" + relativePath;
                ChangeCurrentDirectoryAbsolute(currentPath);
            }
        }

        public void ChangeCurrentDirectoryAbsolute(string absolutePath)
        {
            if (!Directory.Exists(absolutePath))
            {
                throw new InvalidPathException();
            }

            SessionData.CurrentPath = absolutePath;
        }

        public void CreateDirectoryInCurrentFolder(string name)
        {
            try
            {
                var path = SessionData.CurrentPath + '\\' + name;
                Directory.CreateDirectory(path);
            }
            catch (ArgumentException)
            {
                throw new InvalidFileNameException();
            }
        }
    }
}