namespace Jakar.SettingsView.iOS
{
	public static class FontUtility
	{
		private static readonly Dictionary<ToNativeFontFontKey, UIFont> ToUiFont = new();
		private static readonly string _defaultFontName = UIFont.SystemFontOfSize(12).Name;

		public static UIFont CreateNativeFont( string? family, float size, FontAttributes attributes = FontAttributes.None )
		{
			var key = new ToNativeFontFontKey(family, size, attributes);

			lock ( ToUiFont )
			{
				if ( ToUiFont.TryGetValue(key, out UIFont value) )
					return value;
			}

			UIFont generatedValue = ToNativeFont(family, size, attributes);

			lock ( ToUiFont )
			{
				if ( !ToUiFont.TryGetValue(key, out UIFont value) ) { ToUiFont.Add(key, value = generatedValue); }

				return value;
			}
		}

		private static UIFont ToNativeFont( string? family, float size, FontAttributes attributes )
		{
			bool bold = ( attributes & FontAttributes.Bold ) != 0;
			bool italic = ( attributes & FontAttributes.Italic ) != 0;

			if ( family is not null )
			{
				if ( family != _defaultFontName )
				{
					try
					{
						UIFont result;
						if ( UIFont.FamilyNames.Contains(family) )
						{
							using UIFontDescriptor descriptor = new UIFontDescriptor().CreateWithFamily(family);

							if ( bold || italic )
							{
								var traits = (UIFontDescriptorSymbolicTraits) 0;

								if ( bold )
									traits |= UIFontDescriptorSymbolicTraits.Bold;

								if ( italic )
									traits |= UIFontDescriptorSymbolicTraits.Italic;

								result = UIFont.FromDescriptor(descriptor.CreateWithTraits(traits), size);
								if ( result is not null )
									return result;
							}
						}

						string cleansedFont = CleanseFontName(family);
						result = UIFont.FromName(cleansedFont, size);
						if ( family.StartsWith(".SFUI", StringComparison.InvariantCultureIgnoreCase) )
						{
							string? fontWeight = family.Split('-').LastOrDefault();

							if ( !string.IsNullOrWhiteSpace(fontWeight) &&
								 Enum.TryParse<UIFontWeight>(fontWeight, true, out UIFontWeight uIFontWeight) )
							{
								result = UIFont.SystemFontOfSize(size, uIFontWeight);
								return result;
							}

							result = UIFont.SystemFontOfSize(size, UIFontWeight.Regular);
							return result;
						}

						result ??= UIFont.FromName(family, size);

						if ( result is not null )
							return result;
					}
					catch { Debug.WriteLine("Could not load font named: {0}", family); }
				}
			}

			if ( bold && italic )
			{
				using UIFont defaultFont = UIFont.SystemFontOfSize(size);

				UIFontDescriptor descriptor = defaultFont.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold | UIFontDescriptorSymbolicTraits.Italic);
				return UIFont.FromDescriptor(descriptor, 0);
			}

			if ( italic ) return UIFont.ItalicSystemFontOfSize(size);

			if ( bold ) return UIFont.BoldSystemFontOfSize(size);

			return UIFont.SystemFontOfSize(size);
		}

		private static string CleanseFontName( string fontName )
		{
			//First check Alias
			( bool hasFontAlias, string fontPostScriptName ) = FontRegistrar.HasFont(fontName);
			if ( hasFontAlias ) return fontPostScriptName;

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
					string formatted = fontFile.FileNameWithExtension(ext);
					( bool hasFont, string filePath ) = FontRegistrar.HasFont(formatted);
					if ( hasFont ) return filePath;
				}
			}

			return fontFile.PostScriptName;
		}

		private struct ToNativeFontFontKey
		{
			internal ToNativeFontFontKey( string? family, float size, FontAttributes attributes )
			{
				_family = family;
				_size = size;
				_attributes = attributes;
			}

#pragma warning disable 0414 // these are not called explicitly, but they are used to establish uniqueness. allow it!
			private string? _family;
			private float _size;
			private FontAttributes _attributes;
#pragma warning restore 0414
		}
	}
}