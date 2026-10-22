using Fili.ReadyTemplate.Services;

namespace Fili.ReadyTemplate.UnitTests;

public class ContentPathsTests
{
    [Theory]
    [InlineData("templates/proposal.html", "templates/proposal.html")]
    [InlineData("templates/proposal.html?x=1", "templates/proposal.html")]
    [InlineData("templates/proposal.html#goal", "templates/proposal.html")]
    [InlineData("/docs/readyset/release-notes-0-9-3.html#whatsnew", "docs/readyset/release-notes-0-9-3.html")]
    [InlineData("templates/glossary%20std.html", "templates/glossary std.html")]
    public void Strips_query_fragment_and_leading_slash_from_a_relative_uri(string relativeUri, string expected)
    {
        Assert.Equal(expected, ContentPaths.FromRelativeUri(relativeUri));
    }

    [Fact]
    public void English_has_a_single_candidate_the_original_path()
    {
        Assert.Equal(["templates/proposal.html"], ContentPaths.Candidates("templates/proposal.html", "en"));
    }

    [Theory]
    [InlineData("templates/proposal.html", "templates/pt/proposal.html")]
    [InlineData("docs/faq.html", "docs/pt/faq.html")]
    [InlineData("docs/readyset/release-notes-0-9-3.html", "docs/readyset/pt/release-notes-0-9-3.html")]
    [InlineData("content/overview.html", "content/pt/overview.html")]
    [InlineData("overview.html", "pt/overview.html")]
    public void Other_cultures_try_the_culture_folder_beside_the_file_then_the_original(string path, string translated)
    {
        Assert.Equal([translated, path], ContentPaths.Candidates(path, "pt"));
    }

    [Theory]
    [InlineData(null, "en")]
    [InlineData("", "en")]
    [InlineData("en", "en")]
    [InlineData("en-GB", "en")]
    [InlineData("pt", "pt")]
    [InlineData("pt-PT", "pt")]
    [InlineData("pt-BR", "pt")]
    [InlineData("fr-FR", "en")]
    [InlineData("PT", "pt")]
    public void Culture_names_normalise_to_a_supported_neutral_code(string? name, string expected)
    {
        Assert.Equal(expected, CultureService.Normalise(name));
    }

    [Fact]
    public void The_bootstrap_script_and_the_service_agree_on_the_supported_cultures()
    {
        var js = File.ReadAllText(Path.Combine(RepoRoot.WwwRoot, "js", "app.js"));
        var list = "['" + string.Join("', '", CultureService.Supported) + "']";

        Assert.Contains($"var supported = {list};", js);
    }
}
