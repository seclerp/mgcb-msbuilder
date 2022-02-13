using System;
using System.Text;
using CliWrap;
using MonoGame.Content.MSBuilder.Exceptions;

namespace MonoGame.Content.MSBuilder.Commands.Execution
{
  /// <summary>
  /// Base class for a command executor implementation.
  /// </summary>
  public abstract class MgcbCommandExecutorBase
  {
    /// <summary>
    /// Executes given command.
    /// </summary>
    /// <param name="command">A logical command instance.</param>
    /// <exception cref="MgcbExecutionException">Thrown when exit code of MGCB executable is not succeed or when unexpected exception occured.</exception>
    public void Execute(MgcbCommand command)
    {
      var outputBuilder = new StringBuilder();
      var errorBuilder = new StringBuilder();

      try
      {
        var result = PrepareCliCommand(command)
          .WithStandardOutputPipe(PipeTarget.ToStringBuilder(outputBuilder))
          .WithStandardErrorPipe(PipeTarget.ToStringBuilder(errorBuilder))
          .ExecuteAsync()
          .GetAwaiter()
          .GetResult();

        if (result.ExitCode != 0)
        {
          throw new MgcbExecutionException(outputBuilder.ToString(), errorBuilder.ToString(), result.ExitCode);
        }
      }
      catch (Exception ex)
      {
        throw new MgcbExecutionException(outputBuilder.ToString(), errorBuilder.ToString(), ex);
      }
    }

    /// <summary>
    /// Returns configured and ready-to-execute CLI command using logical command as an input.
    /// </summary>
    /// <param name="command">A logical command instance.</param>
    /// <returns>Configured CLI command to execute.</returns>
    protected abstract Command PrepareCliCommand(MgcbCommand command);
  }
}
