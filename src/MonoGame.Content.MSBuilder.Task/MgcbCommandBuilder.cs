using System.Text;

namespace MonoGame.Content.MSBuilder.Task
{
  public class MgcbCommandBuilder
  {
    private readonly StringBuilder _stringBuilder = new StringBuilder();
    private readonly string _mgcbCommandBase;

    public MgcbCommandBuilder(string mgcbCommandBase)
    {
      _mgcbCommandBase = mgcbCommandBase;
    }

    public MgcbCommandBuilder WorkingDirectory(string workingDir)
    {
      _stringBuilder.Append($" /workingDir:{workingDir}");

      return this;
    }

    public MgcbCommandBuilder OutputDirectory(string outputDir)
    {
      _stringBuilder.Append($" /outputDir:{outputDir}");

      return this;
    }

    public MgcbCommandBuilder IntermediateDirectory(string intermediateDir)
    {
      _stringBuilder.Append($" /intermediateDir:{intermediateDir}");

      return this;
    }

    public MgcbCommandBuilder UseRebuild()
    {
      _stringBuilder.Append(" /rebuild");

      return this;
    }

    public MgcbCommandBuilder UseClean()
    {
      _stringBuilder.Append(" /clean");

      return this;
    }

    public MgcbCommandBuilder UseIncrementalBuild()
    {
      _stringBuilder.Append(" /incremental");

      return this;
    }

    public MgcbCommandBuilder AssemblyReference(string assemblyReference)
    {
      _stringBuilder.Append($" /reference:{assemblyReference}");

      return this;
    }

    public MgcbCommandBuilder TargetPlatform(string targetPlatform)
    {
      _stringBuilder.Append($" /platform:{targetPlatform}");

      return this;
    }

    public MgcbCommandBuilder TargetGraphicsProfile(string targetGraphicsProfile)
    {
      _stringBuilder.Append($" /profile:{targetGraphicsProfile}");

      return this;
    }

    public MgcbCommandBuilder TargetBuildConfiguration(string targetBuildConfiguration)
    {
      _stringBuilder.Append($" /config:{targetBuildConfiguration}");

      return this;
    }

    public MgcbCommandBuilder UseContentCompression()
    {
      _stringBuilder.Append(" /compress");

      return this;
    }

    public MgcbItemBuilder BeginItem(string sourcePath, string? destinationPath = null)
    {
      return new MgcbItemBuilder(this, _stringBuilder, sourcePath, destinationPath);
    }

    public MgcbCommand Complete()
    {
      return new MgcbCommand(_mgcbCommandBase, _stringBuilder.ToString());
    }
  }
}
