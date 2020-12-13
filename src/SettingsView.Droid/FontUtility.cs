using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public static class FontUtility
	{
		private static readonly ConcurrentDictionary<Tuple<string, FontAttributes>, Typeface> Typefaces = new ConcurrentDictionary<Tuple<string, FontAttributes>, Typeface>();

		private static Typeface s_defaultTypeface;

		public static Typeface CreateTypeface( string fontFamily, FontAttributes fontAttributes = FontAttributes.None )
		{
			if ( fontAttributes == FontAttributes.None &&
				 string.IsNullOrEmpty(fontFamily) ) { return s_defaultTypeface ?? ( s_defaultTypeface = Typeface.Default ); }

			return ToTypeface(fontFamily, fontAttributes);
		}

		private static Typeface ToTypeface( string fontFamily, FontAttributes fontAttributes )
		{
			fontFamily ??= String.Empty;
			return Typefaces.GetOrAdd(new Tuple<string, FontAttributes>(fontFamily, fontAttributes), CreateTypeface);
		}

		private static Typeface CreateTypeface( Tuple<string, FontAttributes> key )
		{
			Typeface result;
			string fontFamily = key.Item1;
			FontAttributes fontAttribute = key.Item2;

			if ( String.IsNullOrWhiteSpace(fontFamily) )
			{
				TypefaceStyle style = ToTypefaceStyle(fontAttribute);
				result = Typeface.Create(Typeface.Default, style);
			}
			else if ( IsAssetFontFamily(fontFamily) ) { result = Typeface.CreateFromAsset(Android.App.Application.Context.Assets, FontNameToFontFile(fontFamily)); }
			else { result = fontFamily.ToTypeFace(fontAttribute); }

			return result;
		}

		private static Typeface ToTypeFace( this string fontfamily, FontAttributes attr = FontAttributes.None )
		{
			fontfamily ??= String.Empty;
			(bool success, Typeface typeface) result = fontfamily.TryGetFromAssets();
			if ( result.success ) { return result.typeface; }
			else
			{
				TypefaceStyle style = ToTypefaceStyle(attr);
				return Typeface.Create(fontfamily, style);
			}
		}

		private static (bool success, Typeface typeface) TryGetFromAssets( this string fontName )
		{
			//First check Alias
			(bool hasFontAlias, string fontPostScriptName) = FontRegistrar.HasFont(fontName);
			if ( hasFontAlias )
				return ( true, Typeface.CreateFromFile(fontPostScriptName) );

			bool isAssetFont = IsAssetFontFamily(fontName);
			if ( isAssetFont ) { return LoadTypefaceFromAsset(fontName); }

			var folders = new[]
						  {
							  "",
							  "Fonts/",
							  "fonts/",
						  };


			//copied text
			FontFile fontFile = FontFile.FromString(fontName);

			if ( !string.IsNullOrWhiteSpace(fontFile.Extension) )
			{
				(bool hasFont, string fontPath) = FontRegistrar.HasFont(fontFile.FileNameWithExtension());
				if ( hasFont ) { return ( true, Typeface.CreateFromFile(fontPath) ); }
			}
			else
			{
				foreach ( string ext in FontFile.Extensions )
				{
					string formated = fontFile.FileNameWithExtension(ext);
					(bool hasFont, string fontPath) = FontRegistrar.HasFont(formated);
					if ( hasFont ) { return ( true, Typeface.CreateFromFile(fontPath) ); }

					foreach ( string folder in folders )
					{
						formated = $"{folder}{fontFile.FileNameWithExtension()}#{fontFile.PostScriptName}";
						(bool success, Typeface typeface) result = LoadTypefaceFromAsset(formated);
						if ( result.success )
							return result;
					}
				}
			}

			return ( false, null );
		}

		private static (bool success, Typeface typeface) LoadTypefaceFromAsset( string fontfamily )
		{
			try
			{
				var result = Typeface.CreateFromAsset(Android.App.Application.Context.Assets, FontNameToFontFile(fontfamily));
				return ( true, result );
			}
			catch ( Exception ex )
			{
				Debug.WriteLine(ex);
				return ( false, null );
			}
		}

		private static bool IsAssetFontFamily( string name ) => name != null && ( name.Contains(".ttf#") || name.Contains(".otf#") );

		private static TypefaceStyle ToTypefaceStyle( FontAttributes attrs )
		{
			var style = TypefaceStyle.Normal;
			if ( ( attrs & ( FontAttributes.Bold | FontAttributes.Italic ) ) == ( FontAttributes.Bold | FontAttributes.Italic ) )
				style = TypefaceStyle.BoldItalic;
			else if ( ( attrs & FontAttributes.Bold ) != 0 )
				style = TypefaceStyle.Bold;
			else if ( ( attrs & FontAttributes.Italic ) != 0 )
				style = TypefaceStyle.Italic;
			return style;
		}

		private static string FontNameToFontFile( string fontFamily )
		{
			fontFamily ??= String.Empty;
			int hashtagIndex = fontFamily.IndexOf('#');
			if ( hashtagIndex >= 0 )
				return fontFamily.Substring(0, hashtagIndex);

			throw new InvalidOperationException($"Can't parse the {nameof(fontFamily)} {fontFamily}");
		}
	}
}