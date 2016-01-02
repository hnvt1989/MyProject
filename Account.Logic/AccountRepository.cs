

namespace Account.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using Account = Account;
    using Transactional = Transactional.Transactional;
    using System;

    public class AccountRepository : Transactional
    {

        //public void GetAll()
        //{
        //    using (var ctx = CreateContext(AuditMode.PROFILE))
        //    {
        //        //VECTORIZE:
        //        //TODO: vectorize ExistsCore and overrides
        //        var result = await ExistsCore(ctx, key);
        //        retVal.Add(new Tuple<TComponentKey, bool>(key, result));
        //    }
        //}

        //public void CreateContext()
        //{
        //    DataContext db = new DataContext(@"c:\Northwnd.mdf");

        //    var context = new Transactional(_Identity, _SqlSettings, auditMode)
        //    {
        //        CommandTimeout = SettingsManager.Instance.AppSettings.Core_DefaultCommandTimeout
        //    };
        //}

        public List<Account> GetAll()
        {
            using (var db = CreateContext())
            {
                var accounts = db.Accounts.ToList();

                var ret = new List<Account>();
                foreach (var a in accounts)
                {
                    ret.Add(Clone(a));
                }
                return ret;
            }

        }

        public void Update(IEnumerable<Account> components)
        {
            using (var db = CreateContext())
            {
                var accounts = db.Accounts.Where(a => components.Single(c => c.Code == a.Code) != null);

                foreach (var a in accounts)
                {
                    var c = components.Single(co => co.Code == a.Code);

                    a.FamilyName = a.FamilyName;
                    a.GivenName = c.GivenName;
                    a.AccountType = c.AccountType;
                }
                db.SubmitChanges();
            }
        }

        public void Create(Account account)
        {
            using (var db = CreateContext())
            {
                db.Accounts.InsertOnSubmit(Clone(account));
                db.SubmitChanges();
            }

        }

        public void DeleteAll(IEnumerable<Account> components)
        {
            using (var db = CreateContext())
            {
                var accounts = db.Accounts.Where(a => components.Single(c => c.Code == a.Code) != null);

                db.Accounts.DeleteAllOnSubmit(accounts);
                db.SubmitChanges();
            }
        }


        public global::Transactional.Account Clone(Account account)
        {
            return new global::Transactional.Account()
            {
                AccountType = account.AccountType,
                Name = account.Name,
                FamilyName = account.FamilyName,
                GivenName = account.GivenName,
                Email = account.Email,
                Code = Guid.NewGuid().ToString().Substring(0, 5),

            };
        }

        public Account Clone(global::Transactional.Account account)
        {
            return new Account()
            {
                AccountType = account.AccountType,
                Name = account.Name,
                FamilyName = account.FamilyName,
                GivenName = account.GivenName,
                Email = account.Email,
                Code = account.Code
            };
        }
    }
}
