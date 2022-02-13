using CliWrap;

namespace MonoGame.Content.MSBuilder.Commands.Execution
{
  /// <summary>
  /// Command executor implementation that calls commands using a path to MGCB dotnet assembly.
  /// </summary>
  public class MgcbDllCommandExecutor : MgcbCommandExecutorBase
  {
    private readonly string _workingDirectory;
    private readonly string _dllPath;
    private readonly string? _additionalArguments;

    /// <summary>
    /// Initializes a new instance of the <see cref="MgcbDllCommandExecutor"/> class.
    /// </summary>
    /// <param name="workingDirectory">Working directory for the command line execution.</param>
    /// <param name="dllPath">Path to the MGCB DLL.</param>
    /// <param name="additionalArguments">Additional command line arguments that should be passed to the MGCB.</param>
    public MgcbDllCommandExecutor(string workingDirectory, string dllPath, string? additionalArguments = null)
    {
      _workingDirectory = workingDirectory;
      _dllPath = dllPath;
      _additionalArguments = additionalArguments;
    }

    /// <inheritdoc/>
    protected override Command PrepareCliCommand(MgcbCommand command) =>
      Cli.Wrap("dotnet")
        .WithWorkingDirectory(_workingDirectory)
        .WithArguments(builder =>
        {
          builder.Add(_dllPath);
          builder.Add(command.Arguments);
          if (!string.IsNullOrWhiteSpace(_additionalArguments))
          {
            builder.Add(_additionalArguments);
          }
        });
  }
}
