using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.NavigationParams;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
    public class CompanyDetailPageViewModel : ViewModelBase
    {
        private readonly ICompanyFacade _companyFacade;
        private readonly IPageDialogService _pageDialogService;

        public CompanyDetailPageViewModel(INavigationService navigationService, ICompanyFacade companyFacade,
            IPageDialogService pageDialogService) : base(navigationService)
        {
            _companyFacade = companyFacade;
            _pageDialogService = pageDialogService;
            SaveCommand = new DelegateCommand(async () => await SaveAsync());
            DeleteCommand = new DelegateCommand(async () => await DeleteAsync());
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand DeleteCommand { get; }

        private ICompanyModel _selectedCompany;

        public ICompanyModel SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                if (_selectedCompany == value) return;
                _selectedCompany = value;
                RaisePropertyChanged();
            }
        }

        protected override void OnNavigatedToInt(INavigationParameters parameters)
        {
            base.OnNavigatedToInt(parameters);
            CompanyNavigationParameters navParameters = new CompanyNavigationParameters(parameters);
            SelectedCompany = navParameters.CompanyModel;
        }

        private async Task SaveAsync()
        {
            await _companyFacade.UpdateAsync(SelectedCompany);
            await NavigationService.GoBackAsync();
        }

        private async Task DeleteAsync()
        {
            IsDialogThrown = true;
            if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning,
                    TranslateViewModelsSR.SelectedCompanyDeleteDialogMessageFormat(SelectedCompany.Name), TranslateViewModelsSR.DialogYes,
                    TranslateViewModelsSR.DialogNo))
            {
                await _companyFacade.RemoveAsync(SelectedCompany.Id);
                await NavigationService.GoBackAsync();
            }

            IsDialogThrown = false;
        }
    }
}