using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using MonoGame.Content.MSBuilder.Commands;
using MonoGame.Content.MSBuilder.Commands.Execution;

namespace MonoGame.Content.MSBuilder.Task
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once UnusedType.Global

  /// <summary>
  /// Task that builds MGCB content items using data provided by MSBuild properties and items.
  /// </summary>
  [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by reflection")]
  public class MgcbMSBuilderTask : Microsoft.Build.Utilities.Task
  {
    // MGCB Tool

    /// <summary>
    /// Gets or sets a value indicating whether dotnet-mgcb should be used or not.
    /// If not, <see cref="MgcbPath"/> value is used as a path to the DLL of MGCB executable.
    /// </summary>
    public bool UseMgcbTool { get; set; }

    /// <summary>
    /// Gets or sets a path to DLL with MGCB executable.
    /// If <see cref="UseMgcbTool"/> is set to true, this property could be omitted.
    /// </summary>
    public string MgcbPath { get; set; } = null!;

    /// <summary>
    /// Gets or sets additional command line arguments that should be passed to the MGCB executable call.
    /// </summary>
    public string AdditionalArguments { get; set; } = null!;

    // Folders

    /// <summary>
    /// Gets or sets a path to the root folder for a content.
    /// By default, project folder of currently building project is used.
    /// </summary>
    public string ContentFolder { get; set; } = null!;

    /// <summary>
    /// Gets or sets a path to the root folder for a content.
    /// By default, "obj/mgcb-msbuilder/obj" is used.
    /// </summary>
    public string IntermediateFolder { get; set; } = null!;

    /// <summary>
    /// Gets or sets a path to the root folder for a content.
    /// By default, "obj/mgcb-msbuilder/out" is used.
    /// </summary>
    public string OutputFolder { get; set; } = null!;

    // Items

    /// <summary>
    /// Gets or sets a set of content items to build.
    /// </summary>
    public ITaskItem[]? BuildItems { get; set; }

    /// <summary>
    /// Gets or sets a set of assemblies that contain custom importer and processor classes.
    /// </summary>
    public ITaskItem[]? Assemblies { get; set; }

    // Build options

    /// <summary>
    /// Gets or sets a value indicating whether clean should be done on MGCB side.
    /// </summary>
    public bool DoClean { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether rebuild should be done on MGCB side.
    /// </summary>
    public bool DoRebuild { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether incremental build should be used on MGCB side.
    /// </summary>
    public bool DoIncrementalBuild { get; set; }

    /// <summary>
    /// Gets or sets target platform for a content.
    /// </summary>
    public string TargetPlatform { get; set; } = null!;

    /// <summary>
    /// Gets or sets target build configuration for a content.
    /// </summary>
    public string TargetBuildConfiguration { get; set; } = null!;

    // Other options

    /// <summary>
    /// Gets or sets a value indicating whether compression should be used or not.
    /// </summary>
    public bool DoCompression { get; set; }

    /// <summary>
    /// Gets or sets target graphics profile for content.
    /// </summary>
    public string TargetGraphicsProfile { get; set; } = null!;

    /// <inheritdoc />
    public override bool Execute()
    {
      if (BuildItems != null)
      {
        var builder = new MgcbCommandBuilder();

        ApplyCommonOptions(builder);

        if (Assemblies != null)
        {
          ApplyAssemblies(builder, Assemblies);
        }

        ApplyContentItems(builder, BuildItems);

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

    private MgcbCommandBuilder ApplyAssemblies(MgcbCommandBuilder builder, IEnumerable<ITaskItem> assemblies)
    {
      var fullPathAssemblies = assemblies.Select(assembly => Path.GetFullPath(assembly.ItemSpec, ContentFolder));

      foreach (var assembly in fullPathAssemblies)
      {
        builder.AssemblyReference(assembly);
      }

      return builder;
    }

    private MgcbCommandBuilder ApplyContentItems(MgcbCommandBuilder builder, IEnumerable<ITaskItem> buildItems)
    {
      var contentItems = buildItems.Select(item =>
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
        return new MgcbToolCommandExecutor(ContentFolder, AdditionalArguments);
      }

      return new MgcbDllCommandExecutor(ContentFolder, MgcbPath, AdditionalArguments);
    }

    private class ContentItem
    {
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

      public string SourceFile { get; }

      public string DestinationFile { get; }

      public string Importer { get; }

      public string Processor { get; }

      public IDictionary<string, string> ProcessorParams { get; }

      public override string ToString()
      {
        return $"Source: {SourceFile}, Dest: {DestinationFile}, Importer: {Importer}, Processor: {Processor}";
      }
    }
  }
}
