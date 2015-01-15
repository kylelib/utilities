using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Common.Tools.Cache
{
    public partial class RequestCacheHelper
    {
        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static bool Add<T>(string cacheKey, T objectToCache) where T : class
        {
            bool addSuccessful = false;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[cacheKey] = objectToCache;
                addSuccessful = true;
            }

            return addSuccessful;
        }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public static bool Get<T>(string key, out T retrievedObject) where T : class
        {
            retrievedObject = null;

            if (HttpContext.Current != null)
            {
                retrievedObject = (T)HttpContext.Current.Items[key];
            }

            return retrievedObject != null;
        }
    }
}
