using System.Text;

namespace MonoGame.Content.MSBuilder.Task
{
  public class MgcbItemBuilder
  {
    private readonly StringBuilder _stringBuilder = new StringBuilder();

    private readonly MgcbCommandBuilder _parentBuilder;
    private readonly StringBuilder _parentStringBuilder;
    private readonly string _sourcePath;
    private readonly string? _destinationPath;

    public MgcbItemBuilder(MgcbCommandBuilder parentBuilder, StringBuilder parentStringStringBuilder, string sourcePath, string? destinationPath = null)
    {
      _parentBuilder = parentBuilder;
      _parentStringBuilder = parentStringStringBuilder;
      _sourcePath = sourcePath;
      _destinationPath = destinationPath;
    }

    public MgcbItemBuilder Importer(string importer)
    {
      _stringBuilder.Append($" /importer:{importer}");

      return this;
    }

    public MgcbItemBuilder Processor(string processor)
    {
      _stringBuilder.Append($" /processor:{processor}");

      return this;
    }

    public MgcbItemBuilder ProcessorParam(string key, string value)
    {
      _stringBuilder.Append($" /processorParam:{key}={value}");

      return this;
    }

    public MgcbCommandBuilder EndItem()
    {
      _stringBuilder.Append($" /build:{_sourcePath}");

      if (!string.IsNullOrWhiteSpace(_destinationPath))
      {
        _stringBuilder.Append($";{_destinationPath}");
      }

      _parentStringBuilder.Append(_stringBuilder);

      return _parentBuilder;
    }
  }
}
