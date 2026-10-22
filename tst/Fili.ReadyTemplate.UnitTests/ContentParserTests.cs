using Fili.ReadyTemplate.Services;

namespace Fili.ReadyTemplate.UnitTests;

public class ContentParserTests
{
    private const string Page = """
        <!DOCTYPE html>
        <html lang="en">
        <head>
         <title>Project Proposal</title>
        </head>
        <body>
        <nav class="sitenav" aria-label="Site"><a class="brand" href="../index.html">Fili.ReadyTemplate</a><a href="../templates/browse.html">Browse</a></nav>
        <h2>Project Proposal</h2>
        <div class="app"><p>Body text &amp; more.</p></div>
        </body>
        </html>
        """;

    [Fact]
    public void Extracts_the_title_and_the_body_without_the_page_navigation()
    {
        var page = ContentParser.Parse(Page, "en", fellBackToDefault: false);

        Assert.Equal("Project Proposal", page.Title);
        Assert.StartsWith("<h2>Project Proposal</h2>", page.BodyHtml);
        Assert.DoesNotContain("sitenav", page.BodyHtml);
        Assert.Contains("Body text &amp; more.", page.BodyHtml);
        Assert.EndsWith("</div>", page.BodyHtml);
    }

    [Fact]
    public void A_fragment_without_html_or_body_is_returned_whole_and_untitled()
    {
        var fragment = " <h3>Mission</h3>\n <dl><dt>Q</dt><dd>A</dd></dl>\n";

        var page = ContentParser.Parse(fragment, "pt", fellBackToDefault: true);

        Assert.Null(page.Title);
        Assert.Equal("<h3>Mission</h3>\n <dl><dt>Q</dt><dd>A</dd></dl>", page.BodyHtml);
        Assert.Equal("pt", page.RequestedCulture);
        Assert.True(page.FellBackToDefault);
    }

    [Fact]
    public void Title_entities_are_decoded()
    {
        var page = ContentParser.Parse("<html><head><title>Q &amp; A</title></head><body>x</body></html>", "en", false);

        Assert.Equal("Q & A", page.Title);
    }

    [Fact]
    public void The_host_page_is_recognised_as_a_missing_file()
    {
        var hostPage = File.ReadAllText(Path.Combine(RepoRoot.WwwRoot, "index.html"));

        Assert.True(ContentParser.IsSpaFallback(hostPage));
        Assert.False(ContentParser.IsSpaFallback(Page));
    }

    [Fact]
    public void No_shipped_content_file_looks_like_the_host_page()
    {
        // If a template ever contained id="app" the loader would treat it as a 404.
        var offenders = Directory.EnumerateFiles(RepoRoot.WwwRoot, "*.html", SearchOption.AllDirectories)
            .Where(f => Path.GetFileName(f) != "index.html" || !Path.GetDirectoryName(f)!.Equals(RepoRoot.WwwRoot, StringComparison.OrdinalIgnoreCase))
            .Where(f => ContentParser.IsSpaFallback(File.ReadAllText(f)))
            .Select(f => Path.GetRelativePath(RepoRoot.WwwRoot, f))
            .ToList();

        Assert.Empty(offenders);
    }

    [Theory]
    [InlineData("plan.html", "templates/plan.html")]
    [InlineData("test-cases.html#student-delete-1", "templates/test-cases.html#student-delete-1")]
    [InlineData("../docs/faq.html", "docs/faq.html")]
    [InlineData("../index.html", "index.html")]
    [InlineData("#risks", "templates/proposal.html#risks")]
    [InlineData("/docs/faq.html", "docs/faq.html")]
    [InlineData("LINK-TO-DIAGRAM", "templates/LINK-TO-DIAGRAM")]
    [InlineData("https://web.archive.org/web/2006/http://readyset.tigris.org/", "https://web.archive.org/web/2006/http://readyset.tigris.org/")]
    [InlineData("mailto:support@COMPANY.com", "mailto:support@COMPANY.com")]
    [InlineData("", "")]
    public void Links_inside_a_template_are_rebased_to_the_site_root(string original, string expected)
    {
        var body = ContentParser.RebaseLinks($"<a href=\"{original}\">x</a>", "templates/proposal.html");

        Assert.Equal($"<a href=\"{expected}\">x</a>", body);
    }

    [Theory]
    [InlineData("../release-notes-0-1-0.html", "docs/release-notes-0-1-0.html")]
    [InlineData("release-notes-0-9-2.html", "docs/readyset/release-notes-0-9-2.html")]
    [InlineData("../recommended-tools.html", "docs/recommended-tools.html")]
    [InlineData("../../templates/readyset-license.html", "templates/readyset-license.html")]
    public void Links_inside_a_nested_docs_page_are_rebased(string original, string expected)
    {
        var body = ContentParser.RebaseLinks($"<a href=\"{original}\">x</a>", "docs/readyset/release-notes-0-9-3.html");

        Assert.Equal($"<a href=\"{expected}\">x</a>", body);
    }

    [Fact]
    public void A_root_fragment_keeps_root_relative_links_and_rebases_fragments_to_the_root()
    {
        var body = ContentParser.RebaseLinks("<a href=\"docs/faq.html#chipaway\">a</a> <a href=\"#top\">b</a>", "");

        Assert.Equal("<a href=\"docs/faq.html#chipaway\">a</a> <a href=\"#top\">b</a>", body);
    }

    [Fact]
    public void Src_attributes_are_rebased_too_and_other_attributes_are_untouched()
    {
        var body = ContentParser.RebaseLinks("<img src=\"pic.png\" alt=\"x.html\" title=\"a\"><a class=\"def\" href=\"glossary.html#api\">api</a>", "templates/srs.html");

        Assert.Equal("<img src=\"templates/pic.png\" alt=\"x.html\" title=\"a\"><a class=\"def\" href=\"templates/glossary.html#api\">api</a>", body);
    }

    [Fact]
    public void Parse_rebases_links_against_the_content_path()
    {
        var page = ContentParser.Parse(Page.Replace("Body text", "<a href=\"plan.html\">plan</a>"), "en", false, "templates/proposal.html");

        Assert.Contains("href=\"templates/plan.html\"", page.BodyHtml);
    }

    [Fact]
    public void Every_shipped_template_parses_to_a_titled_body_without_its_navigation_bar()
    {
        var templates = Directory.GetFiles(Path.Combine(RepoRoot.WwwRoot, "templates"), "*.html");

        Assert.All(templates, file =>
        {
            var page = ContentParser.Parse(File.ReadAllText(file), "en", false);
            Assert.False(string.IsNullOrWhiteSpace(page.Title), file);
            Assert.DoesNotContain("class=\"sitenav\"", page.BodyHtml);
            Assert.False(string.IsNullOrWhiteSpace(page.BodyHtml), file);
        });
    }
}
