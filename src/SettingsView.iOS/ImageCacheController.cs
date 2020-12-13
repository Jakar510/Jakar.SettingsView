using System;
using Foundation;

namespace Jakar.SettingsView.iOS
{
	/// <summary>
	/// Image cache controller.
	/// </summary>
	[Preserve(AllMembers = true)]
	public static class ImageCacheController
	{
		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static NSCache Instance
		{
			get
			{
				if ( _CacheInstance == null )
				{
					_CacheInstance = new NSCache();
					_CacheInstance.CountLimit = CacheCountLimit;
					Shared.SettingsView._clearCache = Clear;
				}

				return _CacheInstance;
			}
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public static void Clear()
		{
			_CacheInstance?.RemoveAllObjects();
			_CacheInstance?.Dispose();
			_CacheInstance = null;
			Shared.SettingsView._clearCache = null;
		}

		private static readonly nuint CacheCountLimit = 30;
		private static NSCache _CacheInstance;
	}
}