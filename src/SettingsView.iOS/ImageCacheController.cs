using System;
using Foundation;

namespace Jakar.SettingsView.iOS
{
	[Preserve(AllMembers = true)]
	public static class ImageCacheController
	{
		public static NSCache Instance
		{
			get
			{
				if ( _CacheInstance is not null ) return _CacheInstance;
				_CacheInstance = new NSCache
								 {
									 CountLimit = CacheCountLimit
								 };
				Shared.sv.SettingsView._clearCache = Clear;

				return _CacheInstance;
			}
		}

		public static void Clear()
		{
			_CacheInstance?.RemoveAllObjects();
			_CacheInstance?.Dispose();
			_CacheInstance = null;
			Shared.sv.SettingsView._clearCache = null;
		}

		private static readonly nuint CacheCountLimit = 30;
		private static NSCache _CacheInstance;
	}
}