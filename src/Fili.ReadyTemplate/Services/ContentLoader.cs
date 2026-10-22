namespace Fili.ReadyTemplate.Services;

/// <summary>Fetches static content for the current culture, falling back to the English original.</summary>
public sealed class ContentLoader(HttpClient http, CultureService culture)
{
    /// <param name="path">Base-relative path of the English original, such as <c>templates/proposal.html</c>.</param>
    /// <param name="linkBase">
    /// The path whose folder the content's relative links were written against. Defaults to
    /// <paramref name="path"/>; a fragment authored for the site root passes an empty string.
    /// </param>
    public async Task<ContentPage?> LoadAsync(string path, string? linkBase = null)
    {
        var candidates = ContentPaths.Candidates(path, culture.Current);
        foreach (var candidate in candidates)
        {
            var html = await TryGetAsync(candidate);
            if (html is not null)
            {
                return ContentParser.Parse(html, culture.Current, fellBackToDefault: candidate != candidates[0], contentPath: linkBase ?? path);
            }
        }

        return null;
    }

    private async Task<string?> TryGetAsync(string path)
    {
        try
        {
            using var response = await http.GetAsync(path);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var html = await response.Content.ReadAsStringAsync();
            return ContentParser.IsSpaFallback(html) ? null : html;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
