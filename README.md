# USFMToolsSharp.Renderers.Csv
A CSV renderer for USFMToolsSharp

## Building

Standard .net build process. In Visual Studio just build

With the .net cli run `dotnet build`

## Usage

In order to use the renderer you're going to need to get a USFMDocument from USFMToolsSharp and then pass that into the renderer along with a stream to write the output to

```csharp
var document = new USFMDocument();
var renderer = new CsvRenderer();
var file = File.OpenWrite("output.csv");
renderer.Render(document, file);
```

If you have multiple documents you want to render insert them as multiple documents instead of creating one large document

```csharp
var documents = new List<USFMDocument>();
var renderer = new CsvRenderer();
var file = File.OpenWrite("output.csv");
renderer.Render(documents, file);
```