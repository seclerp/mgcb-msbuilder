using System.Collections.Generic;
using System.Text;
using MonoGame.Content.MSBuilder.Helpers;

namespace MonoGame.Content.MSBuilder.Commands
{
  /// <summary>
  /// Class that is responsible for <see cref="MgcbCommand"/> construction.
  /// </summary>
  public class MgcbCommandBuilder
  {
    private readonly LinkedList<string> _arguments = new LinkedList<string>();

    /// <summary>
    /// Adds "/workingDir" parameter to the resulting command.
    /// </summary>
    /// <param name="workingDir">A value that should be passed to "/workingDir" parameter.</param>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
    public MgcbCommandBuilder WorkingDirectory(string workingDir)
    {
      workingDir = PathHelper.NormalizeSeparators(workingDir);
      _arguments.AddLast($"/workingDir:\"{workingDir}\"");

      return this;
    }

    /// <summary>
    /// Adds "/outputDir" parameter to the resulting command.
    /// </summary>
    /// <param name="outputDir">A value that should be passed to "/outputDir" parameter.</param>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
    public MgcbCommandBuilder OutputDirectory(string outputDir)
    {
      outputDir = PathHelper.NormalizeSeparators(outputDir);
      _arguments.AddLast($"/outputDir:\"{outputDir}\"");

      return this;
    }

    /// <summary>
    /// Adds "/intermediateDir" parameter to the resulting command.
    /// </summary>
    /// <param name="intermediateDir">A value that should be passed to "/intermediateDir" parameter.</param>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
    public MgcbCommandBuilder IntermediateDirectory(string intermediateDir)
    {
      intermediateDir = PathHelper.NormalizeSeparators(intermediateDir);
      _arguments.AddLast($"/intermediateDir:\"{intermediateDir}\"");

      return this;
    }

    /// <summary>
    /// Adds "/rebuild" switch to the resulting command.
    /// </summary>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
    public MgcbCommandBuilder UseRebuild()
    {
      _arguments.AddLast("/rebuild");

      return this;
    }

    /// <summary>
    /// Adds "/clean" switch to the resulting command.
    /// </summary>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
    public MgcbCommandBuilder UseClean()
    {
      _arguments.AddLast("/clean");

      return this;
    }

    /// <summary>
    /// Adds "/incremental" switch to the resulting command.
    /// </summary>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
    public MgcbCommandBuilder UseIncrementalBuild()
    {
      _arguments.AddLast("/incremental");

      return this;
    }

    /// <summary>
    /// Adds "/reference" parameter to the resulting command.
    /// </summary>
    /// <param name="assemblyReference">A value that should be passed to "/reference" parameter.</param>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
    public MgcbCommandBuilder AssemblyReference(string assemblyReference)
    {
      assemblyReference = PathHelper.NormalizeSeparators(assemblyReference);
      _arguments.AddLast($"/reference:\"{assemblyReference}\"");

      return this;
    }

    /// <summary>
    /// Adds "/platform" parameter to the resulting command.
    /// </summary>
    /// <param name="targetPlatform">A value that should be passed to "/platform" parameter.</param>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
    public MgcbCommandBuilder TargetPlatform(string targetPlatform)
    {
      _arguments.AddLast($"/platform:{targetPlatform}");

      return this;
    }

    /// <summary>
    /// Adds "/profile" parameter to the resulting command.
    /// </summary>
    /// <param name="targetGraphicsProfile">A value that should be passed to "/profile" parameter.</param>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
    public MgcbCommandBuilder TargetGraphicsProfile(string targetGraphicsProfile)
    {
      _arguments.AddLast($"/profile:{targetGraphicsProfile}");

      return this;
    }

    /// <summary>
    /// Adds "/config" parameter to the resulting command.
    /// </summary>
    /// <param name="targetBuildConfiguration">A value that should be passed to "/config" parameter.</param>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
    public MgcbCommandBuilder TargetBuildConfiguration(string targetBuildConfiguration)
    {
      _arguments.AddLast($"/config:{targetBuildConfiguration}");

      return this;
    }

    /// <summary>
    /// Adds "/compress" switch to the resulting command.
    /// </summary>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
    public MgcbCommandBuilder UseContentCompression()
    {
      _arguments.AddLast("/compress");

      return this;
    }

    /// <summary>
    /// Starts a content item build configuration process.
    /// </summary>
    /// <param name="sourcePath">Path tp the content item.</param>
    /// <param name="destinationPath">Optional destination path of the content item.</param>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
    public MgcbItemBuilder BeginItem(string sourcePath, string? destinationPath = null)
    {
      return new MgcbItemBuilder(this, _arguments, sourcePath, destinationPath);
    }

    /// <summary>
    /// Finish building process.
    /// </summary>
    /// <param name="command">A command to be executed.</param>
    /// <param name="additionalArguments">Optional MGCB arguments.</param>
    /// <returns>Created command instance.</returns>
    public string Complete(string command, string? additionalArguments = null)
    {
      var resultBuilder = new StringBuilder(command);

      if (!string.IsNullOrWhiteSpace(additionalArguments))
      {
        resultBuilder.Append($" {additionalArguments}");
      }

      resultBuilder.Append($" {string.Join(" ", _arguments)}");

      return resultBuilder.ToString();
    }
  }
}
