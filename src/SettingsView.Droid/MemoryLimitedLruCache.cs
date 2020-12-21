using System;
using Android.Graphics;
using Android.Util;

namespace Jakar.SettingsView.Droid
{
	/// <summary>
	/// Memory limited lru cache.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public class MemoryLimitedLruCache : LruCache
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Droid.MemoryLimitedLruCache"/> class.
		/// </summary>
		/// <param name="size">Size.</param>
		public MemoryLimitedLruCache( int size ) : base(size) { }

		/// <summary>
		/// Sizes the of.
		/// </summary>
		/// <returns>The of.</returns>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		protected override int SizeOf( Java.Lang.Object key, Java.Lang.Object value ) => ( ( value as Bitmap )?.ByteCount ?? throw new NullReferenceException(nameof(value)) ) / 1024;
	}
}