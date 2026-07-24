using Analogi.Core.Algorithm;
using Analogi.Core.Pipeline;

namespace Analogi.Tests;

public class WinnowingTests
{
    [Fact]
    public void Identical_tokens_produce_similarity_1()
    {
        var tokens = new List<string> { "int", "main", "return", "0", "end", "print", "hello" };
        var fpA = Winnowing.Fingerprint(tokens);
        var fpB = Winnowing.Fingerprint(tokens);

        Assert.Equal(1.0, Winnowing.Similarity(fpA, fpB), precision: 5);
    }

    [Fact]
    public void Completely_different_tokens_produce_low_similarity()
    {
        var tokensA = new List<string> { "int", "main", "return", "zero", "end", "print", "hello" };
        var tokensB = new List<string> { "def", "calc", "yield", "sum", "pass", "lambda", "async" };

        var fpA = Winnowing.Fingerprint(tokensA);
        var fpB = Winnowing.Fingerprint(tokensB);

        Assert.True(Winnowing.Similarity(fpA, fpB) < 0.3);
    }

    [Fact]
    public void Tokens_with_one_edit_still_share_fingerprints()
    {
        // Need enough tokens that one edit doesn't destroy all k-grams
        var tokensA = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o" };
        var tokensB = new List<string> { "a", "b", "c", "X", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o" }; // one token changed

        var fpA = Winnowing.Fingerprint(tokensA);
        var fpB = Winnowing.Fingerprint(tokensB);

        // Should still have significant overlap despite the edit
        var similarity = Winnowing.Similarity(fpA, fpB);
        Assert.True(similarity > 0.2, $"Expected > 0.2, got {similarity}");
    }

    [Fact]
    public void Too_few_tokens_returns_empty_fingerprints()
    {
        var tokens = new List<string> { "a", "b" };
        var fp = Winnowing.Fingerprint(tokens, kgramSize: 5);
        Assert.Empty(fp);
    }

    [Fact]
    public void Empty_sets_return_0_similarity()
    {
        Assert.Equal(0.0, Winnowing.Similarity([], []));
    }
}

public class SubmissionTests
{
    [Fact]
    public async Task ScanSubmissions_detects_copied_subfolders()
    {
        // Create temp directory structure simulating student submissions
        var root = Path.Combine(Path.GetTempPath(), "analogi_sub_test_" + Guid.NewGuid());
        var studentA = Path.Combine(root, "student_a");
        var studentB = Path.Combine(root, "student_b");
        Directory.CreateDirectory(studentA);
        Directory.CreateDirectory(studentB);

        var code = """
            #include <iostream>
            using namespace std;
            int fibonacci(int n) {
                if (n <= 1) return n;
                return fibonacci(n-1) + fibonacci(n-2);
            }
            int main() {
                int n = 10;
                cout << fibonacci(n) << endl;
                return 0;
            }
            """;

        // Student B copied student A with minor variable rename
        var codeB = code.Replace("fibonacci", "fib").Replace("n", "x");

        File.WriteAllText(Path.Combine(studentA, "main.cpp"), code);
        File.WriteAllText(Path.Combine(studentB, "main.cpp"), codeB);

        try
        {
            var engine = new AnalysisEngine();
            var result = await engine.ScanSubmissionsAsync(root);

            Assert.Equal(2, result.TotalSubmissions);
            Assert.Single(result.Pairs);
            Assert.True(result.Pairs[0].SimilarityIndex > 0.5,
                $"Expected > 0.5 similarity, got {result.Pairs[0].SimilarityIndex}");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task ScanSubmissions_with_multi_file_submissions()
    {
        var root = Path.Combine(Path.GetTempPath(), "analogi_multi_test_" + Guid.NewGuid());
        var studentA = Path.Combine(root, "alice");
        var studentB = Path.Combine(root, "bob");
        Directory.CreateDirectory(studentA);
        Directory.CreateDirectory(studentB);

        var header = "#include <iostream>\nusing namespace std;\n";
        var mainCode = "int main() { cout << \"hello\" << endl; return 0; }";
        var utilCode = "int add(int a, int b) { return a + b; }";

        // Both students have same structure
        File.WriteAllText(Path.Combine(studentA, "main.cpp"), header + mainCode);
        File.WriteAllText(Path.Combine(studentA, "util.cpp"), header + utilCode);
        File.WriteAllText(Path.Combine(studentB, "app.cpp"), header + mainCode);
        File.WriteAllText(Path.Combine(studentB, "helper.cpp"), header + utilCode);

        try
        {
            var engine = new AnalysisEngine();
            var result = await engine.ScanSubmissionsAsync(root);

            Assert.Equal(2, result.TotalSubmissions);
            Assert.Single(result.Pairs);

            var pair = result.Pairs[0];
            // Should have cross-file matches
            Assert.True(pair.FilePairDetails.Count >= 2,
                $"Expected >= 2 file pair matches, got {pair.FilePairDetails.Count}");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
