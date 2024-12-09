using System.Web.Http;
using Entities;
using BLL;
using System;

namespace Service.Controllers
{
    public class ProductsController : ApiController
    {
        [HttpPost]
        [Route("api/Products")]
        public IHttpActionResult CreateProduct(Products products)
        {
            if (products == null)
                return BadRequest("El producto no puede ser nulo.");

            var _productsLogic = new ProductsLogic();

            try
            {
                var createdProduct = _productsLogic.Create(products);
                return Created($"api/Products/{createdProduct.ProductID}", createdProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el producto: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("api/Products/{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var _productsLogic = new ProductsLogic();

            try
            {
                var result = _productsLogic.Delete(id);

                if (result)
                    return Ok("Producto eliminado correctamente.");
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el producto: {ex.Message}");
            }
        }


        // Obtener todos los productos
        [HttpGet]
        [Route("api/Products")]
        public IHttpActionResult GetAll()
        {
            var _productsLogic = new ProductsLogic();
            try
            {
                // Cambia esta línea para usar RetrieveAll
                var products = _productsLogic.RetrieveAll(); // Llama a RetrieveAll en lugar de Filter
                if (products == null || products.Count == 0)
                    return NotFound();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Obtener producto por ID
        [HttpGet]
        [Route("api/Products/{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var _productsLogic = new ProductsLogic();
            try
            {
                var product = _productsLogic.RetrieveById(id);
                if (product == null)
                    return NotFound();

                // Crear un objeto anónimo para devolver solo las propiedades necesarias
                var response = new
                {
                    product.ProductID,
                    product.ProductName,
                    product.CategoryID,
                    product.UnitPrice,
                    product.UnitsInStock
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Actualizar un producto
        [HttpPut]
        [Route("api/Products/{id:int}")]
        public IHttpActionResult UpdateProduct(int id, Products products)
        {
            if (products == null)
                return BadRequest("El producto no puede ser nulo.");

            if (id != products.ProductID)
                return BadRequest("El ID del producto no coincide con el ID de la URL.");

            var _productsLogic = new ProductsLogic();

            try
            {
                var updated = _productsLogic.Update(products);
                if (updated)
                    return Ok("Producto actualizado correctamente.");
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el producto: {ex.Message}");
            }
        }
    }
}