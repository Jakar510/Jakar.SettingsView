namespace Jakar.SettingsView.Droid;

/// <summary>
/// SettingsViewInit.
/// </summary>
[Preserve(AllMembers = true)]
public static class SettingsViewInit
{
    private static FormsAppCompatActivity? _current;

    internal static FormsAppCompatActivity Current => _current ?? throw new InvalidOperationException($"Must call {nameof(SettingsViewInit)}.{nameof(Init)}");


    /// <summary>
    /// Init this instance.
    /// </summary>
    public static void Init( FormsAppCompatActivity activity ) { _current = activity ?? throw new NullReferenceException(nameof(activity)); }
}