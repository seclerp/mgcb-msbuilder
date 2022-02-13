using System;

namespace MonoGame.Content.MSBuilder.Exceptions
{
  /// <summary>
  /// Exception that may occur during content build process.
  /// </summary>
  public class MgcbExecutionException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MgcbExecutionException"/> class.
    /// </summary>
    /// <param name="output">Standard output of the MGCB command.</param>
    /// <param name="error">Error output of the MGCB command.</param>
    /// <param name="exitCode">Exit code of the MGCB command.</param>
    public MgcbExecutionException(string output, string error, int exitCode)
      : base($"Error executing MGCB command:\nOutput: {output}\nError: {error}\nExit code: {exitCode}")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MgcbExecutionException"/> class.
    /// </summary>
    /// <param name="output">Standard output of the MGCB command.</param>
    /// <param name="error">Error output of the MGCB command.</param>
    /// <param name="inner">Inner exception instance.</param>
    public MgcbExecutionException(string output, string error, Exception inner)
      : base($"Error executing MGCB command:\nOutput: {output}\nError: {error}", inner)
    {
    }
  }
}
