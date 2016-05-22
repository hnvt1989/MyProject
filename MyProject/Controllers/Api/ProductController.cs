using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyProject.DAL;
using MyProject.Models.ViewModels;

namespace MyProject.Controllers.Api
{
    //[Route("api/[controller]")]
    public class ProductController : ApiController
    {
        [HttpGet]
        public IEnumerable<ProductViewModel> Get()
        {
            var products = new List<ProductViewModel>();
            using (var context = new ShoppingCartContext())
            {

                foreach (var prod in context.Products.Take(20))
                {
                    products.Add(new ProductViewModel()
                    {
                        Code = prod.Code,
                        Description = prod.Description,
                        Image = prod.Image,
                        Id = prod.Id,
                        Active = prod.Active,
                        QuantityOnHand = prod.QuantityOnHand
                    });
                }
            }
            return products;
        }

        // GET api/<controller>/5
        public ProductViewModel Get(int id)
        {
            var ret = new ProductViewModel();
            using (var context = new ShoppingCartContext())
            {
                var product = context.Products.SingleOrDefault(p => p.Code == id.ToString());
                if(product != null)
                {
                    ret.Code = product.Code;
                    ret.Description = product.Description;
                    ret.Image = product.Image;
                    ret.Id = product.Id;
                    ret.Active = product.Active;
                    ret.QuantityOnHand = product.QuantityOnHand;
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format("No product with Code = {0}", id)),
                        ReasonPhrase = "Product code Not Found"
                    };
                    throw new HttpResponseException(resp);
                }
            }
            return ret;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}