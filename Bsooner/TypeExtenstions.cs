using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Bsooner
{
    public static class TypeExtenstions
    {
        public static MemberInfo Member<T, TTProperty>(this Expression<Func<T, TTProperty>> property)
        {
            return ((MemberExpression)property.Body).Member;
        }
    }
}