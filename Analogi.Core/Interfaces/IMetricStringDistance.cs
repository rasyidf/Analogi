/*
 * MIT
 */

namespace rasyidf.Analogi.Interfaces
{
    /// <summary>
    /// String distances that implement this interface are metrics, which means:
    ///  - d(x, y) ≥ 0     (non-negativity, or separation axiom)
    ///  - d(x, y) = 0   if and only if   x = y     (identity, or coincidence axiom)
    ///  - d(x, y) = d(y, x)     (symmetry)
    ///  - d(x, z) ≤ d(x, y) + d(y, z)     (triangle inequality).
    /// </summary>
    public interface IMetricStringDistance : IStringDistance
    {
        /// <summary>
        /// Compute and return the metric distance.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        new double Distance(string s1, string s2);
    }
}