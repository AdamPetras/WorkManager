using Prism.Common;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.NavigationParams
{
    public class CompanyNavigationParameters : ParametersBase, IDialogParameters, INavigationParameters
    {
        public ICompanyModel CompanyModel { get; }

        public CompanyNavigationParameters(ICompanyModel companyModel)
        {
            CompanyModel = companyModel;
            Add("Company", companyModel);
        }

        public CompanyNavigationParameters(IParameters parameters)
        {
            CompanyModel = parameters.GetValue<ICompanyModel>("Company");
        }
    }
}