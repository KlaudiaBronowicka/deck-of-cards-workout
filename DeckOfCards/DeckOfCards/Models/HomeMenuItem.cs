using System;
using System.Collections.Generic;
using System.Text;

namespace DeckOfCards.Models
{
    public enum MenuItemType
    {
        EditDeck
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
