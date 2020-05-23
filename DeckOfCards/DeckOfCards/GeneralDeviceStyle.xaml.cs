using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DeckOfCards
{
    public partial class GeneralDeviceStyle : ResourceDictionary
    {
        public static GeneralDeviceStyle SharedInstance { get; } = new GeneralDeviceStyle();

        public GeneralDeviceStyle()
        {
            InitializeComponent();
        }
    }
}
