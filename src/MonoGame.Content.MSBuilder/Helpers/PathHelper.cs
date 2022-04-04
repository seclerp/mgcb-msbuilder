namespace MonoGame.Content.MSBuilder.Helpers
{
  /// <summary>
  /// Helper for string-represented paths.
  /// </summary>
  public static class PathHelper
  {
    /// <summary>
    /// Fixes paths in such way that could be used by MGCB pipeline in any platform.
    /// </summary>
    /// <param name="path">A path instance.</param>
    /// <returns>The normalized path instance.</returns>
    public static string NormalizeSeparators(string path) => path.Replace('\\', '/');
  }
}
