using Analogi.Core.Analyzers;
using Analogi.Core.Extractors;
using Analogi.Core.Languages;
using Analogi.Core.Models;
using Analogi.Core.PreProcessors;

namespace Analogi.Tests;

public class PipelineTests
{
    private static PipelineContext CreateContext(string codeA, string codeB)
    {
        // Write temp files for CodeFile
        var dirA = Path.Combine(Path.GetTempPath(), "analogi_test_a_" + Guid.NewGuid());
        var dirB = Path.Combine(Path.GetTempPath(), "analogi_test_b_" + Guid.NewGuid());
        Directory.CreateDirectory(Path.GetDirectoryName(dirA + ".cpp")!);
        var pathA = dirA + ".cpp";
        var pathB = dirB + ".cpp";
        File.WriteAllText(pathA, codeA);
        File.WriteAllText(pathB, codeB);

        var fileA = new CodeFile(pathA);
        var fileB = new CodeFile(pathB);
        return new PipelineContext(fileA, fileB, new CppProfile());
    }

    [Fact]
    public void Identical_code_produces_high_similarity()
    {
        var code = """
            #include <iostream>
            using namespace std;
            int main() {
                int n = 10;
                cout << n << endl;
                return 0;
            }
            """;

        var ctx = CreateContext(code, code);
        var steps = Analogi.Core.Pipeline.AnalysisEngine.DefaultPipeline();
        foreach (var step in steps) ctx = step.Run(ctx);

        Assert.NotEmpty(ctx.Reasons);
        Assert.Contains(ctx.Reasons, r => r.AnalyzerName == "CosineSimilarity");
    }

    [Fact]
    public void Completely_different_code_produces_no_reasons()
    {
        var codeA = """
            #include <iostream>
            int main() {
                int x = 1;
                return x;
            }
            """;
        var codeB = """
            #include <cmath>
            double compute(double a, double b) {
                return sqrt(a * a + b * b);
            }
            int main() {
                return 0;
            }
            """;

        var ctx = CreateContext(codeA, codeB);
        var steps = Analogi.Core.Pipeline.AnalysisEngine.DefaultPipeline();
        foreach (var step in steps) ctx = step.Run(ctx);

        // Should have very few or no high-score reasons
        var highReasons = ctx.Reasons.Where(r => r.Score > 0.8).ToList();
        Assert.Empty(highReasons);
    }

    [Fact]
    public void StringLiteralNormalize_removes_string_differences()
    {
        var codeA = """
            #include <iostream>
            int main() {
                cout << "Hello World" << endl;
                return 0;
            }
            """;
        var codeB = """
            #include <iostream>
            int main() {
                cout << "Goodbye Earth" << endl;
                return 0;
            }
            """;

        var ctx = CreateContext(codeA, codeB);

        // Run extractors + preprocessors only
        new CodeExtractor().Run(ctx);
        new CaseFold().Run(ctx);
        new WhitespaceNormalize().Run(ctx);
        new StringLiteralNormalize().Run(ctx);

        // After normalization, code should be identical (strings replaced with __STR__)
        var normalizedA = string.Join("\n", ctx.GetMetadata("code.a"));
        var normalizedB = string.Join("\n", ctx.GetMetadata("code.b"));
        Assert.Equal(normalizedA, normalizedB);
    }

    [Fact]
    public void IdentifierNormalize_catches_renamed_variables()
    {
        var codeA = """
            int main() {
                int counter = 0;
                counter = counter + 1;
                return counter;
            }
            """;
        var codeB = """
            int main() {
                int total = 0;
                total = total + 1;
                return total;
            }
            """;

        var ctx = CreateContext(codeA, codeB);
        new CodeExtractor().Run(ctx);
        new CaseFold().Run(ctx);
        new WhitespaceNormalize().Run(ctx);
        new IdentifierNormalize().Run(ctx);

        var normalizedA = string.Join("\n", ctx.GetMetadata("code.a"));
        var normalizedB = string.Join("\n", ctx.GetMetadata("code.b"));
        Assert.Equal(normalizedA, normalizedB);
    }
}
