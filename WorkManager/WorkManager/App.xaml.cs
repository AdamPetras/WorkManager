using CommonServiceLocator;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Unity;
using Unity.ServiceLocation;
using Prism.Mvvm;
using WorkManager.BL.Facades;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.BL.Mappers;
using WorkManager.BL.Providers;
using WorkManager.BL.Services;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories;
using WorkManager.DAL.DbContext.Interfaces;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Extensions;
using WorkManager.Models;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;
using WorkManager.Models.Interfaces.ModelServices;
using WorkManager.ViewModels.Dialogs;
using WorkManager.ViewModels.Pages;
using WorkManager.ViewModels.Views;
using WorkManager.Views.Dialogs;
using WorkManager.Views.Pages;
using WorkManager.Views.Views;
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
			containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
			IUnityContainer container = containerRegistry.GetContainer();
			ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
			container.RegisterSingleton<IToastMessageService, ToastMessageService>();
			RegisterXamarinEssentials(containerRegistry);
			RegisterDbContext(container);
			RegisterRepositories(container);
			RegisterFactories(container);
			RegisterMappers(container);
			RegisterFacades(container);
			RegisterServices(container);
			RegisterProviders(container);
			RegisterNavigation(containerRegistry);
		}

		private void RegisterXamarinEssentials(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<IPreferences, PreferencesImplementation>();
			containerRegistry.RegisterSingleton<IVersionTracking, VersionTrackingImplementation>();
			containerRegistry.RegisterSingleton<IDeviceInfo, DeviceInfoImplementation>();
			containerRegistry.RegisterSingleton<IMainThread, MainThreadImplementation>();
		}

		private void RegisterDbContext(IUnityContainer container)
		{
			container.RegisterSingleton<IDBContextFactory<WorkManagerDbContext>, WorkManagerDbContextFactory>();
			container.Resolve<WorkManagerDbContextFactory>().CreateDbContext();
		}

		private void RegisterRepositories(IUnityContainer container)
		{
			container.RegisterMultipleTypeSingleton<IUserRepository, IRepository<UserEntity>, UserRepository>();
			container.RegisterMultipleTypeSingleton<ICompanyRepository, IRepository<CompanyEntity>, CompanyRepository>();
			container.RegisterMultipleTypeSingleton<ITaskGroupRepository, IRepository<TaskGroupEntity>, TaskGroupRepository>();
			container.RegisterMultipleTypeSingleton<ITaskRepository, IRepository<TaskEntity>, TaskRepository>();
			container.RegisterMultipleTypeSingleton<IWorkRecordRepository, IRepository<WorkRecordEntity>, WorkRecordRepository>();
			container.RegisterMultipleTypeSingleton<IKanbanStateRepository, IRepository<KanbanStateEntity>, KanbanStateRepository>();
			container.RegisterMultipleTypeSingleton<IKanbanTaskGroupRepository, IRepository<KanbanTaskGroupEntity>, KanbanTaskGroupRespository>();
			container.RegisterMultipleTypeSingleton<IImageRepository, IRepository<ImageEntity>, ImageRepository>();
		}

		private void RegisterFactories(IUnityContainer container)
		{
			container.RegisterSingleton<IWorkRecordModelFactory, WorkRecordModelFactory>();
		}

		private void RegisterMappers(IUnityContainer container)
		{
			container.RegisterSingleton<IMapper<WorkRecordEntity, IWorkRecordModelBase>, WorkRecordMapper>();
			container.RegisterSingleton<IMapper<CompanyEntity, ICompanyModel>, CompanyMapper>();
			container.RegisterSingleton<IMapper<TaskEntity, ITaskModel>, TaskMapper>();
			container.RegisterSingleton<IMapper<TaskGroupEntity, ITaskGroupModel>, TaskGroupMapper>();
			container.RegisterSingleton<IMapper<UserEntity, IUserModel>, UserMapper>();
			container.RegisterSingleton<IMapper<KanbanStateEntity, IKanbanStateModel>, KanbanStateMapper>();
			container.RegisterSingleton<IMapper<KanbanTaskGroupEntity, IKanbanTaskGroupModel>, KanbanTaskGroupMapper>();
			container.RegisterSingleton<IMapper<ImageEntity, IImageModel>, ImageMapper>();

		}

		private void RegisterFacades(IUnityContainer container)
		{
			container.RegisterMultipleTypeSingleton<IUserFacade, IFacade<IUserModel>, UserFacade>();
			container.RegisterMultipleTypeSingleton<ICompanyFacade, IFacade<ICompanyModel>, CompanyFacade>();
			container.RegisterMultipleTypeSingleton<ITaskGroupFacade, IFacade<ITaskGroupModel>, TaskGroupFacade>();
			container.RegisterMultipleTypeSingleton<ITaskFacade, IFacade<ITaskModel>, TaskFacade>();
			container.RegisterMultipleTypeSingleton<IKanbanStateFacade, IFacade<IKanbanStateModel>, KanbanStateFacade>();
			container.RegisterMultipleTypeSingleton<IKanbanTaskGroupFacade, IFacade<IKanbanTaskGroupModel>, KanbanTaskGroupFacade>();
			container.RegisterMultipleTypeSingleton<IWorkRecordFacade, IFacade<IWorkRecordModelBase>, WorkRecordFacade>();
			container.RegisterMultipleTypeSingleton<IImageFacade, IFacade<IImageModel>, ImageFacade>();
		}

		private void RegisterProviders(IUnityContainer container)
		{
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


			containerRegistry.RegisterDialog<AddWorkRecordDialogView, AddWorkRecordDialogViewModel>();
			containerRegistry.RegisterDialog<AddCompanyDialogView, AddCompanyDialogViewModel>();
			containerRegistry.RegisterDialog<AddTaskGroupDialogView, AddTaskGroupDialogViewModel>();
			containerRegistry.RegisterDialog<AddTaskDialogView, AddTaskDialogViewModel>();
			containerRegistry.RegisterDialog<FilterDialogView, FilterDialogViewModel>();
			containerRegistry.RegisterDialog<SelectPictureOrCaptureCameraDialog, SelectPictureOrCaptureCameraDialogViewModel>();
			containerRegistry.RegisterDialog<ImageDetailDialog, ImageDetailDialogViewModel>();

			ViewModelLocationProvider.Register<CompanyEmptyView, CompanyEmptyViewModel>();
			ViewModelLocationProvider.Register<TaskGroupEmptyView, TaskGroupEmptyViewModel>();
			ViewModelLocationProvider.Register<WorkRecordEmptyView, WorkRecordEmptyViewModel>();
		}
	}
}

