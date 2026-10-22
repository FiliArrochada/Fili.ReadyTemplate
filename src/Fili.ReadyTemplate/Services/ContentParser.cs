using System.Text.RegularExpressions;

namespace Fili.ReadyTemplate.Services;

/// <summary>A static page or fragment, reduced to what the shell renders inside its layout.</summary>
/// <param name="Title">The page's title, or null for a fragment.</param>
/// <param name="BodyHtml">The body's inner HTML with the page's own navigation bar removed and its links rebased.</param>
/// <param name="RequestedCulture">The culture the visitor asked for.</param>
/// <param name="FellBackToDefault">True when no translation existed and the English original is shown.</param>
public sealed record ContentPage(string? Title, string BodyHtml, string RequestedCulture, bool FellBackToDefault);

public static partial class ContentParser
{
    /// <summary>
    /// The host page carries this marker and nothing else on the site does. A dev server or GitHub
    /// Pages answers a missing file with the host page, so a body containing it is "not found".
    /// </summary>
    public const string SpaMarker = "id=\"app\"";

    public static bool IsSpaFallback(string html) => html.Contains(SpaMarker, StringComparison.Ordinal);

    /// <param name="html">The fetched page or fragment.</param>
    /// <param name="requestedCulture">The culture the visitor asked for.</param>
    /// <param name="fellBackToDefault">Whether the English original is being shown instead of a translation.</param>
    /// <param name="contentPath">
    /// The base-relative path the content was written for, such as <c>templates/proposal.html</c>.
    /// Links inside it are rebased against this path's folder; an empty string means the site root.
    /// </param>
    public static ContentPage Parse(string html, string requestedCulture, bool fellBackToDefault, string contentPath = "")
    {
        var title = TitlePattern().Match(html) is { Success: true } t ? System.Net.WebUtility.HtmlDecode(t.Groups[1].Value.Trim()) : null;

        var body = BodyPattern().Match(html) is { Success: true } b ? b.Groups[1].Value : html;
        body = SiteNavPattern().Replace(body, "", 1).Trim();
        body = RebaseLinks(body, contentPath);

        return new ContentPage(title, body, requestedCulture, fellBackToDefault);
    }

    /// <summary>
    /// The shell's host page declares <c>&lt;base href="/"&gt;</c>, so the browser resolves every
    /// relative link against the site root rather than against the page that contains it. The
    /// templates were written as plain files linking each other with <c>plan.html</c> and
    /// <c>../docs/faq.html</c>, so those links are rewritten here to base-relative form:
    /// <c>templates/plan.html</c>, <c>docs/faq.html</c>. Fragment-only links become the content's
    /// own path plus the fragment, and root-relative links lose their leading slash. Links with a
    /// scheme (http, https, mailto, javascript, data) are left alone.
    /// </summary>
    public static string RebaseLinks(string body, string contentPath)
    {
        var slash = contentPath.LastIndexOf('/');
        var directory = slash < 0 ? "" : contentPath[..(slash + 1)];
        var root = new Uri("http://content.invalid/" + directory);

        return LinkPattern().Replace(body, m =>
        {
            var value = m.Groups[2].Value;
            if (value.Length == 0 || HasScheme(value))
            {
                return m.Value;
            }

            string rebased;
            if (value[0] == '#')
            {
                rebased = contentPath + value;
            }
            else if (value[0] == '/')
            {
                rebased = value.TrimStart('/');
            }
            else if (Uri.TryCreate(root, value, out var resolved))
            {
                rebased = (resolved.PathAndQuery + resolved.Fragment).TrimStart('/');
            }
            else
            {
                return m.Value;
            }

            return $"{m.Groups[1].Value}=\"{rebased}\"";
        });
    }

    private static bool HasScheme(string value)
    {
        var colon = value.IndexOf(':');
        if (colon <= 0)
        {
            return false;
        }

        var slashOrHash = value.IndexOfAny(['/', '#', '?']);
        return slashOrHash < 0 || colon < slashOrHash;
    }

    [GeneratedRegex("<title>(.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex TitlePattern();

    [GeneratedRegex("<body[^>]*>(.*)</body>", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex BodyPattern();

    [GeneratedRegex("<nav class=\"sitenav\"[^>]*>.*?</nav>\\s*", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex SiteNavPattern();

    [GeneratedRegex("\\b(href|src)=\"([^\"]*)\"", RegexOptions.IgnoreCase)]
    private static partial Regex LinkPattern();
}
