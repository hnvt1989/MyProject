using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using MyProject.DAL;
using NUnit.Framework;

namespace UnitTest.DataUnitTest
{
    class CartProcessingUnitTest
    {
        public ShoppingCartContext CartContext = new ShoppingCartContext();

        [TestFixtureSetUp]
        public void OneTimeSetUp()
        {
        }

        [Test]
        public void Start()
        {
            //Database.SetInitializer(new DatabaseMasterInitializerForTest());
            //var context = new DatabaseMasterContext();
            //context.Database.Initialize(true);
            
            Assert.AreEqual("1001", CartContext.Products.Single(p => p.Code == "1001").Code);
        }
    }
}
