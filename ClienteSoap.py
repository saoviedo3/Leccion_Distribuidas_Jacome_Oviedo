from zeep import Client
from zeep.transports import Transport
from requests import Session
import warnings
from requests.packages.urllib3.exceptions import InsecureRequestWarning

# Deshabilitar advertencias SSL durante las pruebas
warnings.simplefilter('ignore', InsecureRequestWarning)

# Configurar sesión HTTP para ignorar la verificación SSL
session = Session()

session.verify = False  # Deshabilitar la verificación de certificados SSL
transport = Transport(session=session)

# URL del WSDL del servidor (reemplazar con la IP o hostname de la máquina que hospeda el servicio)
wsdl_url = "http://localhost:62050/Service1.svc?wsdl"

# Crear cliente SOAP
client = Client(wsdl=wsdl_url, transport=transport)

# Validar entradas de usuario
def validate_number_input(prompt):
    while True:
        try:
            value = int(input(prompt).strip())
            if value < 0:
                raise ValueError("El valor no puede ser negativo.")
            return value
        except ValueError as e:
            print(f"Entrada inválida: {e}. Intente nuevamente.")

def validate_decimal_input(prompt):
    while True:
        try:
            value = float(input(prompt).strip())
            return value
        except ValueError:
            print("Entrada inválida. Ingrese un número decimal.")

def validate_text_input(prompt):
    while True:
        value = input(prompt).strip()
        if value:
            return value
        else:
            print("El campo no puede estar vacío.")

# Manejo de errores SOAP
def handle_soap_fault(exception):
    print(f"Error: {exception}")

# CRUD para productos
def get_all_products():
    try:
        products = client.service.GetAllProducts()
        if not products:
            print("No hay productos disponibles.")
            return
        print("\nLista de productos:")
        for product in products:
            print(f"- ID: {product.ProductID}, Nombre: {product.ProductName}, Categoría: {product.CategoryID}, Precio: {product.UnitPrice}, Stock: {product.UnitsInStock}")
    except Exception as e:
        handle_soap_fault(e)

def get_product_by_id():
    try:
        product_id = validate_number_input("Ingrese el ID del producto: ")
        product = client.service.GetProductById(product_id)
        if product:
            print(f"Detalles del producto - ID: {product.ProductID}, Nombre: {product.ProductName}, Categoría: {product.CategoryID}, Precio: {product.UnitPrice}, Stock: {product.UnitsInStock}")
        else:
            print(f"No se encontró el producto con ID {product_id}.")
    except Exception as e:
        print(f"Error al buscar producto por ID: {e}")

def create_product():
    try:
        product_id = validate_number_input("Ingrese el ID del producto: ")
        name = validate_text_input("Ingrese el nombre del producto: ")
        category_id = validate_number_input("Ingrese el ID de la categoría: ")
        price = validate_decimal_input("Ingrese el precio del producto: ")
        stock = validate_number_input("Ingrese la cantidad en stock: ")

        result = client.service.CreateProduct(product_id, name, category_id, price, stock)
        if result:
            print("Producto creado exitosamente.")
        else:
            print("No se pudo crear el producto.")
    except Exception as e:
        handle_soap_fault(e)

def update_product():
    try:
        product_id = validate_number_input("Ingrese el ID del producto a actualizar: ")
        name = validate_text_input("Ingrese el nuevo nombre del producto: ")
        category_id = validate_number_input("Ingrese el nuevo ID de la categoría: ")
        price = validate_decimal_input("Ingrese el nuevo precio del producto: ")
        stock = validate_number_input("Ingrese la nueva cantidad en stock: ")

        result = client.service.UpdateProduct(product_id, name, category_id, price, stock)
        if result:
            print("Producto actualizado exitosamente.")
        else:
            print("No se pudo actualizar el producto.")
    except Exception as e:
        handle_soap_fault(e)

def delete_product():
    try:
        product_id = validate_number_input("Ingrese el ID del producto a eliminar: ")
        result = client.service.DeleteProduct(product_id)
        if result:
            print("Producto eliminado exitosamente.")
        else:
            print("No se pudo eliminar el producto.")
    except Exception as e:
        handle_soap_fault(e)

# CRUD para categorías
def get_all_categories():
    try:
        categories = client.service.GetAllCategories()
        if not categories:
            print("No hay categorías disponibles.")
            return
        print("\nLista de categorías:")
        for category in categories:
            print(f"- ID: {category.CategoryID}, Nombre: {category.CategoryName}, Descripción: {category.Description}")
    except Exception as e:
        handle_soap_fault(e)

def create_category():
    try:
        category_id = validate_number_input("Ingrese el ID de la categoría: ")
        name = validate_text_input("Ingrese el nombre de la categoría: ")
        description = validate_text_input("Ingrese la descripción de la categoría: ")

        result = client.service.CreateCategory(category_id, name, description)
        if result:
            print("Categoría creada exitosamente.")
        else:
            print("No se pudo crear la categoría.")
    except Exception as e:
        handle_soap_fault(e)

def delete_category():
    try:
        category_id = validate_number_input("Ingrese el ID de la categoría a eliminar: ")
        result = client.service.DeleteCategory(category_id)
        if result:
            print("Categoría eliminada exitosamente.")
        else:
            print("No se pudo eliminar la categoría.")
    except Exception as e:
        handle_soap_fault(e)
        
def update_category():
    try:
        category_id = validate_number_input("Ingrese el ID de la categoría a actualizar: ")
        name = validate_text_input("Ingrese el nuevo nombre de la categoría: ")
        description = validate_text_input("Ingrese la nueva descripción de la categoría: ")

        result = client.service.UpdateCategory(category_id, name, description)
        if result:
            print("Categoría actualizada exitosamente.")
        else:
            print("No se pudo actualizar la categoría.")
    except Exception as e:
        handle_soap_fault(e)


# Menú interactivo
def menu():
    while True:
        print("\n===== Menú de Operaciones CRUD =====")
        print("1. Obtener todos los productos")
        print("2. Obtener un producto por ID")
        print("3. Crear un producto")
        print("4. Actualizar un producto")
        print("5. Eliminar un producto")
        print("6. Obtener todas las categorías")
        print("7. Crear una categoría")
        print("8. Actualizar una categoría")
        print("9. Eliminar una categoría")
        print("10. Salir")

        opcion = input("\nSeleccione una opción: ")

        if opcion == "1":
            get_all_products()
        elif opcion == "2":
            get_product_by_id()
        elif opcion == "3":
            create_product()
        elif opcion == "4":
            update_product()
        elif opcion == "5":
            delete_product()
        elif opcion == "6":
            get_all_categories()
        elif opcion == "7":
            create_category()
        elif opcion == "8":  # Nueva opción
            update_category()
        elif opcion == "9":
            delete_category()
        elif opcion == "10":
            print("Saliendo del programa...")
            break
        else:
            print("Opción no válida. Intente nuevamente.")


# Ejecutar el menú
if __name__ == "__main__":
    menu()