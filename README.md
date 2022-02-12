# MonoGame.Content.MSBuilder

!['main' Build status](../../actions/workflows/build.yml/badge.svg?branch=main)


Replace `.mgcb` files with MSBuild properties in your MonoGame project.

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

## How to use
TBD

## Motivation
TBD

## MGCB parameters mapping
TBD

## What supported
TBD
