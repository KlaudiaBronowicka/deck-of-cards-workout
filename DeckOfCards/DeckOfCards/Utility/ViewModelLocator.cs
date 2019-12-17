using DeckOfCards.Bootstrap;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace DeckOfCards.Utility
{
    public class ViewModelLocator
    {
        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool),
                typeof(ViewModelLocator), default(bool),
                propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(AutoWireViewModelProperty, value);
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Element view))
            {
                return;
            }

            var viewType = view.GetType();
            if (!string.IsNullOrEmpty(viewType.FullName))
            {
                var viewName = viewType.FullName
                                       .Replace(".Views.", ".ViewModels.")
                                       .Replace("Page", "ViewModel");

                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;

                var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewAssemblyName);

                var viewModelType = Type.GetType(viewModelName);
                if (viewModelType == null)
                {
                    return;
                }
                var viewModel = AppContainer.Resolve(viewModelType);
                view.BindingContext = viewModel;
            }
        }
    }
}
