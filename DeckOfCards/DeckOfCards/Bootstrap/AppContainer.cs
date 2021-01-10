using Autofac;
using DeckOfCards.Contracts.Services;
using DeckOfCards.Services;
using DeckOfCards.Services.Data;
using DeckOfCards.ViewModels;
using System;
using Xamarin.Forms;

namespace DeckOfCards.Bootstrap
{
    public class AppContainer
    {
        private static IContainer _container;

        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            //ViewModels
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<MainTabbedViewModel>();
            builder.RegisterType<WorkoutViewModel>();
            builder.RegisterType<EditDeckViewModel>();
            builder.RegisterType<HistoryViewModel>();
            builder.RegisterType<WorkoutDetailsViewModel>();
            builder.RegisterType<SettingsViewModel>();
            builder.RegisterType<WorkoutRemindersViewModel>();

            //services - general
            builder.RegisterType<NavigationService>().As<INavigationService>();
            builder.RegisterType<DeckDataService>().As<IDeckDataService>();
            builder.RegisterType<PopupService>().As<IPopupService>();
            builder.RegisterType<WorkoutService>().As<IWorkoutService>();

            if (Device.RuntimePlatform == Device.iOS)
            {
                builder.RegisterType<IViewLifecycleService>();
            }

            builder.RegisterType<DeckOfCardsDB>();

            _container = builder.Build();
        }

        public static object Resolve(Type typeName)
        {
            return _container.Resolve(typeName);
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
