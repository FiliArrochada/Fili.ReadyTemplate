using System.Xml.Linq;
using Fili.ReadyTemplate.Services;

namespace Fili.ReadyTemplate.UnitTests;

public class ResourceTests
{
    private static Dictionary<string, string> Load(string fileName)
        => XDocument.Load(Path.Combine(RepoRoot.Resources, fileName))
            .Root!.Elements("data")
            .ToDictionary(d => (string)d.Attribute("name")!, d => (string)d.Element("value")!);

    private static readonly Dictionary<string, string> English = Load("Strings.resx");

    public static TheoryData<string> TranslatedCultures()
    {
        var data = new TheoryData<string>();
        foreach (var code in CultureService.Supported.Where(c => c != CultureService.Default))
        {
            data.Add(code);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(TranslatedCultures))]
    public void Each_translation_has_exactly_the_english_keys(string culture)
    {
        var translated = Load($"Strings.{culture}.resx");

        Assert.Empty(English.Keys.Except(translated.Keys));
        Assert.Empty(translated.Keys.Except(English.Keys));
    }

    [Theory]
    [MemberData(nameof(TranslatedCultures))]
    public void No_translated_value_is_empty(string culture)
    {
        var empty = Load($"Strings.{culture}.resx").Where(kv => string.IsNullOrWhiteSpace(kv.Value)).Select(kv => kv.Key).ToList();

        Assert.Empty(empty);
    }

    [Fact]
    public void The_catalog_keys_all_exist_in_the_string_table()
    {
        var missing = new TemplateCatalog().ResourceKeys().Where(k => !English.ContainsKey(k)).ToList();

        Assert.Empty(missing);
    }

    [Fact]
    public void Every_supported_culture_has_a_display_name()
    {
        Assert.All(CultureService.Supported, code => Assert.True(English.ContainsKey("Lang_" + code), code));
    }

    [Theory]
    [MemberData(nameof(TranslatedCultures))]
    public void Format_placeholders_match_between_english_and_translation(string culture)
    {
        var translated = Load($"Strings.{culture}.resx");

        foreach (var (key, value) in English)
        {
            var expected = Placeholders(value);
            var actual = Placeholders(translated[key]);
            Assert.True(expected.SetEquals(actual), $"{key}: {string.Join(",", expected)} vs {string.Join(",", actual)}");
        }
    }

    private static HashSet<string> Placeholders(string s)
        => System.Text.RegularExpressions.Regex.Matches(s, @"\{\d+\}").Select(m => m.Value).ToHashSet();
}
