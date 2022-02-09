using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;

namespace MonoGame.Content.MSBuilder.Task
{
  // ReSharper disable once UnusedType.Global
  public class MgcbMSBuilderTask : Microsoft.Build.Utilities.Task
  {
    public string MgcbPath { get; set; }

    public string RootFolder { get; set; }

    public string ObjFolder { get; set; }

    public string OutFolder { get; set; }

    public ITaskItem[]? BuildItems { get; set; }

    public ITaskItem[]? Assemblies { get; set; }

    /// <inheritdoc />
    public override bool Execute()
    {
      if (BuildItems != null)
      {
        var builder = new MgcbCommandBuilder(MgcbPath)
          .WorkingDirectory(RootFolder)
          .OutputDirectory(OutFolder)
          .IntermediateDirectory(ObjFolder);

        if (Assemblies != null)
        {
          var assemblies = Assemblies.Select(assembly => Path.GetFullPath(assembly.ItemSpec, RootFolder));

          foreach (var assembly in assemblies)
          {
            builder.AssemblyReference(assembly);
          }
        }

        var contentItems = BuildItems.Select(item =>
        {
          var sourceFile = item.ItemSpec;
          var destinationFile = Path.Combine(OutFolder, Path.GetRelativePath(RootFolder, sourceFile));
          var importer = item.GetMetadata("Importer");
          var processor = item.GetMetadata("Processor");
          var processorParams = new Dictionary<string, string>();
          foreach (var itemMetadataName in item.MetadataNames)
          {
            var stringMetadataName = itemMetadataName.ToString();
            if (stringMetadataName.StartsWith("Processor_"))
            {
              processorParams.Add(stringMetadataName, item.GetMetadata(stringMetadataName));
            }
          }

          return new ContentItem(sourceFile, destinationFile, importer, processor, processorParams);
        }).ToList();

        foreach (var item in contentItems)
        {
          var itemBuilder = builder
            .BeginItem(item.SourceFile, item.DestinationFile)
            .Importer(item.Importer)
            .Processor(item.Processor);

          foreach (var param in item.ProcessorParams)
          {
            itemBuilder.ProcessorParam(param.Key, param.Value);
          }

          itemBuilder.EndItem();
        }

        var command = builder.Complete();
        command.Execute();
      }

      return true;
    }

    private class ContentItem
    {
      public string SourceFile { get; }

      public string DestinationFile { get; }

      public string Importer { get; }

      public string Processor { get; }

      public IDictionary<string, string> ProcessorParams { get; }

      public ContentItem(
        string sourceFile,
        string destinationFile,
        string importer,
        string processor,
        IDictionary<string, string> processorParams)
      {
        SourceFile = sourceFile;
        DestinationFile = destinationFile;
        Importer = importer;
        Processor = processor;
        ProcessorParams = processorParams;
      }

      public override string ToString()
      {
        return $"Source: {SourceFile}, Dest: {DestinationFile}, Importer: {Importer}, Processor: {Processor}";
      }
    }
  }
}
