using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using Transactional = Transactional.Transactional;

    public class UserRepository : Transactional
    {
        public List<User> GetAll()
        {
            using (var db = CreateContext())
            {
                var accounts = db.Users.ToList();

                var ret = new List<User>();
                foreach (var a in accounts)
                {
                    ret.Add(Clone(a));
                }
                return ret;
            }

        }

        public void Update(IEnumerable<User> components)
        {
            using (var db = CreateContext())
            {
                var users = db.Users.Where(a => components.Single(c => c.Code == a.Code) != null);

                foreach (var u in users)
                {
                    var c = components.Single(co => co.Code == u.Code);

                    u.Password = c.Password;
                    u.Hint = c.Hint;
                    u.Culture = c.Culture;
                }
                db.SubmitChanges();
            }
        }

        public void Create(User user)
        {
            using (var db = CreateContext())
            {
                db.Users.InsertOnSubmit(Clone(user));
                db.SubmitChanges();
            }
        }

        public void DeleteAll(IEnumerable<User> components)
        {
            using (var db = CreateContext())
            {
                var users = db.Users.Where(a => components.Single(c => c.Code == a.Code) != null);

                db.Users.DeleteAllOnSubmit(users);
                db.SubmitChanges();
            }
        }


        public global::Transactional.User Clone(User user)
        {
            return new global::Transactional.User()
            {
                Code = user.Code,
                Account = 1,
                Password = user.Password,
                PasswordEncoding = "SHA512",
                Hint = user.Hint,
                JoinDate = user.JoinDate,
                Active = user.Active,
                MustChangePassword = user.MustChangePassword,
                Culture = user.Culture,

            };
        }

        public User Clone(global::Transactional.User user)
        {
            return new User()
            {
                Code = user.Code,
                Account = 1,
                Password = user.Password,
                PasswordEncoding = "SHA512",
                Hint = user.Hint,
                JoinDate = user.JoinDate,
                Active = user.Active,
                MustChangePassword = user.MustChangePassword,
                Culture = user.Culture,

            };
        }
    }

}
