using CacheManager.Abstarct;
using System;
using System.Runtime.Caching;

namespace CacheManager.Concrete
{
    public class CacheManager<T> : ICacheManager<T>
    {
        ObjectCache cacheInstance;
        CacheItem itemField;
        CacheItemPolicy policyField;

        public CacheManager()
        {
            this.cacheInstance = MemoryCache.Default;
        }

        #region ICacheAdapter<T> Members

        public void Add(string key, T obj)
        {
            if (!this.cacheInstance.Contains(key))
            {
                this.itemField = new CacheItem(key, obj);

                this.policyField = new CacheItemPolicy();
                this.policyField.Priority = CacheItemPriority.NotRemovable;

                this.Add();
            }
        }

        public void Add(string key, T obj, DateTime expire)
        {
            this.Add(key, obj, expire, null);
        }

        public void Add(string key, T obj, DateTime expire, CacheEntryUpdateCallback callback)
        {
            if (!this.cacheInstance.Contains(key))
            {
                this.itemField = new CacheItem(key, obj);

                // Сreate CacheItemPolicy with default Priority
                this.policyField = new CacheItemPolicy();
                this.policyField.AbsoluteExpiration = new DateTimeOffset(expire);

                if (callback != null)
                    this.policyField.UpdateCallback = callback;

                this.Add();
            }
        }

        public bool TryGetValue(string key, out T obj)
        {
            bool bSuccess;

            if (!String.IsNullOrWhiteSpace(key) && this.cacheInstance.Contains(key))
            {
                try
                {
                    object objCacheItemValue = this.cacheInstance.Get(key);
                    obj = (T)Convert.ChangeType(objCacheItemValue, typeof(T));
                    bSuccess = true;
                }
                catch
                {
                    obj = default(T);
                    bSuccess = false;
                }
            }
            else
            {
                obj = default(T);
                bSuccess = false;
            }

            return bSuccess;
        }

        #endregion

        void Add()
        {
            cacheInstance.Set(this.itemField, this.policyField);
        }
    }
}
