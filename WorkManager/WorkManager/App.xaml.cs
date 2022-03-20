using System;
using CommonServiceLocator;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Unity;
using Unity.ServiceLocation;
using WorkManager.BL.Facades;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.BL.Mappers;
using WorkManager.BL.Providers;
using WorkManager.BL.Services;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.Extensions;
using WorkManager.Logger;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;
using WorkManager.Models.Interfaces.ModelServices;
using WorkManager.ViewModels.Dialogs;
using WorkManager.ViewModels.Pages;
using WorkManager.Views.Dialogs;
using WorkManager.Views.Pages;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
[assembly: ExportFont("fa-solid-900.ttf")]
namespace WorkManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : PrismApplication
    {

        private static readonly TimeSpan ApplicationTimeout = TimeSpan.FromSeconds(180);
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            IUnityContainer container = containerRegistry.GetContainer();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            container.Resolve<LoggerRegistrator>().Register();
            container.RegisterSingleton<IToastMessageService, ToastMessageService>();
            RegisterXamarinEssentials(containerRegistry);
            RegisterDbContext(container);
            RegisterFactories(container);
            RegisterMappers(container);
            RegisterFacades(container);
            RegisterServices(container);
            RegisterProviders(container);
            RegisterNavigation(containerRegistry);
        }

        private void RegisterXamarinEssentials(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterSingleton<IPreferences, PreferencesImplementation>();
            containerRegistry.RegisterSingleton<IVersionTracking, VersionTrackingImplementation>();
            containerRegistry.RegisterSingleton<IDeviceInfo, DeviceInfoImplementation>();
            containerRegistry.RegisterSingleton<IMainThread, MainThreadImplementation>();
        }

        private void RegisterDbContext(IUnityContainer container)
        {
            container.RegisterSingleton<WorkManagerDbContext>();
        }
        
        private void RegisterFactories(IUnityContainer container)
        {
            container.RegisterSingleton<IWorkRecordModelFactory, WorkRecordModelFactory>();
        }

        private void RegisterMappers(IUnityContainer container)
        {
            container.RegisterMultipleTypeSingleton<IWorkRecordMapper, IMapper<WorkRecordEntity, IWorkRecordModelBase>, WorkRecordMapper>();
            container.RegisterMultipleTypeSingleton<ICompanyMapper, IMapper<CompanyEntity, ICompanyModel>, CompanyMapper>();
            container.RegisterMultipleTypeSingleton<IRelatedTaskMapper, IMapper<RelatedTaskEntity, IRelatedTaskModel>, RelatedTaskMapper>();
            container.RegisterMultipleTypeSingleton<ITaskMapper, IMapper<TaskEntity, ITaskModel>, TaskMapper>();
            container.RegisterMultipleTypeSingleton<ITaskGroupMapper, IMapper<TaskGroupEntity, ITaskGroupModel>, TaskGroupMapper>();
            container.RegisterMultipleTypeSingleton<IUserMapper, IMapper<UserEntity, IUserModel>, UserMapper>();
            container.RegisterMultipleTypeSingleton<IKanbanStateMapper, IMapper<KanbanStateEntity, IKanbanStateModel>, KanbanStateMapper>();
            container.RegisterMultipleTypeSingleton<IImageMapper, IMapper<ImageEntity, IImageModel>, ImageMapper>();

        }

        private void RegisterFacades(IUnityContainer container)
        {
            container.RegisterMultipleTypeSingleton<IUserFacade, IFacade<IUserModel>, UserFacade>();
            container.RegisterMultipleTypeSingleton<ICompanyFacade, IFacade<ICompanyModel>, CompanyFacade>();
            container.RegisterMultipleTypeSingleton<ITaskGroupFacade, IFacade<ITaskGroupModel>, TaskGroupFacade>();
            container.RegisterMultipleTypeSingleton<IRelatedTaskFacade, IFacade<IRelatedTaskModel>, RelatedTaskFacade>();
            container.RegisterMultipleTypeSingleton<ITaskFacade, IFacade<ITaskModel>, TaskFacade>();
            container.RegisterMultipleTypeSingleton<IKanbanStateFacade, IFacade<IKanbanStateModel>, KanbanStateFacade>();
            container.RegisterMultipleTypeSingleton<IWorkRecordFacade, IFacade<IWorkRecordModelBase>, WorkRecordFacade>();
            container.RegisterMultipleTypeSingleton<IImageFacade, IFacade<IImageModel>, ImageFacade>();
        }

        private void RegisterProviders(IUnityContainer container)
        {
            container.RegisterSingleton<IServerCurrentTimeProvider, ServerCurrentTimeProvider>();
            container.RegisterMultipleTypeSingleton<ICurrentModelProvider<IUserModel>, ICurrentModelProviderManager<IUserModel>, CurrentModelProvider<IUserModel>>();
            container.RegisterMultipleTypeSingleton<ICurrentModelProvider<ITaskGroupModel>, ICurrentModelProviderManager<ITaskGroupModel>, CurrentModelProvider<ITaskGroupModel>>();
            container.RegisterMultipleTypeSingleton<ICurrentModelProvider<ICompanyModel>, ICurrentModelProviderManager<ICompanyModel>, CurrentModelProvider<ICompanyModel>>();
        }

        private void RegisterServices(IUnityContainer container)
        {
            container.RegisterSingleton<IAuthenticationService, AuthenticationService>();
            container.RegisterSingleton<IRegistrationService, RegistrationService>();
            container.RegisterSingleton<IRecordTotalCalculatorService, RecordTotalCalculatorService>();
            container.RegisterSingleton<DialogEventService>();
            container.RegisterSingleton<IPhotoService, PhotoService>();

            container.RegisterSingleton<SettingsService>();
            container.RegisterSingleton<ISettingsServiceProvider, SettingsService>();
            container.RegisterSingleton<ISettingsServiceManager, SettingsService>();
            container.RegisterSingleton<WorkManagerSettingsService>();

            //volání async bez await pokud nastane vyjímka tak zapadne...
            container.RegisterAndResolve<IDatabaseSessionController, DatabaseSessionController>().Initialize(ApplicationTimeout);

            //model services
            container.RegisterSingleton<IRecordCalculatorService, RecordCalculatorService>();
        }

        private void RegisterNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<RootPage, RootPageViewModel>();
            containerRegistry.RegisterForNavigation<TaskGroupPage, TaskGroupPageViewModel>();
            containerRegistry.RegisterForNavigation<CompanyPage, CompanyPageViewModel>();
            containerRegistry.RegisterForNavigation<WorkRecordPage, WorkRecordPageViewModel>();
            containerRegistry.RegisterForNavigation<TaskKanbanPage, TaskKanbanPageViewModel>();
            containerRegistry.RegisterForNavigation<TaskDetailPage, TaskDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<WorkRecordDetailPage, WorkRecordDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<UserDetailPage, UserDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<TaskGroupDetailPage, TaskGroupDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<CompanyDetailPage, CompanyDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<WorkRecordStatisticsPage, WorkRecordStatisticsPageViewModel>();
            containerRegistry.RegisterForNavigation<RelatedTasksPage, RelatedTasksPageViewModel>();


            containerRegistry.RegisterDialog<AddWorkRecordDialog, AddWorkRecordDialogViewModel>();
            containerRegistry.RegisterDialog<AddCompanyDialog, AddCompanyDialogViewModel>();
            containerRegistry.RegisterDialog<AddTaskGroupDialog, AddTaskGroupDialogViewModel>();
            containerRegistry.RegisterDialog<AddTaskDialog, AddTaskDialogViewModel>();
            containerRegistry.RegisterDialog<FilterDialog, FilterDialogViewModel>();
            containerRegistry.RegisterDialog<SelectPictureOrCaptureCameraDialog, SelectPictureOrCaptureCameraDialogViewModel>();
            containerRegistry.RegisterDialog<ImageDetailDialog, ImageDetailDialogViewModel>();
            containerRegistry.RegisterDialog<AddKanbanStateDialog, AddKanbanStateDialogViewModel>();
            containerRegistry.RegisterDialog<ChangePasswordDialog, ChangePasswordDialogViewModel>();

            //ViewModelLocationProvider.Register<CompanyEmptyView, CompanyEmptyViewModel>();
            //ViewModelLocationProvider.Register<TaskGroupEmptyView, TaskGroupEmptyViewModel>();
            //ViewModelLocationProvider.Register<WorkRecordEmptyView, WorkRecordEmptyViewModel>();
        }
    }
}

