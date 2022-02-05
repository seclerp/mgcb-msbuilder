using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace MonoGame.Content.MSBuilder
{
  public class MgcbBuilderTask : Task
  {
    public string MgcbOutputDirectory { get; set; }

    public string MgcbIntermediateDirectory { get; set; }

    public bool MgcbRebuild { get; set; }

    public bool MgcbClean { get; set; }

    public bool MgcbIncremental { get; set; }

    public string MgcbGraphicsProfile { get; set; }

    public string MgcbBuildConfiguration { get; set; }

    public string MgcbContentCompression { get; set; }

    public ITaskItem[] MgcbItem { get; set; }

    public ITaskItem[] MgcbReference { get; set; }

    public override bool Execute()
    {
      // var manager = new PipelineManager(_projectDirectory, )

      return true;
    }
  }
}
