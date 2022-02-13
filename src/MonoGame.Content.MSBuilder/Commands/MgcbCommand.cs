namespace MonoGame.Content.MSBuilder.Commands
{
  /// <summary>
  /// Class that represents logical MGCB command.
  /// </summary>
  public class MgcbCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MgcbCommand"/> class.
    /// </summary>
    /// <param name="arguments">A set of arguments that represents this MGCB command.</param>
    public MgcbCommand(string[] arguments)
    {
      Arguments = arguments;
    }

    /// <summary>
    /// Gets a set of arguments that represents this MGCB command.
    /// </summary>
    public string[] Arguments { get; }
  }
}
