using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Framework.Content.Pipeline.Builder;

namespace MonoGame.Content.MSBuilder.Task
{
  // ReSharper disable once UnusedType.Global
  public class MgcbMSBuilderTask : Microsoft.Build.Utilities.Task
  {
    public string RootFolder { get; set; }

    public string ObjFolder { get; set; }

    public string OutFolder { get; set; }

    public ITaskItem[] BuildItems { get; set; }

    public ITaskItem[] Assemblies { get; set; }

    /// <inheritdoc />
    public override bool Execute()
    {
      var manager = new PipelineManager(RootFolder, OutFolder, ObjFolder);

      if (Assemblies != null)
      {
        foreach (var assembly in Assemblies)
        {
          manager.AddAssembly(assembly.ItemSpec);
        }
      }

      if (BuildItems != null)
      {
        var contentItems = BuildItems.Select(item =>
        {
          var sourceFile = item.ItemSpec;
          var destinationFile = Path.Combine(OutFolder, Path.GetRelativePath(RootFolder, sourceFile));
          var importer = item.GetMetadata("Importer");
          var processor = item.GetMetadata("Processor");
          var processorParams = new OpaqueDataDictionary();
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
          manager.RegisterContent(item.SourceFile, item.DestinationFile, item.Importer, item.Processor,
            item.ProcessorParams);
        }

        foreach (var item in contentItems)
        {
          try
          {
            manager.BuildContent(
              item.SourceFile,
              item.DestinationFile,
              item.Importer,
              item.Processor,
              item.ProcessorParams);

            BuildEngine.LogMessageEvent(new BuildMessageEventArgs(item.ToString(), string.Empty,
              nameof(MgcbMSBuilderTask), MessageImportance.High));
          }
          catch (InvalidContentException ex)
          {
            var message = string.Empty;
            if (ex.ContentIdentity != null && !string.IsNullOrEmpty(ex.ContentIdentity.SourceFilename))
            {
              message = ex.ContentIdentity.SourceFilename;
              if (!string.IsNullOrEmpty(ex.ContentIdentity.FragmentIdentifier))
              {
                message += "(" + ex.ContentIdentity.FragmentIdentifier + ")";
              }

              message += ": ";
            }

            message += ex.Message;
            // Console.WriteLine(message);
            // ++errorCount;

            throw;
          }
          catch (PipelineException ex)
          {
            // Console.Error.WriteLine("{0}: error: {1}", c.SourceFile, ex.Message);
            // if (ex.InnerException != null)
            //   Console.Error.WriteLine(ex.InnerException.ToString());
            // ++errorCount;

            throw;
          }
          catch (Exception ex)
          {
            // Console.Error.WriteLine("{0}: error: {1}", c.SourceFile, ex.Message);
            // if (ex.InnerException != null)
            //   Console.Error.WriteLine(ex.InnerException.ToString());
            // ++errorCount;

            throw;
          }
        }
      }

      return true;
    }

    private class ContentItem
    {
      public string SourceFile { get; }

      public string DestinationFile { get; }

      public string Importer { get; }

      public string Processor { get; }

      public OpaqueDataDictionary ProcessorParams { get; }

      public ContentItem(
        string sourceFile,
        string destinationFile,
        string importer,
        string processor,
        OpaqueDataDictionary processorParams)
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
