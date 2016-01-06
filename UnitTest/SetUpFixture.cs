using System.Data.Entity;
using NUnit.Framework;
using MyProject;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;

namespace UnitTest
{
    public class SetUpFixture
    {

        [SetUp]
        public void SetUp()
        {
            //initialize data
            Database.SetInitializer(new DatabaseMasterInitializerForTest());
            var context = new DatabaseMasterContext();
            context.Database.Initialize(true);
        }

        [TearDown]
        public void TearDown()
        {
            var type = new Product();
        }
    }
}
