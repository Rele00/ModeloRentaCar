# Documentación y estructura — **Rent Car (Blazor + EF Core + Identity)**

Este documento describe la arquitectura y estructura actual del proyecto **Rent Car**, desarrollado con **Blazor Server**, **Entity Framework Core** (.NET 8) e integración de **ASP.NET Identity**. Incluye la estructura real de archivos, ejemplos de entidades, configuración de `DbContext`, servicios, y buenas prácticas.

---

## Tabla de contenidos
1. Objetivo
2. Estructura de archivos y carpetas actual
3. Entidades principales (modelos)
4. DbContext y configuración EF Core
5. Servicios y patrón Repository
6. Integración con Blazor Server
7. Seguridad e Identity
8. Migraciones y pruebas
9. Buenas prácticas

---

## 1. Objetivo

- Modelar el dominio de un sistema de alquiler de vehículos (vehículos, clientes, usuarios, categorías).
- Persistir datos con EF Core y SQL Server.
- Integrar autenticación y autorización con ASP.NET Identity.
- Consumir servicios y datos desde componentes Blazor Server.
- Mantener una arquitectura limpia y escalable.

---

## 2. Estructura de archivos y carpetas actual

```
RentCar/                                 // Solución principal del proyecto RentCar (backend + frontend)
├─ Program.cs                            // Punto de entrada de la aplicación (configura servicios, middleware, etc.)
├─ appsettings.json                      // Archivo de configuración (cadena de conexión, logging, opciones de la app)
├─ README.md                             // Documento de explicación general del proyecto (cómo ejecutar, propósito, etc.)
├─ Comentarios.md                        // Notas, decisiones de diseño, pendientes o documentación interna
├─ ConsultaRentCar_ejemplo.sql           // Script SQL de ejemplo (consultas para practicar/reportes sobre la BD)
├─ Data/                                 // Capa de acceso a datos y lógica de negocio básica
│  ├─ Context/                           // Contexto de Entity Framework Core (mapeo a la base de datos)
│  │  ├─ ApplicationDbContext.cs         // DbContext principal: define DbSet<>, configuración de tablas y relaciones
│  │  └─ ApplicationUser.cs              // Clase de usuario que extiende IdentityUser (Nombre, Teléfono, etc.)
│  ├─ Models/                            // Modelos de dominio (tablas de la base de datos)
│  │  ├─ Vehiculo.cs                     // Entidad Vehiculo: Marca, Modelo, Placa, Año, Estado, CategoriaId, etc.
│  │  ├─ Movimiento.cs                   // Entidad Movimiento: Vehiculo, Cliente, tipo de movimiento, fechas, etc.
│  │  ├─ Categoria.cs                    // Entidad Categoria: nombre y descripción de la categoría de vehículos
│  │  └─ Cliente.cs                      // Entidad Cliente: datos personales, licencia, vigencia, estado activo, etc.
│  ├─ Dtos/                              // DTOs (Data Transfer Objects) para exponer datos al frontend o API
│  │  ├─ MasUsadosDto.cs                 // DTO para reporte de vehículos más usados (Id, Marca, VecesUsado, etc.)
│  │  ├─ VehiculoDto.cs                  // DTO de Vehiculo (campos necesarios para vistas/formularios)
│  │  ├─ MovimientoDto.cs                // DTO de Movimiento (para listar y registrar movimientos)
│  │  ├─ ClienteDto.cs                   // DTO de Cliente (para formularios y listados)
│  │  └─ CategoriaDto.cs                 // DTO de Categoria (para combos/listados de categorías)
│  └─ Services/                          // Servicios (lógica de negocio y acceso a datos encapsulado)
│     ├─ IMasUsadosService.cs            // Interfaz del servicio que obtiene los vehículos más usados
│     ├─ MasUsadosService.cs             // Implementación del servicio de vehículos más usados (consultas agrupadas, TOP N, etc.)
│     ├─ IVehiculoService.cs             // Interfaz para operaciones con vehículos (CRUD, filtros, etc.)
│     ├─ VehiculoService.cs              // Implementación del servicio de Vehiculos (usa ApplicationDbContext)
│     ├─ IMovimientoService.cs           // Interfaz para operaciones con movimientos (rentas, devoluciones, etc.)
│     ├─ MovimientoService.cs            // Implementación del servicio de Movimientos
│     ├─ IClienteService.cs              // Interfaz para operaciones con clientes (alta, baja, consulta)
│     └─ ClienteService.cs               // Implementación del servicio de Clientes
└─ Web/                                  // Capa de presentación (Blazor / componentes de interfaz)
   └─ Components/                        // Componentes Razor del frontend
      ├─ App.razor                       // Raíz de la app Blazor (define Router y layout base)
      ├─ Routes.razor                    // Definición de rutas (mapa de páginas de la aplicación)
      ├─ _Imports.razor                  // Usings y directivas compartidas por los componentes Razor
      ├─ Pages/                          // Páginas de la aplicación
      │  └─ Dashboard/                   // Módulo de dashboard (pantalla principal / resumen)
      │     ├─ Index.razor               // Página principal del dashboard (gráficas, tarjetas de resumen, etc.)
      │     ├─ Index.razor.css           // Estilos específicos del dashboard (CSS aislado para Index.razor)
      │     └─ _Imports.razor            // Usings y directivas locales para el área de Dashboard
      ├─ Layout/                         // Componentes de layout (estructura visual general)
      │  ├─ MainLayout.razor             // Layout principal (header, sidebar, content)
      │  └─ NavMenu.razor                // Menú de navegación lateral (links a Dashboard, Vehículos, Clientes, etc.)
      └─ Account/                        // Lógica de cuentas y autenticación con Identity
         ├─ Pages/                       // Páginas de autenticación
         │  ├─ Login.razor               // Página de inicio de sesión
         │  └─ Register.razor            // Página de registro de nuevos usuarios
         ├─ IdentityRedirectManager.cs   // Maneja redirecciones de Identity (login, logout, acceso no autorizado)
         └─ IdentityNoOpEmailSender.cs   // Implementación “vacía” para envío de correo (placeholder para confirmación de email)




```

---

## 3. Entidades principales (modelos)
Ejemplos de entidades del dominio, como `Vehiculo`, `Cliente`, `Reserva`, con sus propiedades y relaciones.

---

## 4. DbContext y configuración EF Core
Configuración de `DbContext` usando Fluent API, definición de DbSets para las entidades principales y configuración de la cadena de conexión a la base de datos.

---

## 5. Servicios y patrón Repository
Definición de interfaces y clases para los repositorios y servicios, incluyendo `IVehiculoRepository`, `IClienteService`, etc. Implementación del patrón Unit of Work opcional.

---

## 6. Integración con Blazor Server

- Los servicios se inyectan en los componentes Blazor.
- Consumo de APIs de la aplicación a través de `HttpClient` y servicios personalizados.
- Manejo del estado de la aplicación utilizando `CascadingAuthenticationState` y `AuthorizeView`.

---

## 7. Seguridad e Identity

- Configuración de **ASP.NET Identity** para gestión de usuarios y roles en `Program.cs`.
- Páginas y componentes Blazor para registro, inicio de sesión y gestión de usuarios.
- Políticas de autorización y autenticación basadas en roles y requisitos personalizados.

---

## 8. Migraciones y pruebas

- Uso de `dotnet ef migrations` para gestionar cambios en el modelo de datos.
- Pruebas automatizadas con pruebas unitarias y de integración, usando una base de datos en memoria para pruebas.

---

## 9. Buenas prácticas

- Separación clara entre las capas de presentación, aplicación y acceso a datos.
- Uso de DTOs y AutoMapper para la transferencia de datos entre capas.
- Validación de datos en el servidor y cliente.
- Documentación del código y uso de comentarios claros y concisos.

---

## Conclusión

Esta documentación proporciona una visión general de la arquitectura y estructura del proyecto **Rent Car**, sirviendo como guía para el desarrollo, integración y mantenimiento del sistema.
