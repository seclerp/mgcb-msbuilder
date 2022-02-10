using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Content.MSBuilder.Task.Commands
{
  public class MgcbCommandBuilder
  {
    private readonly LinkedList<string> _arguments = new LinkedList<string>();
    private readonly string _mgcbCommandBase;

    public MgcbCommandBuilder(string mgcbCommandBase)
    {
      _mgcbCommandBase = mgcbCommandBase;
    }

    public MgcbCommandBuilder WorkingDirectory(string workingDir)
    {
      _arguments.AddLast($"/workingDir:{workingDir}");

      return this;
    }

    public MgcbCommandBuilder OutputDirectory(string outputDir)
    {
      _arguments.AddLast($"/outputDir:{outputDir}");

      return this;
    }

    public MgcbCommandBuilder IntermediateDirectory(string intermediateDir)
    {
      _arguments.AddLast($"/intermediateDir:{intermediateDir}");

      return this;
    }

    public MgcbCommandBuilder UseRebuild()
    {
      _arguments.AddLast("/rebuild");

      return this;
    }

    public MgcbCommandBuilder UseClean()
    {
      _arguments.AddLast("/clean");

      return this;
    }

    public MgcbCommandBuilder UseIncrementalBuild()
    {
      _arguments.AddLast("/incremental");

      return this;
    }

    public MgcbCommandBuilder AssemblyReference(string assemblyReference)
    {
      _arguments.AddLast($"/reference:{assemblyReference}");

      return this;
    }

    public MgcbCommandBuilder TargetPlatform(string targetPlatform)
    {
      _arguments.AddLast($"/platform:{targetPlatform}");

      return this;
    }

    public MgcbCommandBuilder TargetGraphicsProfile(string targetGraphicsProfile)
    {
      _arguments.AddLast($"/profile:{targetGraphicsProfile}");

      return this;
    }

    public MgcbCommandBuilder TargetBuildConfiguration(string targetBuildConfiguration)
    {
      _arguments.AddLast($"/config:{targetBuildConfiguration}");

      return this;
    }

    public MgcbCommandBuilder UseContentCompression()
    {
      _arguments.AddLast("/compress");

      return this;
    }

    public MgcbItemBuilder BeginItem(string sourcePath, string? destinationPath = null)
    {
      return new MgcbItemBuilder(this, _arguments, sourcePath, destinationPath);
    }

    public MgcbCommand Complete()
    {
      return new MgcbCommand(_arguments.ToArray());
    }
  }
}
