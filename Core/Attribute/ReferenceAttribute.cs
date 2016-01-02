using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Attribute
{
    /// <summary>
    /// Indicates that the property is a reference to an object of another type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public class ReferenceAttribute : DataTypeAttribute
    {
        /// <summary>
        /// The type of the referenced object
        /// </summary>
        public Type ReferenceType { get; protected set; }
        /// <summary>
        /// Action
        /// </summary>
        public string Action { get; protected set; }
        /// <summary>
        /// Area
        /// </summary>
        public string Area { get; protected set; }
        /// <summary>
        /// Method
        /// </summary>
        public string Method { get; protected set; }

        /// <summary>
        /// Display as text
        /// </summary>
        public bool DisplayAsText { get; protected set; }

        /// <summary>
        /// Public constructor
        /// </summary>
        public ReferenceAttribute(Type referenceType, string customDataType = "ForeignKey", string action = "Details", string area = "", string method = null, bool displayAsText = false
            )
            : base(customDataType)
        {
            ReferenceType = referenceType;
            Action = action;
            Area = area;
            Method = method;
            DisplayAsText = displayAsText;
        }

        /// <summary>
        /// Alternate constructor for displayAsText as second param
        /// </summary>
        public ReferenceAttribute(Type referenceType, bool displayAsText, string customDataType = "ForeignKey", string action = "Details", string area = "", string method = null
            )
            : base(customDataType)
        {
            ReferenceType = referenceType;
            Action = action;
            Area = area;
            Method = method;
            DisplayAsText = displayAsText;
        }
    }

    /// <summary>
    /// Indicates that the property is a reference to an object of another type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public class ReferencesAttribute : ReferenceAttribute
    {
        /// <summary>
        /// Public constructor
        /// </summary>
        public ReferencesAttribute(Type referenceType, string customDataType = "ForeignKeys", string action = "Details",
            string area = "", string method = null)
            : base(referenceType, customDataType, action, area, method)
        {
            ReferenceType = referenceType;
            Action = action;
            Area = area;
            Method = method;
        }
    }
}
