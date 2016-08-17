using System;
using System.Runtime.Caching;

namespace CacheManager.Abstarct
{
    public interface ICacheManager<T>
    {
        /// <summary>
        /// <para>Method add object to cache</para>
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">object</param>
        /// <seealso cref="Add(string, T, DateTime)"/>
        /// <seealso cref="Add(string, T, DateTime, CacheEntryUpdateCallback)"/>
        void Add(string key, T obj);

        /// <summary>
        /// <para>Method add object to cache</para>
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">object</param>
        /// <param name="expire">expiration time</param>
        /// <seealso cref="Add(string, T)"/>
        /// <seealso cref="Add(string, T, DateTime, CacheEntryUpdateCallback)"/>
        void Add(string key, T obj, DateTime expire);

        /// <summary>
        /// <para>Method add object to cache</para>
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">object</param>
        /// <param name="expire">expiration time</param>
        /// <param name="callback">method that will call befor object will deleted from cache</param>
        /// <seealso cref="Add(string, T)"/>
        /// <seealso cref="Add(string, T, DateTime)"/>
        void Add(string key, T obj, DateTime expire, CacheEntryUpdateCallback callback);

        /// <summary>
        /// <para>Method try to get strongly typed object from cache by key</para>
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">obj</param>
        /// <return>true if object exist in cache else fasle</return>
        bool TryGetValue(string key, out T obj);
    }
}
