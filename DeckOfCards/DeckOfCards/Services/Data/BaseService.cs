using Akavache;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace DeckOfCards.Services
{
    public class BaseService
    {
        private IBlobCache _cache;
        protected IBlobCache Cache
        {
            get
            {
                _cache = _cache ?? BlobCache.LocalMachine;
                return _cache;
            }
        }
        
        public async Task<T> GetFromCache<T>(string cacheName)
        {
            try
            {
                T element = await Cache.GetObject<T>(cacheName);
                return element;
            }
            catch (KeyNotFoundException)
            {
                return default;
            }
        }

        public void InvalidateCache()
        {
            Cache.InvalidateAllObjects<object>();
        }

    }
}
