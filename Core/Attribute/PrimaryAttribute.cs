using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Attribute
{
    /// <summary>
    /// Used to indicate a primary key field on a component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public class PrimaryAttribute : RequiredAttribute { }
}
