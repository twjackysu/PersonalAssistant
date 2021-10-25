using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PersonalAssistant.Extension
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> propertySelector)
        {
            foreach (var item in source)
            {
                propertySelector(item);
                yield return item;
            }
        }
    }
}
