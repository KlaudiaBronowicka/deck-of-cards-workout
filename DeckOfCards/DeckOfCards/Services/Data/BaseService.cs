using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DeckOfCards.Services
{
    public class BaseService
    {
        protected DeckOfCardsDB _db;

        public BaseService(DeckOfCardsDB db)
        {
            _db = db;
        }
    }
}
