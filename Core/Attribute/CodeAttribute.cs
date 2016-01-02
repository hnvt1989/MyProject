using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Attribute
{
    /// <summary>
    /// Used to indicate that the field must be a valid Code (rules depend on
    /// configuration, but typically something like [a-zA-Z][a-zA-Z0-9_]+).
    /// </summary>
    public class CodeAttribute : ValidationAttribute
    {
        /// <see cref="ValidationAttribute.IsValid(object)"/>
        public override bool IsValid(object value)
        {
            return value == null || Code.ValidCodeRegex.IsMatch((string)value);
        }
    }
}
