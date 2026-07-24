namespace Analogi.Core.Models;

public sealed class CodeFile
{
    public string Path { get; }
    public string Name { get; }
    public string Content { get; }
    public long Length { get; }

    public CodeFile(string path)
    {
        Path = path;
        var info = new FileInfo(path);
        Name = info.Name;
        Length = info.Length;
        Content = File.ReadAllText(path);
    }

    public override string ToString() => Name;
}
