using System;
using System.IO;

namespace Analogi
{
    internal class ScriptItem
    {
        public ScriptItem(string url)
        {
            this.Path = url;
            var a = new FileInfo(url);
            this.Name = a.Name;
            this.CreationTime = a.CreationTime;
            this.Length = a.Length;
        }

        public string Name { get; private set; }
        public DateTime CreationTime { get; set; }
        public long Length { get; private set; }
        public string Path { get; set; }
        public int Index { get; set; }

        internal string ReadAll()
        {
            return File.ReadAllText(Path);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}