using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DevStore.Domain;
using DevStore.Infra.DataContexts;

namespace DevStore.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class ProductController : ApiController
    {
        private DevStoreDataContext db = new DevStoreDataContext();


        [Route("products")]
        public HttpResponseMessage GetProducts()
        {
            var result = db.Products.Include("Category").ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("categories")]
        public HttpResponseMessage GetCategories()
        {
            var result = db.Products.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("categories/{categoryId}/products")]
        public HttpResponseMessage GetProductsByCategories(int categoryId)
        {
            var result = db.Products.Include("Category").Where(x => x.CategoryId == categoryId).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [HttpPost]
        [Route("products")]
        public HttpResponseMessage postProduct(Product product)
        {
            if (product == null)
            {
                return Request.CreateResponse((HttpStatusCode.BadRequest));
            }

            try
            {
                db.Products.Add(product);
                db.SaveChanges();
                var result = product;
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                return Request.CreateResponse((HttpStatusCode.InternalServerError, "Falha ao incluir produto"));
                throw;
            }

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

      
    }
}