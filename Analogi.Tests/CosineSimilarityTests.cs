using Analogi.Core.Algorithm;

namespace Analogi.Tests;

public class CosineSimilarityTests
{
    [Fact]
    public void Identical_strings_return_1()
    {
        var score = CosineSimilarity.Compute("hello world", "hello world");
        Assert.Equal(1.0, score, precision: 5);
    }

    [Fact]
    public void Completely_different_strings_return_0()
    {
        var score = CosineSimilarity.Compute("aaa bbb ccc", "xxx yyy zzz");
        Assert.Equal(0.0, score, precision: 5);
    }

    [Fact]
    public void Null_or_empty_returns_0()
    {
        Assert.Equal(0.0, CosineSimilarity.Compute("", "hello"));
        Assert.Equal(0.0, CosineSimilarity.Compute("hello", ""));
        Assert.Equal(0.0, CosineSimilarity.Compute(null!, "hello"));
    }

    [Fact]
    public void Partial_overlap_returns_between_0_and_1()
    {
        var score = CosineSimilarity.Compute("the cat sat on the mat", "the cat sat on the hat");
        Assert.InRange(score, 0.5, 0.99);
    }
}
