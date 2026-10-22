namespace Fili.ReadyTemplate.UnitTests;

/// <summary>Finds the repository root from the test binary's location, so tests can read source files.</summary>
internal static class RepoRoot
{
    public static string Path { get; } = Find();

    public static string WwwRoot => System.IO.Path.Combine(Path, "src", "Fili.ReadyTemplate", "wwwroot");

    public static string Resources => System.IO.Path.Combine(Path, "src", "Fili.ReadyTemplate", "Resources");

    private static string Find()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(System.IO.Path.Combine(dir.FullName, "Fili.ReadyTemplate.sln")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Fili.ReadyTemplate.sln not found above the test binary.");
    }
}
