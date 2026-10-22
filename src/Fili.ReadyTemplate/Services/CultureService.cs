using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fili.ReadyTemplate.Services;

/// <summary>
/// The site's current UI culture. The choice is made before .NET starts, in wwwroot/js/app.js,
/// which reads the stored preference (or the browser language), passes it to Blazor.start so the
/// runtime loads the matching satellite assembly, and sets the document's lang attribute. This
/// service exposes that choice and switches it: a switch stores the new code and reloads the page,
/// because satellite assemblies and ICU data are only loaded at startup.
/// </summary>
public sealed class CultureService(IJSRuntime js, NavigationManager navigation)
{
    public const string Default = "en";

    /// <summary>Neutral culture codes the site ships resources for, in menu order.</summary>
    public static readonly IReadOnlyList<string> Supported = ["en", "pt"];

    public string Current { get; private set; } = Default;

    /// <summary>Reads what the runtime started with and clamps it to a supported code.</summary>
    public void NormaliseStartupCulture()
    {
        Current = Normalise(CultureInfo.CurrentUICulture.Name);
        if (CultureInfo.CurrentUICulture.Name != Current)
        {
            var culture = new CultureInfo(Current);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
    }

    /// <summary>Maps any culture name onto a supported neutral code, falling back to English.</summary>
    public static string Normalise(string? cultureName)
    {
        if (string.IsNullOrWhiteSpace(cultureName))
        {
            return Default;
        }

        var neutral = cultureName.Split('-')[0].ToLowerInvariant();
        return Supported.Contains(neutral) ? neutral : Default;
    }

    public async Task SetCultureAsync(string code)
    {
        code = Normalise(code);
        if (code == Current)
        {
            return;
        }

        await js.InvokeVoidAsync("filiReadyTemplate.setCulture", code);
        navigation.NavigateTo(navigation.Uri, forceLoad: true);
    }
}
