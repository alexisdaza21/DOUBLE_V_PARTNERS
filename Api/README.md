Aplicación desarrollada en .net core 9

respuesta estandar api:

    200 OK: La solicitud ha sido exitosa.
    201 Created: La solicitud ha sido exitosa y se ha creado un nuevo recurso.
    204 No Content: La solicitud ha sido exitosa, pero no hay contenido para enviar en la respuesta.
    400 Bad Request: La solicitud no se pudo entender o está mal formada.
    401 Unauthorized: Se requiere autenticación para acceder al recurso solicitado.
    403 Forbidden: No tienes permiso para acceder al recurso solicitado.
    404 Not Found: El recurso solicitado no se pudo encontrar en el servidor.
    500 Internal Server Error: Se produjo un error interno en el servidor.

    # el aplicativo esta basado en .net core 9 utiliza autenticacion JWT para algunos enponts adicional implementa clains para 
    # validar el rol permitiendo gentionar metodos de manera más segura, la conexion esta basa en EF con dbContext con base de datos
    # Postgres se utiliza linq para la administracion de datos y Redis para manejo de cache en metodos de consulta actualizando el cache
    # se maneja controlador <> contrato <> servicio, y DTOs para el manejo de clases
    # depuración:
    # se debe tener instalado redis aunque si no esta se controla la conexion para apuntar solo a Base, la cadena de conexion esta en appsettings.Json 
    # junto con configuracion de la generacion de token
    # para consumir el api desde Swuagger es necesario ir al controlado Usurios > GetTokenAnonimo obtener el token devuelto y agregarlo
    # en la seccion autenticacion de esta forma "Bearer [token]" en swagger se detalla, este token solo es valido para algunos metodos
