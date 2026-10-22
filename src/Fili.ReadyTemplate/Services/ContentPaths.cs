namespace Fili.ReadyTemplate.Services;

/// <summary>
/// Where a piece of static content lives for a given culture. English files sit at their
/// original paths; a translation sits in a culture-named folder next to the original, so
/// templates/pt/proposal.html translates templates/proposal.html and docs/pt/faq.html translates
/// docs/faq.html. A translated template therefore reaches the shared stylesheets with ../css/ and
/// still works as a standalone file when the templates folder is copied into a project.
/// </summary>
public static class ContentPaths
{
    /// <summary>Turns the base-relative part of a URL into a content path: no query, no fragment.</summary>
    public static string FromRelativeUri(string relativeUri)
    {
        var end = relativeUri.IndexOfAny(['?', '#']);
        var path = end < 0 ? relativeUri : relativeUri[..end];
        return Uri.UnescapeDataString(path.TrimStart('/'));
    }

    /// <summary>The paths to try, most specific first: the culture's copy, then the original.</summary>
    public static IReadOnlyList<string> Candidates(string path, string culture)
    {
        if (culture == CultureService.Default)
        {
            return [path];
        }

        var slash = path.LastIndexOf('/');
        var translated = slash < 0
            ? $"{culture}/{path}"
            : $"{path[..(slash + 1)]}{culture}/{path[(slash + 1)..]}";
        return [translated, path];
    }
}
