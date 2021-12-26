using System;
using Prism.Common;
using Prism.Navigation;
using Prism.Services.Dialogs;

namespace WorkManager.BL.NavigationParams
{
    public class FilterNavigationParameters : ParametersBase, IDialogParameters, INavigationParameters
    {
        public DateTime DateFrom { get; }
        public DateTime DateTo { get; }

        public FilterNavigationParameters(DateTime dateFrom,DateTime dateTo)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
            Add(nameof(DateFrom), DateFrom);
            Add(nameof(DateTo), DateTo);
        }

        public FilterNavigationParameters(IParameters parameters)
        {
            DateFrom = parameters.GetValue<DateTime>(nameof(DateFrom));
            DateTo = parameters.GetValue<DateTime>(nameof(DateTo));
        }
    }
}