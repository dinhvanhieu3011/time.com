using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace BMBSOFT.GIS.CORE.Extensions
{
    public static class SortExtensions
    {
        public static IQueryable Sort(this IQueryable collection, string sortBy, bool reverse = false)
        {
            return string.IsNullOrWhiteSpace(sortBy) ? collection : collection.OrderBy(sortBy + (reverse ? " descending" : ""));
        }
        public static IQueryable<T> Sort<T>(this IQueryable<T> collection, string sortByFields)
        {
            return string.IsNullOrWhiteSpace(sortByFields) ? collection : collection.OrderBy(sortByFields);
        }
        public static IOrderedEnumerable<T> SortOnMemory<T>(this IEnumerable<T> Source, string sortExpression)
        {
            var sortInfo = sortExpression.Split(' ');
            var memberName = sortInfo[0].Trim();
            var sortDirection = sortInfo.Length == 2 ? sortInfo[1].Trim() : "asc";

            MemberInfo mi = typeof(T).GetField(memberName);
            if (mi == null)
                mi = typeof(T).GetProperties().FirstOrDefault(x => x.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));

            //mi = typeof(T).GetProperty(memberName,t);
            if (mi == null)
                throw new InvalidOperationException("Field or property '" + memberName + "' not found");

            // get the field or property's type, and make a delegate type that takes a T and returns this member's type
            var memberType = mi is FieldInfo ? (mi as FieldInfo).FieldType : (mi as PropertyInfo).PropertyType;
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), memberType);

            // we need to call ValueProxy.ReturnValue, which is a generic type
            var genericReturnValueMethod = typeof(ValueProxy).GetMethod("GetValue");

            // it's type parameters are MemberType and <T>, so we "make" a method to bake in those specific types
            var getValueMethod = genericReturnValueMethod.MakeGenericMethod(memberType, typeof(T));

            var proxy = new ValueProxy(mi);

            // now create a delegate using the delegate type and method we just constructed
            var methodDelegate = Delegate.CreateDelegate(delegateType, proxy, getValueMethod);

            // method name on IEnumerable/IOrderedEnumerable to call later
            // do we already have at least one sort on this collection?
            var methodName = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase) ? "OrderBy" : "OrderByDescending";
            var method = typeof(Enumerable).GetMethods()
                .Single(m => m.Name == methodName && m.MakeGenericMethod(typeof(int), typeof(int)).GetParameters().Length == 2);
            var result = method.MakeGenericMethod(typeof(T), memberType)
                .Invoke(Source, new object[] { Source, methodDelegate });
            return result as IOrderedEnumerable<T>;
        }
    }
    public class ValueProxy
    {
        private MemberInfo Member;

        public T GetValue<T, TKey>(TKey obj)
        {
            if (Member is FieldInfo)
                return (T)(Member as FieldInfo).GetValue(obj);
            if (Member is PropertyInfo)
                return (T)(Member as PropertyInfo).GetValue(obj, null);

            return default(T);
        }

        public ValueProxy(MemberInfo Member)
        {
            this.Member = Member;
        }
    }
}