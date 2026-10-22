using Fili.ReadyTemplate.Services;

namespace Fili.ReadyTemplate.UnitTests;

public class TemplateCatalogTests
{
    private readonly TemplateCatalog _catalog = new();

    [Fact]
    public void Every_catalog_entry_is_a_file_in_wwwroot_templates()
    {
        var missing = _catalog.All
            .Where(t => !File.Exists(Path.Combine(RepoRoot.WwwRoot, "templates", t.File)))
            .Select(t => t.File)
            .ToList();

        Assert.Empty(missing);
    }

    [Fact]
    public void Every_template_file_in_wwwroot_is_in_the_catalog()
    {
        // browse.html is the static browse page for visitors who open the raw files; the shell
        // has its own Browse component, so it is the one template file the catalog skips.
        var onDisk = Directory.GetFiles(Path.Combine(RepoRoot.WwwRoot, "templates"), "*.html")
            .Select(Path.GetFileName)
            .Where(f => f != "browse.html");

        var unlisted = onDisk.Except(_catalog.All.Select(t => t.File)).ToList();

        Assert.Empty(unlisted);
    }

    [Fact]
    public void Files_and_keys_are_unique()
    {
        Assert.Equal(_catalog.All.Count, _catalog.All.Select(t => t.File).Distinct().Count());
        Assert.Equal(_catalog.All.Count, _catalog.All.Select(t => t.Key).Distinct().Count());
    }

    [Fact]
    public void Every_template_appears_in_the_by_activity_grouping()
    {
        var grouped = _catalog.ByActivity.SelectMany(g => g.Children).SelectMany(Flatten).Select(t => t.File).ToHashSet();
        var absent = _catalog.All.Where(t => !grouped.Contains(t.File)).Select(t => t.File).ToList();

        // The all-in-one bundle and the license are not activities; everything else must be reachable.
        Assert.Equal(["all-in-one.html", "readyset-license.html"], absent.Order());
    }

    [Fact]
    public void Sequence_steps_are_numbered_in_order_and_end_with_every_week()
    {
        var numbers = _catalog.BySequence.Select(s => s.Number).ToList();

        Assert.Equal([1, 2, 3, 4, 5, 6, 7, null], numbers);
    }

    [Fact]
    public void Hrefs_point_into_the_templates_folder()
    {
        Assert.All(_catalog.All, t => Assert.Equal($"templates/{t.File}", t.Href));
    }

    private static IEnumerable<TemplateInfo> Flatten(TemplateNode node)
    {
        yield return node.Template;
        foreach (var child in node.Children.SelectMany(Flatten))
        {
            yield return child;
        }
    }
}
