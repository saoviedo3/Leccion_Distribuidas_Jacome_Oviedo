using SLC;
using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace NWindProxyService
{
    public class Proxy : IProductService, ICategoriesService
    {
        private readonly string BaseAddress = "http://localhost:56104";

        // Método para enviar peticiones POST
        private async Task<T> SendPost<T, PostData>(string requestURI, PostData data)
        {
            T Result = default;
            using (var Client = new HttpClient())
            {
                try
                {
                    requestURI = BaseAddress + requestURI;
                    Client.DefaultRequestHeaders.Accept.Clear();
                    Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var JSONData = JsonConvert.SerializeObject(data);
                    HttpResponseMessage Response = await Client.PostAsync(requestURI, new StringContent(JSONData.ToString(), Encoding.UTF8, "application/json"));


                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<T>(ResultWebAPI);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Error en POST: {ex.Message}", ex);
                }
            }
            return Result;
        }

        // Método para enviar peticiones GET
        public async Task<T> SendGet<T>(string requestURI)
        {
            T Result = default(T);
            using (var Client = new HttpClient())
            {
                try
                {
                    requestURI = BaseAddress + requestURI; // URL Absoluto
                    Client.DefaultRequestHeaders.Accept.Clear();
                    Client.DefaultRequestHeaders.Accept.Add(
                     new MediaTypeWithQualityHeaderValue("application/json"));
                    var ResultJSON = await Client.GetStringAsync(requestURI);
                    Result = JsonConvert.DeserializeObject<T>(ResultJSON);
                }
                catch (Exception ex)
                {
                    // Manejar la excepción
                }
            }
            return Result;
        }

        private async Task<T> SendUpdate<T, UpdateData>(string requestURI, UpdateData data)
        {
            T result = default;
            using (var client = new HttpClient())
            {
                try
                {
                    requestURI = BaseAddress + requestURI; // Construir URL completa
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var jsonData = JsonConvert.SerializeObject(data);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Realiza la solicitud PUT
                    var response = await client.PutAsync(requestURI, content);

                    if (response.IsSuccessStatusCode)
                    {
                        if (typeof(T) == typeof(bool))
                        {
                            // Si se espera un booleano, asume que cualquier respuesta exitosa es "true"
                            result = (T)(object)true;
                        }
                        else
                        {
                            // Si se espera otro tipo, deserializa la respuesta
                            var responseContent = await response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<T>(responseContent);
                        }
                    }
                    else
                    {
                        throw new ApplicationException($"Error en PUT: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Excepción en PUT: {ex.Message}", ex);
                }
            }
            return result;
        }

        private async Task<T> SendDelete<T>(string requestURI)
        {
            T result = default;
            using (var client = new HttpClient())
            {
                try
                {
                    requestURI = BaseAddress + requestURI; // Construir URL completa
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.DeleteAsync(requestURI);

                    if (response.IsSuccessStatusCode)
                    {
                        if (typeof(T) == typeof(bool))
                        {
                            // Si se espera un booleano, asumimos que la respuesta exitosa significa true
                            result = (T)(object)true;
                        }
                        else if (typeof(T) == typeof(string))
                        {
                            // Si se espera un string, retorna el contenido de la respuesta como está
                            var responseContent = await response.Content.ReadAsStringAsync();
                            result = (T)(object)responseContent;
                        }
                        else
                        {
                            // Si es otro tipo, intenta deserializar la respuesta
                            var responseContent = await response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<T>(responseContent);
                        }
                    }
                    else
                    {
                        throw new ApplicationException($"Error en DELETE: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Excepción en DELETE: {ex.Message}", ex);
                }
            }
            return result;
        }




        public async Task<Products> RetrieveProductByIDAsync(int id)
        {
            return await SendGet<Products>($"/api/Products/{id}");
        }
        public Products RetrieveProductByID(int id)
        {
            Products Result = null;
            Task.Run(async () =>
           Result = await RetrieveProductByIDAsync(id)).Wait();
            return Result;
        }


        // PRODUCTOS
        public async Task<Products> CreateProductAsync(Products newProduct)
        {
            return await SendPost<Products, Products>
            ("/api/Products", newProduct);
        }
        public Products CreateProduct(Products newProduct)
        {
            Products Result = null;
            // Ejecutar la tarea en un nuevo hilo
            // para que no se bloquee el hilo síncrono
            // con Wait esperamos la operación asíncrona
            Task.Run(async () => Result =
            await CreateProductAsync(newProduct)).Wait();
            return Result;
        }


        public async Task<Products> RetrieveProductByidAsync(int id)
        {
            return await SendGet<Products>($"/api/Products/{id}");
        }
        Products IProductService.GetById(int id)
        {
            Products Result = null;
            Task.Run(async () =>
           Result = await RetrieveProductByidAsync(id)).Wait();
            return Result;
        }


        public async Task<bool> UpdateProductAsync(Products productToUpdate)
        {
            if (productToUpdate == null || productToUpdate.ProductID <= 0)
                throw new ArgumentException("El producto o el ID del producto no es válido.");

            var requestURI = $"/api/Products/{productToUpdate.ProductID}";
            return await SendUpdate<bool, Products>(requestURI, productToUpdate);
        }


        public bool UpdateProduct(Products productToUpdate)
        {
            bool Result = false;
            Task.Run(async () => Result = await
            UpdateProductAsync(productToUpdate)).Wait();
            return Result;
        }


        public async Task<bool> DeleteProductAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID proporcionado no es válido.");

            var requestURI = $"/api/Products/{id}";
            return await SendDelete<bool>(requestURI);
        }

        bool IProductService.Delete(int id)
        {
            bool Result = false;
            Task.Run(async () => Result = await DeleteProductAsync(id)).Wait();
            return Result;
        }


        public async Task<List<Products>> GetAllProductsAsync()
        {
            return await SendGet<List<Products>>("/api/nwind/GetAllProducts");
        }

        List<Products> IProductService.GetAll()
        {
            List<Products> result = null;
            Task.Run(async () => result = await GetAllProductsAsync()).Wait();
            return result;
        }

        //FILTRO

        public async Task<List<Products>> FilterProductsByCategoryIDAsync(int categoryId)
        {
            var products = await SendGet<List<Products>>("/api/Products");
            return products.Where(p => p.CategoryID == categoryId).ToList();
        }

        //CATEOGRIAS

        public async Task<Categories> CreateCategoryAsync(Categories newCategory)
        {
            return await SendPost<Categories, Categories>
            ("/api/nwind/CreateCategory", newCategory);
        }
        public Categories CreateCategory(Categories newCategory)
        {
            Categories Result = null;
            Task.Run(async () => Result = await
           CreateCategoryAsync(newCategory)).Wait();
            return Result;
        }

        public async Task<List<Categories>> GetAllCategoriesAsync()
        {
            return await SendGet<List<Categories>>("/api/nwind/GetAllCategories");
        }

        List<Categories> ICategoriesService.GetAll()
        {
            List<Categories> result = null;
            Task.Run(async () => result = await GetAllCategoriesAsync()).Wait();
            return result;
        }

        public async Task<Categories> GetCategoryAsync(int id)
        {
            return await SendGet<Categories>($"/api/nwind/GetCategory/{id}");
        }

        public Categories GetCategory(int id)
        {
            Categories result = null;
            Task.Run(async () => result = await GetCategoryAsync(id)).Wait();
            return result;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await SendGet<bool>($"/api/nwind/DeleteCategory/{id}");
        }

        bool ICategoriesService.Delete(int id)
        {
            bool result = false;
            Task.Run(async () => result = await DeleteCategoryAsync(id)).Wait();
            return result;
        }

        public async Task<bool> UpdateCategoryAsync(Categories category)
        {
            return await SendPost<bool, Categories>("/api/nwind/UpdateCategory", category);
        }

        public bool UpdateCategories(Categories category)
        {
            bool result = false;
            Task.Run(async () => result = await UpdateCategoryAsync(category)).Wait();
            return result;
        }

    }
}