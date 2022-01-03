using System;
using Prism.Common;
using Prism.Navigation;
using Prism.Services.Dialogs;

namespace WorkManager.BL.NavigationParams
{
    public class FilterNavigationParameters : ParametersBase, IDialogParameters, INavigationParameters
    {
        public string Title { get; set; }
        public DateTime DateFrom { get; }
        public DateTime DateTo { get; }

        public FilterNavigationParameters(string title, DateTime dateFrom,DateTime dateTo)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
            Title = title;
            Add(nameof(Title), Title);
            Add(nameof(DateFrom), DateFrom);
            Add(nameof(DateTo), DateTo);
        }

        public FilterNavigationParameters(IParameters parameters)
        {
            Title = parameters.GetValue<string>(nameof(Title));
            DateFrom = parameters.GetValue<DateTime>(nameof(DateFrom));
            DateTo = parameters.GetValue<DateTime>(nameof(DateTo));
        }
    }
}