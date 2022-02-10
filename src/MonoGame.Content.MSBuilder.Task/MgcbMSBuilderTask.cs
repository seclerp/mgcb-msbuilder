using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using MonoGame.Content.MSBuilder.Task.Commands;
using MonoGame.Content.MSBuilder.Task.Commands.Execution;

namespace MonoGame.Content.MSBuilder.Task
{
  // ReSharper disable once UnusedType.Global
  public class MgcbMSBuilderTask : Microsoft.Build.Utilities.Task
  {
    // MGCB Tool

    public bool UseMgcbTool { get; set; }

    public string MgcbPath { get; set; }

    // Folders

    public string ContentFolder { get; set; }

    public string IntermediateFolder { get; set; }

    public string OutputFolder { get; set; }

    // Items

    public ITaskItem[]? BuildItems { get; set; }

    public ITaskItem[]? Assemblies { get; set; }

    // Build options

    public bool DoClean { get; set; }

    public bool DoRebuild { get; set; }

    public bool DoIncrementalBuild { get; set; }

    public string TargetPlatform { get; set; }

    public string TargetBuildConfiguration { get; set; }

    // Other options

    public bool DoCompression { get; set; }

    public string TargetGraphicsProfile { get; set; }

    /// <inheritdoc />
    public override bool Execute()
    {
      if (BuildItems != null)
      {
        var builder = new MgcbCommandBuilder(MgcbPath);

        ApplyCommonOptions(builder);
        ApplyAssemblies(builder);
        ApplyContentItems(builder);

        var command = builder.Complete();
        var commandExecutor = GetCommandExecutor();

        commandExecutor.Execute(command);
      }

      return true;
    }

    private MgcbCommandBuilder ApplyCommonOptions(MgcbCommandBuilder builder)
    {
      builder = builder
        .WorkingDirectory(ContentFolder)
        .OutputDirectory(OutputFolder)
        .IntermediateDirectory(IntermediateFolder)
        .TargetPlatform(TargetPlatform)
        .TargetGraphicsProfile(TargetGraphicsProfile)
        .TargetBuildConfiguration(TargetBuildConfiguration);

      if (DoClean)
      {
        builder = builder.UseClean();
      }

      if (DoRebuild)
      {
        builder = builder.UseRebuild();
      }

      if (DoIncrementalBuild)
      {
        builder = builder.UseIncrementalBuild();
      }

      if (DoCompression)
      {
        builder = builder.UseContentCompression();
      }

      return builder;
    }

    private MgcbCommandBuilder ApplyAssemblies(MgcbCommandBuilder builder)
    {
      if (Assemblies != null)
      {
        var assemblies = Assemblies.Select(assembly => Path.GetFullPath(assembly.ItemSpec, ContentFolder));

        foreach (var assembly in assemblies)
        {
          builder.AssemblyReference(assembly);
        }
      }

      return builder;
    }

    private MgcbCommandBuilder ApplyContentItems(MgcbCommandBuilder builder)
    {
      var contentItems = BuildItems.Select(item =>
      {
        var sourceFile = item.ItemSpec;
        var destinationFile = Path.Combine(OutputFolder, Path.GetRelativePath(ContentFolder, sourceFile));
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
      });

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

      return builder;
    }

    private MgcbCommandExecutorBase GetCommandExecutor()
    {
      if (UseMgcbTool)
      {
        return new MgcbToolCommandExecutor(ContentFolder);
      }

      return new MgcbDllCommandExecutor(ContentFolder, MgcbPath);
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
