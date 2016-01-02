using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

namespace Transactional
{
    public abstract class Transactional
    {

        public virtual DataContext CreateContext()
        {
            return new DataContext();
        }

        public virtual void Create(IEnumerable<Account> components)
        {

        }
    }
}
