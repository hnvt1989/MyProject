using System;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using JetBrains.Annotations;

namespace Core
{
    /// <summary>
    /// Represents the external identifier for some component.
    /// </summary>
    [Serializable]
    [XmlSchemaProvider("QualifiedName")]
    public class Code : IXmlSerializable, IConvertible
    {
        private string _Value;
        /// <summary>
        /// Regular expression that controls validation of codes
        /// </summary>
        public static Regex ValidCodeRegex { get; set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        public Code() { }

        /// <summary>
        /// Constructs a code with the given value.
        /// </summary>
        /// <param name="value"></param>
        public Code([NotNull]string value)
        {
            if (string.IsNullOrEmpty(value) || value.Trim() == string.Empty
                || (ValidCodeRegex != null && !ValidCodeRegex.IsMatch(value)))
            {
                if (string.IsNullOrEmpty(value) || value.Trim() == string.Empty)
                {
                    throw new Exception("CodeEmptyArgumentException");
                }
                else
                {
                    throw new Exception(string.Format("CodeRegExArgumentException. Argument = {0}", value));
                }
            }
            _Value = value;
        }

        /// <summary>
        /// The code's value.
        /// </summary>
        public virtual string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        #region IConvertible Members
        ///<summary>
        ///Returns the <see cref="T:System.TypeCode"></see> for this instance.
        ///</summary>
        ///
        ///<returns>
        ///The enumerated constant that is the <see cref="T:System.TypeCode"></see> of the class or value type that implements this interface.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///A Boolean value equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        public bool ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///A Unicode character equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        public char ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///An 8-bit signed integer equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        [CLSCompliantAttribute(false)]
        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///An 8-bit unsigned integer equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        public byte ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///An 16-bit signed integer equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        public short ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///An 16-bit unsigned integer equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        [CLSCompliantAttribute(false)]
        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///An 32-bit signed integer equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        public int ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///An 32-bit unsigned integer equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        [CLSCompliantAttribute(false)]
        public uint ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///An 64-bit signed integer equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        public long ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///An 64-bit unsigned integer equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        [CLSCompliantAttribute(false)]
        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///A single-precision floating-point number equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        public float ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///A double-precision floating-point number equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        public double ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent <see cref="T:System.Decimal"></see> number using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Decimal"></see> number equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent <see cref="T:System.DateTime"></see> using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.DateTime"></see> instance equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ///<summary>
        ///Converts the value of this instance to an equivalent <see cref="T:System.String"></see> using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> instance equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
        public string ToString(IFormatProvider provider)
        {
            return _Value;
        }

        ///<summary>
        ///Converts the value of this instance to an <see cref="T:System.Object"></see> of the specified <see cref="T:System.Type"></see> that has an equivalent value, using the specified culture-specific formatting information.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Object"></see> instance of type conversionType whose value is equivalent to the value of this instance.
        ///</returns>
        ///
        ///<param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information. </param>
        ///<param name="conversionType">The <see cref="T:System.Type"></see> to which the value of this instance is converted. </param><filterpriority>2</filterpriority>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        #endregion

        #region IXmlSerializable Members
        /// <summary>
        /// Returns the null to prevent this method from creating a schema
        /// </summary>
        /// <returns>XML Schema object.</returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads the xml data into the value of this Code.
        /// </summary>
        /// <param name="reader">Xml Reader</param>
        public void ReadXml(XmlReader reader)
        {
            Value = reader.ReadString();
            //ReadXml must read past the EndElement
            reader.Read();
        }

        /// <summary>
        /// Sets the Value property from the Xml Writer
        /// </summary>
        /// <param name="writer">Xml Writer</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(Value);
        }
        #endregion

        /// <summary>
        /// Returns the qualified name for Code as just a string.
        /// </summary>
        public static XmlQualifiedName QualifiedName(XmlSchemaSet xs)
        {
            return new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
        }

        /// <summary>
        /// <see cref="object.Equals(object)"/>
        /// </summary>
        public override bool Equals(object obj)
        {
            //if (this == obj)
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            Code code = obj as Code;
            if (code == null)
            {
                return false;
            }
            return Equals(_Value, code._Value);
        }

        /// <summary>
        /// <see cref="object.GetHashCode"/>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _Value != null ? _Value.GetHashCode() : 0;
        }

        /// <summary>
        /// Returns the string value of the code.
        /// </summary>
        /// <returns>Value property.</returns>
        public override string ToString()
        {
            return Value;
        }

        ///<summary>
        /// Parse a string into a Code object
        ///</summary>
        public static Code Parse(string value)
        {
            return new Code(value);
        }

        /// <summary>
        /// Implicitly coverts the code to a string.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static implicit operator string(Code c)
        {
            return c == null ? null : c.Value;
        }

        /// <summary>
        /// Implicitly converts a string to a code.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static implicit operator Code(string c)
        {
            return c == null ? null : new Code(c);
        }

        /// <summary>
        /// overload == operator
        /// </summary>
        public static bool operator ==(Code lhs, Code rhs)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }
            // If one is null, but not both, return false.
            if (((object)lhs == null) || ((object)rhs == null))
            {
                return false;
            }

            // Return true if the fields match:
            return lhs.Value == rhs.Value;
        }

        /// <summary>
        /// overload != operator
        /// </summary>	    
        public static bool operator !=(Code lhs, Code rhs)
        {
            return !(lhs == rhs);
        }
    }
}
