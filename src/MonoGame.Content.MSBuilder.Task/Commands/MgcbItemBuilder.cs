using System.Collections.Generic;

namespace MonoGame.Content.MSBuilder.Task.Commands
{
  public class MgcbItemBuilder
  {
    private readonly LinkedList<string> _itemArguments = new LinkedList<string>();
    private readonly MgcbCommandBuilder _parentBuilder;
    private readonly LinkedList<string> _parentArguments;

    private readonly string _sourcePath;
    private readonly string? _destinationPath;

    public MgcbItemBuilder(MgcbCommandBuilder parentBuilder, LinkedList<string> parentArguments, string sourcePath, string? destinationPath = null)
    {
      _parentBuilder = parentBuilder;
      _parentArguments = parentArguments;
      _sourcePath = sourcePath;
      _destinationPath = destinationPath;
    }

    public MgcbItemBuilder Importer(string importer)
    {
      _itemArguments.AddLast($"/importer:{importer}");

      return this;
    }

    public MgcbItemBuilder Processor(string processor)
    {
      _itemArguments.AddLast($"/processor:{processor}");

      return this;
    }

    public MgcbItemBuilder ProcessorParam(string key, string value)
    {
      _itemArguments.AddLast($"/processorParam:{key}={value}");

      return this;
    }

    public MgcbCommandBuilder EndItem()
    {
      var buildArgument = _destinationPath is null
        ? $"/build:{_sourcePath}"
        : $"/build:{_sourcePath};{_destinationPath}";

      _itemArguments.AddLast(buildArgument);

      foreach (var itemArgument in _itemArguments)
      {
        _parentArguments.AddLast(itemArgument);
      }

      return _parentBuilder;
    }
  }
}
