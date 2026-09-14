**ACOUNT APP**

**Instrucciones de ejecución**
- El proyecto se compone basicamente de dos microservicios (Microservice.Client y Microservice.Account)
- Existe un proyecto aparte que contiene la prueba unitaria realizada para Clients
- Crear un perfil único para inicio de proyecto que contenga los dos microservicios. (Se puede ejecutar por separado pero se recomienda armar un perfil único)

**Consideraciones**
- La union con la base de datos se hizo a través de una conexión con Windows Authenticate, si se va a realizar de otra manera considerar cambiar la cadena de conexión en el appsettings.json de cada proyecto
- Se adjunta dentro del proyecto la carpeta DBScripts con el .sql para crear la base de datos y el postman.collection
