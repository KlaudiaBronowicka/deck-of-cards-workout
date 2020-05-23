using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DeckOfCards
{
    public partial class SmallDeviceStyle : ResourceDictionary
    {
        public static SmallDeviceStyle SharedInstance { get; } = new SmallDeviceStyle();

        public SmallDeviceStyle()
        {
            InitializeComponent();
        }
    }
}
