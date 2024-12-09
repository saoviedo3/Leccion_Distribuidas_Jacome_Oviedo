using System;
using System.Collections.Generic;
using DAL;
using Entities;

namespace Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            addCategoryAndProduct();
        }

        static void addCategoryAndProduct()
        {
            var categories = new Categories();
            categories.CategoryName = "Test 1";
            categories.Description = "Description Test 1";

            var products = new Products();
            products.ProductName = "Cereal";
            products.UnitPrice = 15;
            products.UnitsInStock = 200;

            categories.Products.Add(products);

            using (var repository = RepositoryFactory.CreateRepository())
            {
                repository.Create(categories);
            }

            Console.WriteLine($"Category: {categories.CategoryID}, Producto: {products.ProductID}");
            Console.ReadLine();

        }
    }
}