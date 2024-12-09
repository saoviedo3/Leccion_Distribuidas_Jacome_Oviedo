using System.Collections.Generic;
using System.Linq;
using Entities;
using BLL;
using System.ServiceModel.Activation;


namespace SOAPService
{
    public class Service1 : IService1
    {
        private readonly ProductsLogic _productsLogic = new ProductsLogic();
        private readonly CategoriesLogic _categoriesLogic = new CategoriesLogic();

        // Métodos para Productos
        public Products[] GetAllProducts()
        {
            var products = _productsLogic.RetrieveAll()
                .Select(obj => new Products
                {
                    ProductID = (int)obj.GetType().GetProperty("ProductID").GetValue(obj),
                    ProductName = (string)obj.GetType().GetProperty("ProductName").GetValue(obj),
                    CategoryID = (int)obj.GetType().GetProperty("CategoryID").GetValue(obj),
                    UnitPrice = (decimal)obj.GetType().GetProperty("UnitPrice").GetValue(obj),
                    UnitsInStock = (int)obj.GetType().GetProperty("UnitsInStock").GetValue(obj)
                }).ToArray();

            return products;
        }

        public Products GetProductById(int id)
        {
            var product = _productsLogic.RetrieveById(id);

            if (product != null)
            {
                // Devuelve solo los datos simples para evitar problemas de serialización
                return new Products
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    CategoryID = product.CategoryID,
                    UnitPrice = product.UnitPrice,
                    UnitsInStock = product.UnitsInStock,
                    Categories = null // Elimina las referencias complejas
                };
            }

            return null;
        }

        public bool CreateProduct(int id, string name, int categoryId, decimal unitPrice, int unitsInStock)
        {
            var product = new Products
            {
                ProductID = id,
                ProductName = name,
                CategoryID = categoryId,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock
            };
            return _productsLogic.Create(product) != null;
        }

        public bool UpdateProduct(int id, string name, int categoryId, decimal unitPrice, int unitsInStock)
        {
            var product = new Products
            {
                ProductID = id,
                ProductName = name,
                CategoryID = categoryId,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock
            };
            return _productsLogic.Update(product);
        }

        public bool DeleteProduct(int id)
        {
            return _productsLogic.Delete(id);
        }

        // Métodos para Categorías
        public Categories[] GetAllCategories()
        {
            var categories = _categoriesLogic.RetrieveAll()
                .Select(obj => new Categories
                {
                    CategoryID = (int)obj.GetType().GetProperty("CategoryID").GetValue(obj),
                    CategoryName = (string)obj.GetType().GetProperty("CategoryName").GetValue(obj),
                    Description = (string)obj.GetType().GetProperty("Description").GetValue(obj),
                    Products = null // Elimina las referencias complejas
                }).ToArray();

            return categories;
        }

        public Categories GetCategoryById(int id)
        {
            var category = _categoriesLogic.RetrieveById(id);

            if (category != null)
            {
                // Devuelve solo los datos simples para evitar problemas de serialización
                return new Categories
                {
                    CategoryID = category.CategoryID,
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                    Products = null // Elimina las referencias complejas
                };
            }

            return null;
        }

        public bool CreateCategory(int id, string name, string description)
        {
            var category = new Categories
            {
                CategoryID = id,
                CategoryName = name,
                Description = description
            };
            return _categoriesLogic.Create(category) != null;
        }

        public bool UpdateCategory(int id, string name, string description)
        {
            var category = new Categories
            {
                CategoryID = id,
                CategoryName = name,
                Description = description
            };
            return _categoriesLogic.Update(category);
        }

        public bool DeleteCategory(int id)
        {
            return _categoriesLogic.Delete(id);
        }
    }
}

