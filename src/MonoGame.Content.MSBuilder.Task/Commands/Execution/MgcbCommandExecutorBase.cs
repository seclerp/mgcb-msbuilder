using System;
using System.Text;
using CliWrap;
using MonoGame.Content.MSBuilder.Task.Exceptions;

namespace MonoGame.Content.MSBuilder.Task.Commands.Execution
{
  public abstract class MgcbCommandExecutorBase
  {
    protected abstract Command PrepareCliCommand(MgcbCommand command);

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
  }
}
