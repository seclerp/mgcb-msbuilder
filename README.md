# MonoGame.Content.MSBuilder

!['main' Build status](../../actions/workflows/build.yml/badge.svg?branch=main)


MGCB MSBuilder task allows you to replace `.mgcb` files with MSBuild properties and items in your MonoGame project.

Instead of this:

<kbd>Content.mgcb:</kbd>
```shell
/rebuild

# Build a texture
/importer:TextureImporter
/processor:TextureProcessor
/processorParam:ColorKeyEnabled=false
/build:Textures\wood.png
/build:Textures\metal.png
/build:Textures\plastic.png
```

You could use:
```xml
<PropertyGroup>
    <MgcbDoRebuild>true</MgcbDoRebuild>
</PropertyGroup>

<ItemGroup>
    <MgcbItem Include="Textures\wood.png;Textures\metal.png;Textures\plastic.png">
        <Importer>TextureImporter</Importer>
        <Processor>TextureImporter</Processor>
        <Processor_ColorKeyEnabled>false</Processor_ColorKeyEnabled>
    </MgcbItem>
</ItemGroup>
```

Right inside you project file or in imported MSBuild project.

## Motivation
> This project is mostly experimental.
> 
> It could be also used as an example for learning some MSBuild-related
stuff.

There are some points that could be addressed using MGCB MSBuilder:
- **Elimination of additional non-standard file format**. `.mgcb` files could be surprising and don't introduce strict parameters structure.
These files are not properly supported inside various IDEs and editors.
- **Integration with MSBuild common build pipeline**. MonoGame allows you to use MonoGame Content Builder (MGCB) as a MSBuild task,
but it's still relying on file format described above. It also does not support using dotnet tool `dotnet-mgcb` in easy way.

## How to use

1. Install MonoGame.Content.MSBuilder.Task using NuGet:

    `dotnet install MonoGame.Content.MSBuilder.Task`
   
    (or use your favorite IDE's UI for that)
2. Create your content description file `Content.props`:
    ```xml
    <Project>
        <PropertyGroup>
            <UseMgcbTool>true</UseMgcbTool>
            <MgcbCompress>true</MgcbCompress>
        </PropertyGroup>
   
        <ItemGroup>
            <MgcbItem Include="Content/Images/Image.png">
                <Importer>TextureImporter</Importer>
                <Processor>TextureProcessor</Processor>
                <Processor_ColorKeyEnabled>false</Processor_ColorKeyEnabled>
            </MgcbItem>
        </ItemGroup>
    </Project>
    ```
3. Import your `Content.props` into your .NET project:
   ```xml
   <Import Project="Content.props" />
   ```
4. **If you use `dotnet-mgcb` tool** for building, it should be installed via
   `dotnet install dotnet-mgcb` or `dotnet install -g dotnet-mgcb`.

   Then specify `UseMgcbTool` property:

   ```xml
   <UseMgcbTool>true</UseMgcbTool>
   ```

   **If you use MonoGame.Content.Builder.Task**, no actions required.

   **If you use custom MGCB executable**, it should be presented inside MGCBPath property:

   ```xml
   <MGCBPath>/path/to/mgcb.dll></MGCBPath>
   ```

5. Now build the project - you should see your Content folder, as you usually do with `.mgcb` approach.

## Integration with existing `MonoGame.Content.Builder.Task`

MGCB MSBuilder could be used without `MonoGame.Content.Builder.Task`, but if you use both of them -
`MGCBPath` and `MonoGameMGCBAdditionalArguments` MSBuild properties would be also used by MGCB MSBuilder by default.

## MGCB parameters mapping

### Global

Each parameter should be presented inside `<PropertyGroup>`, ex:
```xml
<PropertyGroup>
   <MgcbContentFolder>path/to/content/source/folder</MgcbContentFolder>
</PropertyGroup>
```

| `.mgcb` parameter  | MSBuild alternative       | Default value             |
|--------------------|---------------------------|---------------------------|
| `/workingDir`      | `MgcbContentFolder`       | Project folder            |
| `/outputDir`       | `MgcbOutputFolder`        | `/obj/mgcb-msbuilder/out` |
| `/intermediateDir` | `MgcbIntermediateFolder`  | `/obj/mgcb-msbuilder/obj` |
| `/rebuild`         | `MgcbDoRebuild`           | `false`                   |
| `/clean`           | `MgcbDoClean`             | `false`                   |
| `/incremental`     | `MgcbDoIncremental`       | `false`                   |
| `/platform`        | `MgcbTargetPlatform`      | (required)                |
| `/profile`         | `MgcbTargetProfile`       | `HiDef`                   |
| `/config`          | `MGcbTargetConfiguration` | Same as project one       |
| `/compress`        | `MgcbDoCompress`          | `false`                   |
| `/launchdebugger`  | ⚠ Not supported yet       | N/A                       |
| `/@`               | ⚠ Not supported yet       | N/A                       |

### Content item-related

Each parameter should be presented inside `<MgcbItem>`, ex:
```xml
<ItemGroup>
   <MgcbItem Include="Content/SomeTexture.png">
      <Importer>CustomImporter</Importer>
   </MgcbItem>
</ItemGroup>
```

| `.mgcb` parameter | MSBuild alternative                                                                     |
|-------------------|-----------------------------------------------------------------------------------------|
| `/importer`       | `Importer`                                                                              |
| `/processor`      | `Processor`                                                                             |
| `/processorParam` | `Processor_ParamName`                                                                   |
| `/build`          | Use `Include` attribute with `<MgcbItem>` node. ⚠ Destination part is not supported yet |

### Custom assemblies (`/reference`)

Use `<MgcbAssembly Include="/path/to.dll">` inside `<ItemGroup>`.

## Disadvantages of using MGCB MSBuilder
1. No UI support. You should specify all options by hand.