using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;

namespace BLL
{
    public class ProductsLogic
    {
        public Products Create (Products products){
            Products _products = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                Products _result = repository.Retrieve<Products>
                    (p => p.ProductName == products.ProductName);

                if (_result == null)
                {
                    _products = repository.Create(products);
                }
                else
                {
                    throw new Exception("Producto ya existe");
                }
            }
            return products;
        }
        
        public Products RetrieveById(int id){
            Products _products = null;
            using (var repository = RepositoryFactory.CreateRepository()){
                _products = repository.Retrieve<Products>(p => p.ProductID == id);
            }
            return _products;
        }
        public bool Update(Products products){
            bool _updated = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                Products _result = repository.Retrieve<Products>(p => p.ProductName == products.ProductName && p.ProductID != products.ProductID);
                if (_result == null)
                {
                    _updated = repository.Update(products);
                }
                else
                {
                    throw new Exception("Ya existe otro producto con el mismo nombre.");
                }

            }
            return _updated;
        }
        public bool Delete(int id)
        {
            bool _delete = false;

            // Recuperar el producto por ID
            var _product = RetrieveById(id);

            if (_product != null)
            {
                // Verificar si el producto tiene unidades en stock
                if (_product.UnitsInStock == 0)
                {
                    using (var repository = RepositoryFactory.CreateRepository())
                    {
                        // Intentar eliminar el producto
                        _delete = repository.Delete(_product);
                    }
                }
                else
                {
                    throw new Exception("No se puede eliminar un producto con unidades en stock.");
                }
            }
            else
            {
                throw new Exception("El producto no existe.");
            }

            return _delete;
        }

        public List<object> RetrieveAll()
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Usar una expresión lambda
                var products = repository.Filter<Products>(p => p.ProductID > 0)
                    .Select(p => new
                    {
                        p.ProductID,
                        p.ProductName,
                        p.CategoryID,
                        p.UnitPrice,
                        p.UnitsInStock
                    }).ToList();

                return products.Cast<object>().ToList(); // Devuelve un listado genérico compatible con JSON
            }
        }
    }
}
