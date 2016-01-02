using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Transactional
{
    //singleton class
    public class DataContext : TransactionalDataDataContext
    {
        //private static DataContext instance;
        //private DataContext(){}
        //public static DataContext Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new DataContext();
        //        }
        //        return instance;
        //    }
        //}
        public DataContext() : base(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()) { }
    }
}
