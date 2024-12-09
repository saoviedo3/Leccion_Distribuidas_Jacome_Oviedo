using System;
using System.Web.Http;
using Entities;
using BLL;
using System.Linq;

namespace Service.Controllers
{
    public class CategoriesController : ApiController
    {
        // Crear categoría
        [HttpPost]
        [Route("api/Categories")]
        public IHttpActionResult CreateCategory(Categories categories)
        {
            if (categories == null)
                return BadRequest("La categoría no puede ser nula.");

            var _categoriesLogic = new CategoriesLogic();

            try
            {
                var createdCategory = _categoriesLogic.Create(categories);
                return Created($"api/Categories/{createdCategory.CategoryID}", createdCategory);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear la categoría: {ex.Message}");
            }
        }

        // Eliminar categoría
        [HttpDelete]
        [Route("api/Categories/{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var _categoriesLogic = new CategoriesLogic();
            var result = _categoriesLogic.Delete(id);

            if (result)
                return Ok("Categoría eliminada correctamente.");
            else
                return NotFound();
        }

        //Obtener todas las categorias
        [HttpGet]
        [Route("api/Categories")]
        public IHttpActionResult GetAll()
        {
            var _categoriesLogic = new CategoriesLogic();
            try
            {
                
                var categories = _categoriesLogic.RetrieveAll(); 
                if (categories == null || categories.Count == 0)
                    return NotFound();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Obtener categoría por ID
        [HttpGet]
        [Route("api/Categories/{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var _categoriesLogic = new CategoriesLogic();
            try
            {
                var category = _categoriesLogic.RetrieveById(id);
                if (category == null)
                    return NotFound();

                // Crear un objeto anónimo para devolver solo las propiedades necesarias
                var response = new
                {
                    category.CategoryID,
                    category.CategoryName,
                    category.Description
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Actualizar categoría
        [HttpPut]
        [Route("api/Categories/{id:int}")]
        public IHttpActionResult UpdateCategory(int id, Categories categories)
        {
            if (categories == null)
                return BadRequest("La categoría no puede ser nula.");

            if (id != categories.CategoryID)
                return BadRequest("El ID de la categoría no coincide con el ID de la URL.");

            var _categoriesLogic = new CategoriesLogic();

            try
            {
                var updated = _categoriesLogic.Update(categories);
                if (updated)
                    return Ok("Categoría actualizada correctamente.");
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar la categoría: {ex.Message}");
            }
        }
    }
}
