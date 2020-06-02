using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dister4Net.Helpers
{
    public static class LinqHelpers
    {
        public static void WaitUntil(this Func<bool> func)
        {
            while (!func())
            {

            }
        }
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var item in @this)
            {
                action(item);
            }
        }
        public static void IfTrue(this bool @this, Action action)
        {
            if (@this)
                action();
        }
        public static void IfFalse(this bool @this, Action action)
        {
            if (!@this)
                action();
        }
        public static void IfNotNull(this object @this, Action notNull, Action isNull)
        {
            if (@this == null)
                isNull();
            else
                notNull();
        }
        public static T ConvertTo<T>(this object obj)
        {
            return (T) Convert.ChangeType(obj, typeof(T));
        }
        public static bool HasAttribute(this MethodInfo m, Type t)
            => m.GetCustomAttributes(t, true).Length > 0;
        public static IEnumerable<MethodInfo> WithAttribute(this IEnumerable<MethodInfo> methods, Type type)
            => methods.Where(x => x.HasAttribute(type));
    }
}
