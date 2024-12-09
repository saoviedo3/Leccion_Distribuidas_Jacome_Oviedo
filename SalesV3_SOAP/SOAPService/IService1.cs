using System.ServiceModel;
using System.Collections.Generic;
using Entities;

namespace SOAPService
{
    [ServiceContract]
    public interface IService1
    {
        // Métodos para Productos
        [OperationContract]
        bool CreateProduct(int id, string name, int categoryId, decimal unitPrice, int unitsInStock);

        [OperationContract]
        bool DeleteProduct(int id);

        [OperationContract]
        Products[] GetAllProducts();

        [OperationContract]
        Products GetProductById(int id);

        [OperationContract]
        bool UpdateProduct(int id, string name, int categoryId, decimal unitPrice, int unitsInStock);

        // Métodos para Categorías
        [OperationContract]
        bool CreateCategory(int id, string name, string description);

        [OperationContract]
        bool DeleteCategory(int id);

        [OperationContract]
        Categories[] GetAllCategories();

        [OperationContract]
        Categories GetCategoryById(int id);

        [OperationContract]
        bool UpdateCategory(int id, string name, string description);
    }
}
