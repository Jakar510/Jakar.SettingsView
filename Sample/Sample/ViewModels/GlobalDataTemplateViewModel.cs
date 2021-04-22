using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Jakar.SettingsView.Sample.Shared.ViewModels
{
	public class GlobalDataTemplateViewModel
	{
		public ObservableCollection<SectionItem> ItemsSource { get; } = new();
		public GlobalDataTemplateViewModel()
		{
			ItemsSource.Add(new SectionItem(new List<SettingItem>
											{
												new()
												{
													Title = "TitleA",
													Name = "AAA"
												},
												new()
												{
													Title = "TitleB",
													Name = "BBB"
												},
											})
							{
								SectionTitle = "SectionA"
							});

			ItemsSource.Add(new SectionItem(new List<SettingItem>
											{
												new()
												{
													Title = "TitleC",
													Name = "CCC"
												},
												new()
												{
													Title = "TitleD",
													Name = "DDD"
												},
											})
							{
								SectionTitle = "SectionB"
							});
		}

		public class SettingItem
		{
			public string Title { get; set; }
			public string Name { get; set; }
		}

		public class SectionItem : ObservableCollection<SettingItem>
		{
			public SectionItem( IEnumerable<SettingItem> list ) : base(list) { }
			public string SectionTitle { get; set; }
		}
	}
}