using CliWrap;

namespace MonoGame.Content.MSBuilder.Task.Commands.Execution
{
  class MgcbToolCommandExecutor : MgcbCommandExecutorBase
  {
    private readonly string _workingDirectory;

    public MgcbToolCommandExecutor(string workingDirectory)
    {
      _workingDirectory = workingDirectory;
    }

    protected override Command PrepareCliCommand(MgcbCommand command) =>
      Cli.Wrap("mgcb")
        .WithWorkingDirectory(_workingDirectory)
        .WithArguments(builder =>
        {
          builder.Add(command.Args);
        });
  }
}
