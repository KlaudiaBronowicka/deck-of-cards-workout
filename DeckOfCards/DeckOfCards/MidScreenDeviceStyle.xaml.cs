using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DeckOfCards
{
    public partial class MidScreenDeviceStyle : ResourceDictionary
    {
        public static MidScreenDeviceStyle SharedInstance { get; } = new MidScreenDeviceStyle();

        public MidScreenDeviceStyle()
        {
            InitializeComponent();
        }
    }
}
