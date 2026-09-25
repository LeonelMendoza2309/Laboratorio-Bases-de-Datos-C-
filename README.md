<div align="center">

# UNIVERSIDAD TECNOLÓGICA DE PANAMÁ  
Facultad de Ingeniería en Sistemas y Computación  
Lic. en Ingeniería de Sistemas Computacionales

</div>

---

# Proyecto Producto - CRUD de Productos

**Autor:** Leonel Mendoza (1IL133)  
**Repositorio:** [Laboratorio-Bases-de-Datos-C-](https://github.com/LeonelMendoza2309/Laboratorio-Bases-de-Datos-C-)

Aplicación de escritorio desarrollada en C# con Windows Forms para administrar productos mediante operaciones CRUD (*Create, Read, Update, Delete*) y una base de datos MySQL.

---

## Tabla de Contenidos

- [Descripción del proyecto](#descripción-del-proyecto)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Funcionalidades](#funcionalidades)
- [Tecnologías utilizadas](#tecnologías-utilizadas)
- [Requisitos](#requisitos)
- [Configuración de la base de datos](#configuración-de-la-base-de-datos)
- [Instalación y ejecución](#instalación-y-ejecución)
- [Uso de la aplicación](#uso-de-la-aplicación)
- [Problemas y soluciones](#problemas-y-soluciones)
- [Resultados](#resultados)
- [Conclusiones](#conclusiones)

---

## Descripción del proyecto

**ProyectoProducto** es una aplicación CRUD que permite gestionar un inventario de productos desde una interfaz gráfica. Cada producto contiene los siguientes datos:

- ID
- Nombre
- Precio
- Cantidad
- Imagen

La aplicación almacena la información en una base de datos MySQL y muestra los registros en un control `DataGridView`. También permite filtrar productos en tiempo real y guardar imágenes en formato binario (`byte[]`).

---

## Estructura del proyecto

```text
Laboratorio-Bases-de-Datos-C-/
├── ProyectoProducto/
│   └── ProyectoProducto/
│       ├── App.config                 # Configuración de .NET y redirecciones de ensamblados
│       ├── Form1.cs                   # Lógica principal de la interfaz gráfica
│       ├── Form1.Designer.cs          # Controles generados por el diseñador de Windows Forms
│       ├── Form1.resx                 # Recursos del formulario
│       ├── Productos.cs               # Modelo de datos de un producto
│       ├── conexionDB.cs              # Conexión y operaciones contra MySQL
│       ├── Program.cs                 # Punto de entrada de la aplicación
│       ├── packages.config            # Dependencias NuGet
│       └── ProyectoProducto.csproj    # Archivo del proyecto C#
└── README.md                          # Documentación del repositorio
```

### Componentes principales

- **`Productos.cs`**: define el modelo con las propiedades `ID`, `Nombre`, `Precio`, `Cantidad` e `Imagen`.
- **`Form1.cs`**: administra la interfaz, la carga de productos, la búsqueda, la selección de imágenes y las acciones CRUD.
- **`conexionDB.cs`**: encapsula la conexión a MySQL y las operaciones de consulta, inserción, actualización y eliminación.
- **`Program.cs`**: inicia la aplicación mediante `Application.Run(new Form1())`.

---

## Funcionalidades

- Agregar nuevos productos.
- Consultar y mostrar productos en un `DataGridView`.
- Buscar productos por ID, nombre, precio o cantidad.
- Modificar productos existentes.
- Eliminar productos seleccionados.
- Seleccionar y almacenar imágenes asociadas a los productos.
- Mostrar las imágenes almacenadas en la tabla.
- Limpiar los campos del formulario.
- Cerrar la aplicación desde el botón **Salir**.

---

## Tecnologías utilizadas

- **Lenguaje:** C#
- **Interfaz gráfica:** Windows Forms
- **Framework:** .NET Framework 4.7.2
- **Base de datos:** MySQL
- **Conector:** `MySql.Data`
- **Controles principales:** `DataGridView`, `PictureBox`, `TextBox`, `Button` y `Panel`
- **Entorno recomendado:** Visual Studio 2019 o superior
- **Control de versiones:** Git y GitHub

---

## Requisitos

Para compilar y ejecutar el proyecto se necesita:

- Windows 10 o superior.
- Visual Studio 2019, 2022 o una versión compatible con proyectos de .NET Framework.
- .NET Framework 4.7.2.
- MySQL Server ejecutándose localmente.
- MySQL Workbench, phpMyAdmin u otra herramienta para crear la base de datos.
- Git para clonar el repositorio.

---

## Configuración de la base de datos

La aplicación utiliza una base de datos llamada `ProdDB` y una tabla llamada `productos`.

### 1. Crear la base de datos y la tabla

Ejecutar el siguiente script en MySQL:

```sql
CREATE DATABASE IF NOT EXISTS ProdDB;
USE ProdDB;

CREATE TABLE IF NOT EXISTS productos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    precio DECIMAL(10, 2) NOT NULL,
    cantidad INT NOT NULL DEFAULT 0,
    imagen LONGBLOB NULL
);
```

### 2. Configurar la conexión

En `ProyectoProducto/ProyectoProducto/conexionDB.cs`, ajustar la cadena de conexión de acuerdo con las credenciales del entorno local:

```csharp
server=localhost;port=3306;database=ProdDB;user id=USUARIO;password=CONTRASEÑA;
```

> **Importante:** no se deben publicar contraseñas reales en el repositorio. Para un entorno de producción, la cadena de conexión debe almacenarse en una configuración segura o mediante variables de entorno.

---

## Instalación y ejecución

### Clonar el repositorio

```bash
git clone https://github.com/LeonelMendoza2309/Laboratorio-Bases-de-Datos-C-.git
cd Laboratorio-Bases-de-Datos-C-
```

### Abrir el proyecto

1. Iniciar Visual Studio.
2. Abrir la solución o el archivo `ProyectoProducto/ProyectoProducto/ProyectoProducto.csproj`.
3. Restaurar los paquetes NuGet si Visual Studio lo solicita.
4. Confirmar que MySQL Server esté iniciado.
5. Verificar la cadena de conexión en `conexionDB.cs`.
6. Establecer `ProyectoProducto` como proyecto de inicio.
7. Ejecutar con **F5** o **Ctrl + F5**.

El ejecutable generado se encontrará, según la configuración seleccionada, en una de las siguientes carpetas:

```text
ProyectoProducto/ProyectoProducto/bin/Debug/
ProyectoProducto/ProyectoProducto/bin/Release/
```

---

## Uso de la aplicación

1. **Agregar:** completar nombre, precio, cantidad e imagen; luego seleccionar **Agregar**.
2. **Consultar:** los productos registrados se muestran automáticamente en la tabla.
3. **Buscar:** escribir un término en el campo **Búsqueda** para filtrar los registros.
4. **Modificar:** seleccionar un producto, editar sus datos y presionar **Modificar**.
5. **Eliminar:** seleccionar una fila y presionar **Eliminar**.
6. **Limpiar:** presionar **Limpiar** para vaciar los campos del formulario.
7. **Salir:** presionar **Salir** para cerrar la aplicación.

---

## Problemas y soluciones

| # | Problema | Solución |
|---:|---|---|
| 1 | La aplicación no se conecta a MySQL | Verificar que el servidor esté activo, que `ProdDB` exista y que las credenciales sean correctas. |
| 2 | No se muestran productos | Confirmar que la tabla `productos` tenga la estructura indicada y que la consulta tenga registros. |
| 3 | Las imágenes no se visualizan | Comprobar que el archivo seleccionado sea válido y que la columna `imagen` sea de tipo `LONGBLOB`. |
| 4 | Error al restaurar dependencias | Restaurar los paquetes NuGet desde Visual Studio y comprobar la referencia a `MySql.Data`. |
| 5 | Error al convertir precio o cantidad | Introducir un precio numérico y una cantidad entera usando el formato esperado por la configuración regional. |

---

## Resultados

El proyecto implementa una aplicación funcional para la administración de productos con:

- Persistencia de datos en MySQL.
- Operaciones CRUD.
- Búsqueda dinámica.
- Visualización de imágenes.
- Interfaz gráfica de escritorio mediante Windows Forms.
- Separación básica entre la interfaz, el modelo `Productos` y la capa de acceso a datos `conexionDB`.

---

## Conclusiones

- Se aplicaron conceptos de programación orientada a objetos mediante un modelo de productos.
- Se implementó la comunicación entre una aplicación C# y una base de datos MySQL.
- Se comprendió el funcionamiento de las operaciones CRUD.
- Windows Forms permitió construir una interfaz sencilla para gestionar información.
- El uso de consultas parametrizadas ayuda a proteger las operaciones de la base de datos.
- El almacenamiento de imágenes como datos binarios permite asociar recursos visuales a los productos.

---

**Última actualización:** 25/09/2026  
**Estado:** Completado
