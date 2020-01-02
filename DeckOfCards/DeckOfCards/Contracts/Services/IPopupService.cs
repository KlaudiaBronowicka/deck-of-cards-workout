using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards.Contracts.Services
{
    public interface IPopupService
    {

        void Initialize();
        Task ShowDialog(string title, string message, string cancel);
        
    }
}
