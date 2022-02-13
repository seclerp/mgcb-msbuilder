using CliWrap;

namespace MonoGame.Content.MSBuilder.Commands.Execution
{
  /// <summary>
  /// Command executor implementation that calls commands using a dotnet-mgcb tool.
  /// </summary>
  public class MgcbToolCommandExecutor : MgcbCommandExecutorBase
  {
    private readonly string _workingDirectory;
    private readonly string? _additionalArguments;

    /// <summary>
    /// Initializes a new instance of the <see cref="MgcbToolCommandExecutor"/> class.
    /// </summary>
    /// <param name="workingDirectory">Working directory for the command line execution.</param>
    /// <param name="additionalArguments">Additional command line arguments that should be passed to the MGCB.</param>
    public MgcbToolCommandExecutor(string workingDirectory, string? additionalArguments = null)
    {
      _workingDirectory = workingDirectory;
      _additionalArguments = additionalArguments;
    }

    /// <inheritdoc />
    protected override Command PrepareCliCommand(MgcbCommand command) =>
      Cli.Wrap("mgcb")
        .WithWorkingDirectory(_workingDirectory)
        .WithArguments(builder =>
        {
          builder.Add(command.Arguments);
          if (!string.IsNullOrWhiteSpace(_additionalArguments))
          {
            builder.Add(_additionalArguments);
          }
        });
  }
}
