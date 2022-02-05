using Microsoft.Build.Framework;

namespace MonoGame.Content.MSBuilder;

public class MyTaskFactory : ITaskFactory
{
  public bool Initialize(string taskName, IDictionary<string, TaskPropertyInfo> parameterGroup, string taskBody, IBuildEngine taskFactoryLoggingHost)
  {
    throw new NotImplementedException();
  }

  public TaskPropertyInfo[] GetTaskParameters()
  {
    throw new NotImplementedException();
  }

  public ITask CreateTask(IBuildEngine taskFactoryLoggingHost)
  {
    throw new NotImplementedException();
  }

  public void CleanupTask(ITask task)
  {
    throw new NotImplementedException();
  }

  public string FactoryName { get; }
  public Type TaskType { get; }
}
