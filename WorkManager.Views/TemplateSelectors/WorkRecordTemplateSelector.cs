using System;
using WorkManager.Models.Interfaces;
using Xamarin.Forms;

namespace WorkManager.Views.TemplateSelectors
{
	public class WorkRecordTemplateSelector:DataTemplateSelector
	{
		public DataTemplate PiecesRecordDataTemplate { get; set; }
		public DataTemplate TimeRecordDataTemplate { get; set; }
		public DataTemplate BothRecordDataTemplate { get; set; }

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			return item switch
			{
				IWorkBothRecordModel _ => BothRecordDataTemplate,
				IWorkTimeRecordModel _ => TimeRecordDataTemplate,
				IWorkPiecesRecordModel _ => PiecesRecordDataTemplate,
				_ => throw new ArgumentException()
			};
		}
	}
}