using System;

namespace Bsooner
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ObjectIdAttribute : Attribute
    {
    }
}