
namespace Jakar.SettingsView.Droid;

/// <summary>
/// Image cache controller.
/// </summary>
[Preserve(AllMembers = true)]
public static class ImageCacheController
{
    private static readonly int CacheSize = (int) ( ( Java.Lang.Runtime.GetRuntime()?.MaxMemory() ?? throw new NullReferenceException(nameof(Java.Lang.Runtime.GetRuntime)) ) / 1024 / 8 );

    private static MemoryLimitedLruCache? _CacheInstance { get; set; }

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static MemoryLimitedLruCache Instance
    {
        get
        {
            if ( _CacheInstance != null ) return _CacheInstance;
            _CacheInstance                     = new MemoryLimitedLruCache(CacheSize);
            Shared.sv.SettingsView._clearCache = Clear;

            return _CacheInstance;
        }
    }

    /// <summary>
    /// Clear this instance.
    /// </summary>
    public static void Clear()
    {
        _CacheInstance?.EvictAll();
        _CacheInstance?.Dispose();
        _CacheInstance                     = null;
        Shared.sv.SettingsView._clearCache = null;
    }
}