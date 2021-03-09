using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Jakar.SettingsView.iOS
{
	public static class FontUtility
	{
		private static readonly Dictionary<ToNativeFontFontKey, UIFont> ToUiFont = new Dictionary<ToNativeFontFontKey, UIFont>();
		private static readonly string _defaultFontName = UIFont.SystemFontOfSize(12).Name;

		public static UIFont CreateNativeFont( string fontFamily, float fontSize, FontAttributes fontAttributes = FontAttributes.None ) => ToNativeFont(fontFamily, fontSize, fontAttributes, _ToNativeFont);

		private static UIFont ToNativeFont( string family,
											float size,
											FontAttributes attributes,
											Func<string, float, FontAttributes, UIFont> factory )
		{
			var key = new ToNativeFontFontKey(family, size, attributes);

			lock ( ToUiFont )
			{
				UIFont value;
				if ( ToUiFont.TryGetValue(key, out value) )
					return value;
			}

			UIFont generatedValue = factory(family, size, attributes);

			lock ( ToUiFont )
			{
				UIFont value;
				if ( !ToUiFont.TryGetValue(key, out value) )
					ToUiFont.Add(key, value = generatedValue);
				return value;
			}
		}

		private static UIFont _ToNativeFont( string family, float size, FontAttributes attributes )
		{
			bool bold = ( attributes & FontAttributes.Bold ) != 0;
			bool italic = ( attributes & FontAttributes.Italic ) != 0;

			if ( family != null &&
				 family != _defaultFontName )
			{
				try
				{
					UIFont result = null;
					if ( UIFont.FamilyNames.Contains(family) )
					{
						UIFontDescriptor descriptor = new UIFontDescriptor().CreateWithFamily(family);

						if ( bold || italic )
						{
							var traits = (UIFontDescriptorSymbolicTraits) 0;
							if ( bold )
								traits = traits | UIFontDescriptorSymbolicTraits.Bold;
							if ( italic )
								traits = traits | UIFontDescriptorSymbolicTraits.Italic;

							descriptor = descriptor.CreateWithTraits(traits);
							result = UIFont.FromDescriptor(descriptor, size);
							if ( result != null )
								return result;
						}
					}

					string cleansedFont = CleanseFontName(family);
					result = UIFont.FromName(cleansedFont, size);
					if ( family.StartsWith(".SFUI", StringComparison.InvariantCultureIgnoreCase) )
					{
						string fontWeight = family.Split('-').LastOrDefault();

						if ( !string.IsNullOrWhiteSpace(fontWeight) &&
							 Enum.TryParse<UIFontWeight>(fontWeight, true, out UIFontWeight uIFontWeight) )
						{
							result = UIFont.SystemFontOfSize(size, uIFontWeight);
							return result;
						}

						result = UIFont.SystemFontOfSize(size, UIFontWeight.Regular);
						return result;
					}

					if ( result == null )
						result = UIFont.FromName(family, size);
					if ( result != null )
						return result;
				}
				catch { Debug.WriteLine("Could not load font named: {0}", family); }
			}

			if ( bold && italic )
			{
				UIFont defaultFont = UIFont.SystemFontOfSize(size);

				UIFontDescriptor descriptor = defaultFont.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold | UIFontDescriptorSymbolicTraits.Italic);
				return UIFont.FromDescriptor(descriptor, 0);
			}

			if ( italic )
				return UIFont.ItalicSystemFontOfSize(size);

			if ( bold )
				return UIFont.BoldSystemFontOfSize(size);

			return UIFont.SystemFontOfSize(size);
		}

		private static string CleanseFontName( string fontName )
		{
			//First check Alias
			( bool hasFontAlias, string fontPostScriptName ) = FontRegistrar.HasFont(fontName);
			if ( hasFontAlias )
				return fontPostScriptName;

			FontFile fontFile = FontFile.FromString(fontName);

			if ( !string.IsNullOrWhiteSpace(fontFile.Extension) )
			{
				( bool hasFont, string filePath ) = FontRegistrar.HasFont(fontFile.FileNameWithExtension());
				if ( hasFont )
					return filePath ?? fontFile.PostScriptName;
			}
			else
			{
				foreach ( string ext in FontFile.Extensions )
				{
					string formated = fontFile.FileNameWithExtension(ext);
					( bool hasFont, string filePath ) = FontRegistrar.HasFont(formated);
					if ( hasFont )
						return filePath;
				}
			}

			return fontFile.PostScriptName;
		}

		private struct ToNativeFontFontKey
		{
			internal ToNativeFontFontKey( string family, float size, FontAttributes attributes )
			{
				_family = family;
				_size = size;
				_attributes = attributes;
			}
#pragma warning disable 0414 // these are not called explicitly, but they are used to establish uniqueness. allow it!
			private string _family;
			private float _size;
			private FontAttributes _attributes;
#pragma warning restore 0414
		}
	}
}