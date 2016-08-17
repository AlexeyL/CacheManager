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

        /// <summary>
        /// <para>Method add object to cache</para>
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">object</param>
        /// <seealso cref="Add(string, T, DateTime)"/>
        /// <seealso cref="Add(string, T, DateTime, CacheEntryUpdateCallback)"/>
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

        /// <summary>
        /// <para>Method add object to cache</para>
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">object</param>
        /// <param name="expire">expiration time</param>
        /// <seealso cref="Add(string, T)"/>
        /// <seealso cref="Add(string, T, DateTime, CacheEntryUpdateCallback)"/>
        public void Add(string key, T obj, DateTime expire)
        {
            this.Add(key, obj, expire, null);
        }

        /// <summary>
        /// <para>Method add object to cache</para>
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">object</param>
        /// <param name="expire">expiration time</param>
        /// <param name="callback">method that will call befor object will deleted from cache</param>
        /// <seealso cref="Add(string, T)"/>
        /// <seealso cref="Add(string, T, DateTime)"/>
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

        /// <summary>
        /// <para>Method try to get strongly typed object from cache by key</para>
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">obj</param>
        /// <return>true if object exist in cache else fasle</return>
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
