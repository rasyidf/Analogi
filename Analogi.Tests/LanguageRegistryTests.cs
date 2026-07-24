using Analogi.Core.Languages;

namespace Analogi.Tests;

public class LanguageRegistryTests
{
    private readonly LanguageRegistry _registry = new();

    [Theory]
    [InlineData(".cpp", "C/C++")]
    [InlineData(".c", "C/C++")]
    [InlineData(".py", "Python")]
    [InlineData(".java", "Java")]
    [InlineData(".cs", "C#")]
    [InlineData(".ts", "JavaScript/TypeScript")]
    [InlineData(".jsx", "JavaScript/TypeScript")]
    public void GetByExtension_returns_correct_profile(string ext, string expectedName)
    {
        var profile = _registry.GetByExtension(ext);
        Assert.NotNull(profile);
        Assert.Equal(expectedName, profile.Name);
    }

    [Fact]
    public void GetByExtension_returns_null_for_unknown()
    {
        Assert.Null(_registry.GetByExtension(".xyz"));
    }

    [Fact]
    public void CppProfile_matches_function_declarations()
    {
        var profile = new CppProfile();
        var code = "int fibonacci(int n) { return n; }";
        var match = profile.FunctionDeclaration.Match(code);
        Assert.True(match.Success);
        Assert.Equal("fibonacci", match.Groups["name"].Value);
    }

    [Fact]
    public void PythonProfile_matches_def()
    {
        var profile = new PythonProfile();
        var code = "def calculate(x, y):";
        var match = profile.FunctionDeclaration.Match(code);
        Assert.True(match.Success);
        Assert.Equal("calculate", match.Groups["name"].Value);
    }

    [Fact]
    public void JavaScriptProfile_matches_import_and_require()
    {
        var profile = new JavaScriptProfile();

        var es6 = "import { useState } from 'react';";
        var cjs = "const fs = require('fs');";

        Assert.True(profile.ImportStatement.IsMatch(es6));
        Assert.True(profile.ImportStatement.IsMatch(cjs));

        Assert.Equal("react", profile.ImportStatement.Match(es6).Groups["file"].Value);
        Assert.Equal("fs", profile.ImportStatement.Match(cjs).Groups["file"].Value);
    }
}
