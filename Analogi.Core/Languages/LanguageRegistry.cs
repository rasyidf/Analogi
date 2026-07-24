using Analogi.Core.Interfaces;

namespace Analogi.Core.Languages;

public sealed class LanguageRegistry
{
    private readonly List<ILanguageProfile> _profiles = [];

    public IReadOnlyList<ILanguageProfile> Profiles => _profiles;

    public LanguageRegistry()
    {
        // Built-in profiles
        _profiles.Add(new CppProfile());
        _profiles.Add(new PythonProfile());
        _profiles.Add(new JavaProfile());
        _profiles.Add(new CSharpProfile());
        _profiles.Add(new JavaScriptProfile());
    }

    public void Register(ILanguageProfile profile) => _profiles.Add(profile);

    public ILanguageProfile? GetByExtension(string extension)
    {
        return _profiles.FirstOrDefault(p =>
            p.FileExtensions.Any(ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase)));
    }

    /// <summary>All file extensions supported by registered profiles.</summary>
    public IEnumerable<string> AllExtensions => _profiles.SelectMany(p => p.FileExtensions);
}
