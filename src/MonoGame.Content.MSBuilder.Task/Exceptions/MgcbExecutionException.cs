using System;

namespace MonoGame.Content.MSBuilder.Task.Exceptions
{
  public class MgcbExecutionException : Exception
  {
    public MgcbExecutionException(string output, string error, int exitCode)
      : base($"Error executing MGCB command:\nOutput: {output}\nError: {error}\nExit code: {exitCode}")
    {
    }

    public MgcbExecutionException(string output, string error, Exception inner)
      : base($"Error executing MGCB command:\nOutput: {output}\nError: {error}", inner)
    {
    }
  }
}
