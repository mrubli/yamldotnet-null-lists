#nullable enable

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var deserializer = new DeserializerBuilder()
    .IgnoreUnmatchedProperties()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();

{
    const string YamlWithListItems = """
    strings:
    - foo
    - bar
    """;
    var yamlWithListItems = deserializer.Deserialize<MyYaml>(new StringReader(YamlWithListItems));
    PrintList(yamlWithListItems.Strings);       // foo|bar => OK
}

{
    const string YamlWithoutList = """
    something: else
    """;
    var yamlWithoutList = deserializer.Deserialize<MyYaml>(new StringReader(YamlWithoutList));
    PrintList(yamlWithoutList.Strings);         // (empty) => OK
}

{
    const string YamlWithoutListItems = """
    strings:
    """;
    var yamlWithoutListItems = deserializer.Deserialize<MyYaml>(new StringReader(YamlWithoutListItems));
    PrintList(yamlWithoutListItems.Strings);    // (null) => BAD
}

static void PrintList(List<string>? list)
{
    Console.WriteLine(
        list switch {
            null => "(null)",
            [] => "(empty)",
            [..] => string.Join("|", list),
        }
    );
}

class MyYaml
{
    public List<string> Strings { get; set; } = [];
}
