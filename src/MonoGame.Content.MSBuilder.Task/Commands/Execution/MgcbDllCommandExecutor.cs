using CliWrap;

namespace MonoGame.Content.MSBuilder.Task.Commands.Execution
{
  class MgcbDllCommandExecutor : MgcbCommandExecutorBase
  {
    private readonly string _workingDirectory;
    private readonly string _dllPath;

    public MgcbDllCommandExecutor(string workingDirectory, string dllPath)
    {
      _workingDirectory = workingDirectory;
      _dllPath = dllPath;
    }

    protected override Command PrepareCliCommand(MgcbCommand command) =>
      Cli.Wrap("dotnet")
        .WithWorkingDirectory(_workingDirectory)
        .WithArguments(builder =>
        {
          builder.Add(_dllPath);
          builder.Add(command.Args);
        });
  }
}
