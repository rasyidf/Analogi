/*
 * MIT
 */

using System;
using System.IO;

namespace rasyidf.Analogi.Core
{
    public class CodeFile
    {
        #region Constructors

        public CodeFile(string url)
        {
            Path = url;
            var a = new FileInfo(url);
            Name = a.Name;
            CreationTime = a.CreationTime;
            Length = a.Length;
        }

        #endregion Constructors

        #region Properties

        public DateTime CreationTime { get; set; }
        public int Index { get; set; }
        public long Length { get; private set; }
        public string Name { get; private set; }
        public string Path { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        internal string ReadAll()
        {
            return File.ReadAllText(Path);
        }

        #endregion Methods
    }
}