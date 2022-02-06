using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.NavigationParams;
using WorkManager.Core;
using WorkManager.Extensions;
using WorkManager.Logger;
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
            IPageDialogService pageDialogService, ViewModelTaskExecute viewModelTaskExecute) : base(navigationService, viewModelTaskExecute)
        {
            _companyFacade = companyFacade;
            _pageDialogService = pageDialogService;
            SaveCommand = new DelegateCommand(async () => await SaveAsync());
            DeleteCommand = new DelegateCommand(async () => await DeleteAsync());
            NameMaxLenght = typeof(ICompanyModel).GetStringMaxLength(nameof(ICompanyModel.Name));
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

        public int NameMaxLenght { get; }

        protected override async Task OnNavigatedToAsyncInt(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsyncInt(parameters);
            CompanyNavigationParameters navParameters = new CompanyNavigationParameters(parameters);
            SelectedCompany = navParameters.CompanyModel;
        }

        private async Task SaveAsync()
        {
            await ViewModelTaskExecute.ExecuteTaskWithQueue(SelectedCompany, _companyFacade.UpdateAsync);
            await NavigationService.GoBackAsync();
        }

        private async Task DeleteAsync()
        {
            IsDialogThrown = true;
            if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning,
                    TranslateViewModelsSR.SelectedCompanyDeleteDialogMessage.Format(SelectedCompany.Name), TranslateViewModelsSR.DialogYes,
                    TranslateViewModelsSR.DialogNo))
            {
                await ViewModelTaskExecute.ExecuteTaskWithQueue(SelectedCompany.Id, _companyFacade.RemoveAsync);
                await NavigationService.GoBackAsync();
            }

            IsDialogThrown = false;
        }
    }
}