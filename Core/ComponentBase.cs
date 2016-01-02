using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using Core.Attribute;

namespace Core
{
    [Serializable]
    public abstract class ComponentBase
    {
        /// <summary>
        /// An identifier for this component, to be used in error messages.
        /// </summary>
        [XmlIgnore]
        [ScaffoldColumn(false)]
        public virtual string Identifier
        {
            get { return ToString(); }
        }

        /// <summary>
        /// Published event when a component's identification has changed
        /// </summary>
        public event Action<string> IdentifierChanged;

        /// <summary>
        /// Fires the IdentifierChanged event
        /// </summary>
        protected void FireIdentifierChanged(string oldIdentifier)
        {
            if (IdentifierChanged != null)
                IdentifierChanged(oldIdentifier);
        }
    }

    /// <summary>
    /// Things that have a uniquely identifying key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IKey<T>
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        T Key { get; set; }
    }

    /// <summary>
    /// Things that have an identifying Code.
    /// </summary>
    public interface ICode
    {
        /// <summary>
        /// An identifier that is unique for all objects in a particular collection.
        /// </summary>
        string Code { get; set; }
    }

    /// <summary>
    /// Things that have an Active attribute
    /// </summary>
    public interface IActive
    {
        /// <summary>
        /// Active flag
        /// </summary>
        bool Active { get; set; }
    }

    /// <summary>
    /// Things that have and identifying System
    /// </summary>
    public interface ISystem
    {
        /// <summary>
        /// Active flag
        /// </summary>
        bool System { get; set; }
    }

    /// <summary>
    /// Things that have an internal ID.
    /// </summary>
    public interface IId
    {
        /// <summary>
        /// The internal ID for the object.
        /// </summary>
        [XmlIgnore]
        int Id { get; }

    }

    /// <summary>
    /// equality comparer for a component
    /// </summary>
    public class ComponentBaseEqualityComparer : IEqualityComparer<ComponentBase>
    {
        private static ComponentBaseEqualityComparer _Instance;
        /// <summary>
        /// Instance of this equality comparer
        /// </summary>
        public static ComponentBaseEqualityComparer Instance
        {
            get { return _Instance ?? (_Instance = new ComponentBaseEqualityComparer()); }
        }

        private ComponentBaseEqualityComparer()
        {

        }

        public bool Equals(ComponentBase x, ComponentBase y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(null, x))
                return false;

            if (x.GetType() != y.GetType())
                return false;

            var xCodeComponent = x as CodeComponent;
            if (!ReferenceEquals(null, xCodeComponent))
            {
                var yCodeComponent = y as CodeComponent;

                if (ReferenceEquals(null, yCodeComponent))
                    return false;

                if (ReferenceEquals(null, xCodeComponent.Key) || ReferenceEquals(null, yCodeComponent.Key))
                    return false;

                return ReferenceEquals(xCodeComponent.Key, yCodeComponent.Key) || Equals(xCodeComponent.Key, yCodeComponent.Key);
            }

            var xIdComponent = x as IdComponent;
            if (!ReferenceEquals(null, xIdComponent))
            {
                var yIdComponent = y as IdComponent;

                if (ReferenceEquals(null, yIdComponent))
                    return false;

                if (default(int).Equals(xIdComponent.Id) || default(int).Equals(yIdComponent.Id))
                    return false;

                return xIdComponent.Id.Equals(yIdComponent.Id);
            }

            return x.Equals(y);
        }

        public int GetHashCode(ComponentBase obj)
        {
            return obj == null
            ? default(int)
            : obj.GetHashCode();
        }
    }

    /// <summary>
    /// Base class that provides 
    /// </summary>
    [Serializable]
    public abstract class IdComponent : ComponentBase, IId, ICloneable
    {
        #region IId Members

        /// <summary>
        /// Internal identifier
        /// </summary>
        [XmlIgnore]
        [ScriptIgnore]
        [ScaffoldColumn(false)]
        public virtual int Id { get; set; }

        #endregion

        /// <see cref="ICloneable.Clone"/>
        public virtual object Clone()
        {
            var copy = (IdComponent)MemberwiseClone();
            return copy;
        }
    }

    /// <summary>
    /// Base class that provides some validation for things that have a Code attribute.
    /// </summary>
    [Serializable]
    public abstract class CodeComponent : IdComponent, ICode, IKey<string>, ICloneable
    {
        private string _Key;

        /// <summary>
        /// Public constructor
        /// </summary>
        protected CodeComponent(string code)
            : this()
        {
            _Key = code;
        }

        /// <summary>
        /// Public constructor
        /// </summary>
        protected CodeComponent()
        {
        }

        /// <summary>
        /// Overridden to return Code.
        /// </summary>
        [ScaffoldColumn(false)]
        public override string Identifier
        {
            get { return GetType().Name + ":" + Code; }
        }

        #region ICode Members
        /// <summary>
        /// Unique, human-readable identifier.
        /// </summary>
        [Primary]
        [DataType("Code")]
        [Code]
        [Display(Order = 1)]
        //[XmlAttribute]
        public virtual string Code
        {
            get { return Key; }
            set { Key = value; }
        }
        #endregion

        /// <summary>
        /// The Key (Code) for this CodeComponent
        /// </summary>
        [XmlIgnore]
        [ScaffoldColumn(false)]
        public string Key
        {
            get { return _Key; }
            set
            {
                var oldKey = _Key;
                var oldIdentifier = Identifier;
                _Key = value;
                if (string.IsNullOrEmpty(oldKey))
                    FireIdentifierChanged(oldIdentifier);
            }
        }

        /// <summary>
        /// Compare to another codecomponent.
        /// </summary>
        public bool Equals(CodeComponent other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(null, other._Key))
                return false;

            if (ReferenceEquals(_Key, other._Key))
                return true;

            return Equals(other._Key, _Key);
        }

        /// <see cref="Object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (!(obj is CodeComponent))
            {
                return false;
            }
            return Equals((CodeComponent)obj);
        }

        /// <see cref="Object.GetHashCode"/>
        public override int GetHashCode()
        {
            return (_Key != null ? _Key.GetHashCode() : 0);
        }

    }

    /// <summary>
    /// base class provides a codecomponent where the description is translated via resources
    /// </summary>	
    //[Serializable]
    //public abstract class ResourcedCodeComponent : CodeComponent
    //{
    //    private ContentProperty _Description;

    //    /// <summary>
    //    /// Public constructor
    //    /// </summary>
    //    protected ResourcedCodeComponent(string code)
    //        : base(code)
    //    {
    //        _Description = new ContentProperty(this, "Description");
    //    }

    //    /// <summary>
    //    /// Public constructor
    //    /// </summary>
    //    protected ResourcedCodeComponent()
    //    {
    //        _Description = new ContentProperty(this, "Description");
    //    }

    //    /// <summary>
    //    /// A short description for the object
    //    /// </summary>
    //    [Content(MaxLength = 200)]
    //    [Display(Order = 2)]
    //    //[XmlAttribute]
    //    public virtual string Description
    //    {
    //        get { return _Description.Get(); }
    //        set { _Description.Set(value); }
    //    }

    //    /// <see cref="ICloneable.Clone"/>
    //    public override object Clone()
    //    {
    //        var copy = (ResourcedCodeComponent)base.Clone();
    //        copy._Description = new ContentProperty(copy, _Description);
    //        return copy;
    //    }
    //}

    /// <summary>
    /// base class provide active flag
    /// </summary>
    [Serializable]
    public abstract class ActiveComponent : IActive
    {
        /// <summary>
        /// Active flag.
        /// </summary>
        [Display(Order = 3)]
        [XmlAttribute]
        public bool Active { get; set; }
    }


    /// <summary>
    /// base class provide system flag
    /// </summary>
    //[Serializable]
    //public abstract class SystemComponent : ActiveComponent, ISystem
    //{
    //    /// <summary>
    //    /// Code Constructor
    //    /// </summary>
    //    protected SystemComponent(Code code)
    //        : base(code)
    //    {
    //    }

    //    /// <summary>
    //    /// Default constructor
    //    /// </summary>
    //    protected SystemComponent()
    //    {
    //    }

    //    /// <summary>
    //    /// System flag, when true, does not allow delete
    //    /// </summary>
    //    [XmlAttribute]
    //    public bool System { get; set; }
    //}
    /// <summary>
    /// base class provides a codecomponent where the description and a message is translated via resources
    /// </summary>	
    //[Serializable]
    //public abstract class ResourcedCodeMessageComponent : ResourcedCodeComponent
    //{
    //    private ContentProperty _Message;

    //    /// <summary>
    //    /// .ctor()
    //    /// </summary>
    //    protected ResourcedCodeMessageComponent()
    //    {
    //        _Message = new ContentProperty(this, "Message");
    //    }

    //    /// <summary>
    //    /// Name
    //    /// </summary>
    //    [Required]
    //    [MaxLength(255)]
    //    public string Message
    //    {
    //        get { return _Message.Get(); }
    //        set { _Message.Set(value); }
    //    }

    //    /// <see cref="ICloneable.Clone"/>
    //    public override object Clone()
    //    {
    //        var copy = (ResourcedCodeMessageComponent)base.Clone();
    //        copy._Message = new ContentProperty(copy, _Message);
    //        return copy;
    //    }
    //}
}
