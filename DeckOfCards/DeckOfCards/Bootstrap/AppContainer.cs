﻿using Autofac;
using DeckOfCards.Contracts.Services;
using DeckOfCards.Services;
using DeckOfCards.ViewModels;
using System;

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
            builder.RegisterType<MenuViewModel>();
            builder.RegisterType<WorkoutViewModel>();
            builder.RegisterType<EditDeckViewModel>();

            //services - data
            builder.RegisterType<MockDataStore>();

            //services - general
            builder.RegisterType<NavigationService>().As<INavigationService>();
            builder.RegisterType<DeckDataService>().As<IDeckDataService>();

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
