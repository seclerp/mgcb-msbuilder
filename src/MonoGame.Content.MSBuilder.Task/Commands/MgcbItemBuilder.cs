using System.Collections.Generic;

namespace MonoGame.Content.MSBuilder.Task.Commands
{
  /// <summary>
  /// Class that is responsible for a content item configuration.
  /// </summary>
  public class MgcbItemBuilder
  {
    private readonly LinkedList<string> _itemArguments = new LinkedList<string>();
    private readonly MgcbCommandBuilder _parentBuilder;
    private readonly LinkedList<string> _parentArguments;

    private readonly string _sourcePath;
    private readonly string? _destinationPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="MgcbItemBuilder"/> class.
    /// </summary>
    /// <param name="parentBuilder">Instance of <see cref="MgcbCommandBuilder"/>.</param>
    /// <param name="parentArguments">List of already added main builder arguments.</param>
    /// <param name="sourcePath">Path to the content item.</param>
    /// <param name="destinationPath">Optional destination path that this content item should be built to.</param>
    public MgcbItemBuilder(MgcbCommandBuilder parentBuilder, LinkedList<string> parentArguments, string sourcePath, string? destinationPath = null)
    {
      _parentBuilder = parentBuilder;
      _parentArguments = parentArguments;
      _sourcePath = sourcePath;
      _destinationPath = destinationPath;
    }

    /// <summary>
    /// Adds "/importer" parameter to the resulting item.
    /// </summary>
    /// <param name="importer">A value that should be passed to "/importer" parameter.</param>
    /// <returns>Updated instance of <see cref="MgcbItemBuilder"/>.</returns>
    public MgcbItemBuilder Importer(string importer)
    {
      _itemArguments.AddLast($"/importer:{importer}");

      return this;
    }

    /// <summary>
    /// Adds "/processor" parameter to the resulting item.
    /// </summary>
    /// <param name="processor">A value that should be passed to "/processor" parameter.</param>
    /// <returns>Updated instance of <see cref="MgcbItemBuilder"/>.</returns>
    public MgcbItemBuilder Processor(string processor)
    {
      _itemArguments.AddLast($"/processor:{processor}");

      return this;
    }

    /// <summary>
    /// Adds "/processorParam" parameter to the resulting item.
    /// </summary>
    /// <param name="key">Name of the parameter.</param>
    /// <param name="value">Value of the parameter.</param>
    /// <returns>Updated instance of <see cref="MgcbItemBuilder"/>.</returns>
    public MgcbItemBuilder ProcessorParam(string key, string value)
    {
      _itemArguments.AddLast($"/processorParam:{key}={value}");

      return this;
    }

    /// <summary>
    /// Finishes a content item build configuration process.
    /// </summary>
    /// <returns>Updated instance of <see cref="MgcbCommandBuilder"/>.</returns>
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
