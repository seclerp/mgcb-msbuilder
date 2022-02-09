namespace MonoGame.Content.MSBuilder.Task
{
  public class MgcbCommand
  {
    public string Command { get; }

    public string Args { get; }

    public MgcbCommand(string command, string args)
    {
      Command = command;
      Args = args;
    }

    public void Execute()
    {
      // TODO
    }
  }
}
