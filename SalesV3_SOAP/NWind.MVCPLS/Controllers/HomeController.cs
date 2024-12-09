using System;
using System.Web.Mvc;
using NWindProxyService;
using Entities;
using SLC;
using System.Threading.Tasks;

namespace NWind.MVCPLS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(int id)
        {
            var proxy = new Proxy();
            var products = await proxy.FilterProductsByCategoryIDAsync(id);
            return View("ProductList", products);
        }



        [HttpGet]
        public ActionResult Details(int id)
        {
            var proxy = new Proxy() as IProductService; // Usamos la interfaz IProductService
            var product = proxy.GetById(id); // Llamamos al método definido en la interfaz
            return View(product);
        }

        public ActionResult CUD(int id = 0)
        {
            var Proxy = new Proxy();
            var Model = new Products();
            if (id != 0)
            {
                Model = Proxy.RetrieveProductByID(id);
            }
            return View(Model);
        }

        [HttpPost]
        public async Task<ActionResult> CUDAsync(Products newProduct,string CreateBtn, string UpdateBtn, string DeleteBtn)
        {
            Products Product;
            var Proxy = new Proxy();
            ActionResult Result = View();
            if (CreateBtn != null) // ¿Crear un producto?
            {
                Product = Proxy.CreateProduct(newProduct);
                if (Product != null)
                {
                    Result = RedirectToAction("CUD", new { id = Product.ProductID});
                }
            }
            else if (UpdateBtn != null) // ¿Modificar un producto?
            {
                var IsUpdate = Proxy.UpdateProduct(newProduct);
                if (IsUpdate)
                {
                    Result = Content("El producto se ha actualizado");
                }
            }
            else if (DeleteBtn != null) // ¿Eliminar un producto?
            {
                IProductService productService = new Proxy(); // Usa Proxy como IProductService
                var DeletedProduct = productService.Delete(newProduct.ProductID); // Llama al método Delete
                if (DeletedProduct)
                {
                    Result = Content("El producto se ha eliminado");
                }
            }

            return Result;
        }



    }
}
