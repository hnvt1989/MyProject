using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;
using WebGrease.Css.Extensions;

namespace MyProject.Controllers
{
    public class ProductApiController : ApiController
    {
        //[Route("customers/{customerId}/orders")]
        [Route("api/GetAllProducts")]
        public IEnumerable<ProductViewModel> GetAllProducts()
        {
            using (var context = new ShoppingCartContext())
            {
                var ret = new List<ProductViewModel>();

                context.Products.ForEach(p => ret.Add(new ProductViewModel()
                {
                    Active = p.Active,
                    BoughtQuantity = p.QuantityOnHand,
                    BuyInPrice = p.BuyInPrice,
                    Code = p.Code,
                    Description = p.Description,
                    DetailDescription = p.DetailDescription,
                    Id = p.Id,
                    
                }));
                return ret;
            }
        }

        [Route("api/GetProduct/{id}")]
        public IHttpActionResult GetProduct(int id)
        {
            using (var context = new ShoppingCartContext())
            {
                var p = context.Products.SingleOrDefault(prod => prod.Id == id);
                if(p == null)
                    return NotFound();
                return Ok(new ProductViewModel()
                {
                    Active = p.Active,
                    BoughtQuantity = p.QuantityOnHand,
                    BuyInPrice = p.BuyInPrice,
                    Code = p.Code,
                    Description = p.Description,
                    DetailDescription = p.DetailDescription,
                    Id = p.Id,
                });
            }
        }
    }
}
