/*
 * MIT
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Analogi.Core
{
    public class CodeFile
    {
        #region Constructors

        public CodeFile(string url)
        {
            Files = new List<string>();
            Path = url;
            if (Directory.Exists(url))
            {
                foreach (var item in Directory.EnumerateFiles(url))
                {
                    Files.Add(item);
                }

                var a = new DirectoryInfo(url);
                Name = a.Name;
                CreationTime = a.CreationTime;
                Length = 0;

            }
            else
            {
                Files.Add(url);

                var a = new FileInfo(url);
                Name = a.Name;
                CreationTime = a.CreationTime;
                Length = a.Length;
            }

        }

        internal string GetFile()
        {
            if (Files.Count == 0) return "";

            return Files[0];
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
            if (Files.Count == 0) return "";
            return File.ReadAllText(Files[0]);
        }

        #endregion Methods
    }
}