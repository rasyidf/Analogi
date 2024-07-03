/*
 * MIT
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Analogi.Core.Models
{
    public class CodeFile
    {
        #region Constructors

        public CodeFile(string url)
        {
            Files = [];
            Path = url;
            if (Directory.Exists(url))
            {
                Length = 0;
                foreach (string item in Directory.EnumerateFiles(url))
                {
                    Files.Add(item);
                    try
                    {
                        FileInfo ua = new(item);
                        Length += ua.Length;
                    }
                    catch
                    {
                        Length += 0;
                    }

                }

                DirectoryInfo a = new(url);
                Name = a.Name;
                CreationTime = a.CreationTime;

            }
            else
            {
                Files.Add(url);

                FileInfo a = new(url);
                Name = a.Name;
                CreationTime = a.CreationTime;
                Length = a.Length;
            }

        }

        internal string GetFile()
        {
            return Files.Count == 0 ? "" : Files[0];
        }

        #endregion Constructors

        #region Properties

        public DateTime CreationTime { get; set; }
        public int Index { get; set; }
        public long Length { get; private set; }
        public string Name { get; private set; }
        public string Path { get; set; }
        public List<string> Files { get; private set; }
        public bool IsDirectory { get; private set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        internal string ReadAll()
        {
            return Files.Count == 0 ? "" : File.ReadAllText(Files[0]);
        }

        #endregion Methods
    }
}