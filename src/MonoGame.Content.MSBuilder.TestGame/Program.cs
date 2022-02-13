using System;

namespace ExampleGame;

/// <summary>
/// Entry point container class.
/// </summary>
public static class Program
{
  /// <summary>
  /// Entry point method.
  /// </summary>
  [STAThread]
  private static void Main()
  {
    using var game = new Game1();
    game.Run();
  }
}
