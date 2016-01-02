﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Transactional
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="CuztomizeIt")]
	public partial class TransactionalDataDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertAccount(Account instance);
    partial void UpdateAccount(Account instance);
    partial void DeleteAccount(Account instance);
    partial void InsertAccountType(AccountType instance);
    partial void UpdateAccountType(AccountType instance);
    partial void DeleteAccountType(AccountType instance);
    partial void InsertUser(User instance);
    partial void UpdateUser(User instance);
    partial void DeleteUser(User instance);
    #endregion
		
		public TransactionalDataDataContext() : 
				base(global::Transactional.Properties.Settings.Default.CuztomizeItConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public TransactionalDataDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TransactionalDataDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TransactionalDataDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TransactionalDataDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Account> Accounts
		{
			get
			{
				return this.GetTable<Account>();
			}
		}
		
		public System.Data.Linq.Table<AccountType> AccountTypes
		{
			get
			{
				return this.GetTable<AccountType>();
			}
		}
		
		public System.Data.Linq.Table<User> Users
		{
			get
			{
				return this.GetTable<User>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Account")]
	public partial class Account : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Code;
		
		private string _FamilyName;
		
		private string _GivenName;
		
		private string _Name;
		
		private string _Email;
		
		private int _AccountType;
		
		private EntityRef<User> _User;
		
		private EntityRef<AccountType> _AccountType1;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnCodeChanging(string value);
    partial void OnCodeChanged();
    partial void OnFamilyNameChanging(string value);
    partial void OnFamilyNameChanged();
    partial void OnGivenNameChanging(string value);
    partial void OnGivenNameChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnEmailChanging(string value);
    partial void OnEmailChanged();
    partial void OnAccountTypeChanging(int value);
    partial void OnAccountTypeChanged();
    #endregion
		
		public Account()
		{
			this._User = default(EntityRef<User>);
			this._AccountType1 = default(EntityRef<AccountType>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
            set
            {
                if ((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Code", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Code
		{
			get
			{
				return this._Code;
			}
			set
			{
				if ((this._Code != value))
				{
					this.OnCodeChanging(value);
					this.SendPropertyChanging();
					this._Code = value;
					this.SendPropertyChanged("Code");
					this.OnCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FamilyName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string FamilyName
		{
			get
			{
				return this._FamilyName;
			}
			set
			{
				if ((this._FamilyName != value))
				{
					this.OnFamilyNameChanging(value);
					this.SendPropertyChanging();
					this._FamilyName = value;
					this.SendPropertyChanged("FamilyName");
					this.OnFamilyNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GivenName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string GivenName
		{
			get
			{
				return this._GivenName;
			}
			set
			{
				if ((this._GivenName != value))
				{
					this.OnGivenNameChanging(value);
					this.SendPropertyChanging();
					this._GivenName = value;
					this.SendPropertyChanged("GivenName");
					this.OnGivenNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Email", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				if ((this._Email != value))
				{
					this.OnEmailChanging(value);
					this.SendPropertyChanging();
					this._Email = value;
					this.SendPropertyChanged("Email");
					this.OnEmailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AccountType", DbType="Int NOT NULL")]
		public int AccountType
		{
			get
			{
				return this._AccountType;
			}
			set
			{
				if ((this._AccountType != value))
				{
					if (this._AccountType1.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnAccountTypeChanging(value);
					this.SendPropertyChanging();
					this._AccountType = value;
					this.SendPropertyChanged("AccountType");
					this.OnAccountTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Account_User", Storage="_User", ThisKey="Id", OtherKey="Id", IsUnique=true, IsForeignKey=false)]
		public User User
		{
			get
			{
				return this._User.Entity;
			}
            set
            {
                User previousValue = this._User.Entity;
                if (((previousValue != value)
                            || (this._User.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._User.Entity = null;
                        previousValue.Account1 = null;
                    }
                    this._User.Entity = value;
                    if ((value != null))
                    {
                        value.Account1 = this;
                    }
                    this.SendPropertyChanged("User");
                }
            }
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="AccountType_Account", Storage="_AccountType1", ThisKey="AccountType", OtherKey="Id", IsForeignKey=true)]
		public AccountType AccountType1
		{
			get
			{
				return this._AccountType1.Entity;
			}
            set
            {
                AccountType previousValue = this._AccountType1.Entity;
                if (((previousValue != value)
                            || (this._AccountType1.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._AccountType1.Entity = null;
                        previousValue.Accounts.Remove(this);
                    }
                    this._AccountType1.Entity = value;
                    if ((value != null))
                    {
                        value.Accounts.Add(this);
                        this._AccountType = value.Id;
                    }
                    else
                    {
                        this._AccountType = default(int);
                    }
                    this.SendPropertyChanged("AccountType1");
                }
            }
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.AccountType")]
	public partial class AccountType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Code;
		
		private string _Description;
		
		private EntitySet<Account> _Accounts;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnCodeChanging(string value);
    partial void OnCodeChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    #endregion
		
		public AccountType()
		{
			this._Accounts = new EntitySet<Account>(new Action<Account>(this.attach_Accounts), new Action<Account>(this.detach_Accounts));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
            set
            {
                if ((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Code", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Code
		{
			get
			{
				return this._Code;
			}
			set
			{
				if ((this._Code != value))
				{
					this.OnCodeChanging(value);
					this.SendPropertyChanging();
					this._Code = value;
					this.SendPropertyChanged("Code");
					this.OnCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(50)")]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="AccountType_Account", Storage="_Accounts", ThisKey="Id", OtherKey="AccountType")]
		public EntitySet<Account> Accounts
		{
			get
			{
				return this._Accounts;
			}
			set
			{
				this._Accounts.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Accounts(Account entity)
		{
			this.SendPropertyChanging();
			entity.AccountType1 = this;
		}
		
		private void detach_Accounts(Account entity)
		{
			this.SendPropertyChanging();
			entity.AccountType1 = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.[User]")]
	public partial class User : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Code;
		
		private int _Account;
		
		private string _Password;
		
		private string _PasswordEncoding;
		
		private string _Hint;
		
		private System.DateTime _JoinDate;
		
		private bool _Active;
		
		private bool _MustChangePassword;
		
		private int _Culture;
		
		private EntityRef<Account> _Account1;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnCodeChanging(string value);
    partial void OnCodeChanged();
    partial void OnAccountChanging(int value);
    partial void OnAccountChanged();
    partial void OnPasswordChanging(string value);
    partial void OnPasswordChanged();
    partial void OnPasswordEncodingChanging(string value);
    partial void OnPasswordEncodingChanged();
    partial void OnHintChanging(string value);
    partial void OnHintChanged();
    partial void OnJoinDateChanging(System.DateTime value);
    partial void OnJoinDateChanged();
    partial void OnActiveChanging(bool value);
    partial void OnActiveChanged();
    partial void OnMustChangePasswordChanging(bool value);
    partial void OnMustChangePasswordChanged();
    partial void OnCultureChanging(int value);
    partial void OnCultureChanged();
    #endregion
		
		public User()
		{
			this._Account1 = default(EntityRef<Account>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					if (this._Account1.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Code", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Code
		{
			get
			{
				return this._Code;
			}
			set
			{
				if ((this._Code != value))
				{
					this.OnCodeChanging(value);
					this.SendPropertyChanging();
					this._Code = value;
					this.SendPropertyChanged("Code");
					this.OnCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Account", DbType="Int NOT NULL")]
		public int Account
		{
			get
			{
				return this._Account;
			}
			set
			{
				if ((this._Account != value))
				{
					this.OnAccountChanging(value);
					this.SendPropertyChanging();
					this._Account = value;
					this.SendPropertyChanged("Account");
					this.OnAccountChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Password", DbType="NVarChar(512) NOT NULL", CanBeNull=false)]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				if ((this._Password != value))
				{
					this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PasswordEncoding", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string PasswordEncoding
		{
			get
			{
				return this._PasswordEncoding;
			}
			set
			{
				if ((this._PasswordEncoding != value))
				{
					this.OnPasswordEncodingChanging(value);
					this.SendPropertyChanging();
					this._PasswordEncoding = value;
					this.SendPropertyChanged("PasswordEncoding");
					this.OnPasswordEncodingChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Hint", DbType="NVarChar(50)")]
		public string Hint
		{
			get
			{
				return this._Hint;
			}
			set
			{
				if ((this._Hint != value))
				{
					this.OnHintChanging(value);
					this.SendPropertyChanging();
					this._Hint = value;
					this.SendPropertyChanged("Hint");
					this.OnHintChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_JoinDate", DbType="DateTime NOT NULL")]
		public System.DateTime JoinDate
		{
			get
			{
				return this._JoinDate;
			}
			set
			{
				if ((this._JoinDate != value))
				{
					this.OnJoinDateChanging(value);
					this.SendPropertyChanging();
					this._JoinDate = value;
					this.SendPropertyChanged("JoinDate");
					this.OnJoinDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Active", DbType="Bit NOT NULL")]
		public bool Active
		{
			get
			{
				return this._Active;
			}
			set
			{
				if ((this._Active != value))
				{
					this.OnActiveChanging(value);
					this.SendPropertyChanging();
					this._Active = value;
					this.SendPropertyChanged("Active");
					this.OnActiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MustChangePassword", DbType="Bit NOT NULL")]
		public bool MustChangePassword
		{
			get
			{
				return this._MustChangePassword;
			}
			set
			{
				if ((this._MustChangePassword != value))
				{
					this.OnMustChangePasswordChanging(value);
					this.SendPropertyChanging();
					this._MustChangePassword = value;
					this.SendPropertyChanged("MustChangePassword");
					this.OnMustChangePasswordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Culture", DbType="Int NOT NULL")]
		public int Culture
		{
			get
			{
				return this._Culture;
			}
			set
			{
				if ((this._Culture != value))
				{
					this.OnCultureChanging(value);
					this.SendPropertyChanging();
					this._Culture = value;
					this.SendPropertyChanged("Culture");
					this.OnCultureChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Account_User", Storage="_Account1", ThisKey="Id", OtherKey="Id", IsForeignKey=true)]
		public Account Account1
		{
			get
			{
				return this._Account1.Entity;
			}
			set
			{
				Account previousValue = this._Account1.Entity;
				if (((previousValue != value) 
							|| (this._Account1.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Account1.Entity = null;
						previousValue.User = null;
					}
					this._Account1.Entity = value;
					if ((value != null))
					{
						value.User = this;
						this._Id = value.Id;
					}
					else
					{
						this._Id = default(int);
					}
					this.SendPropertyChanged("Account1");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
