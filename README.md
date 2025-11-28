# 🚗 RentCar – Modelo de sistema de renta de vehículos

Proyecto base para un sistema de **alquiler de vehículos** usando:

- ASP.NET Core / **Blazor Server**
- **Entity Framework Core** (EF Core) para acceso a datos
- **ASP.NET Core Identity** para autenticación y autorización
- **SQL Server** como base de datos

Su propósito es servir como plantilla/ejemplo para construir un sistema real de renta de autos, con una arquitectura organizada por capas (datos, servicios y UI).

------------------------------------------------------------
## 1. Estructura del repositorio

ModeloRentaCar/
├── .gitattributes
├── .gitignore
├── RentCar.sln                 # Solución de Visual Studio
└── RentCar/                    # Proyecto web principal (Blazor + EF Core + Identity)
    ├── Program.cs              # Configuración principal de la app
    ├── appsettings.json        # Configuración (connection strings, logging, etc.)
    ├── Comentarios.md          # Notas/documentación técnica interna
    ├── README.md               # Documentación del proyecto (este archivo)
    │
    ├── Data/                   # Capa de acceso a datos y dominio
    │   ├── Context/
    │   │   ├── ApplicationDbContext.cs
    │   │   │   # DbContext principal: hereda de IdentityDbContext<ApplicationUser>
    │   │   │   # Incluye DbSet<T> de las entidades y configuración EF Core.
    │   │   └── ApplicationUser.cs
    │   │       # Usuario de Identity extendido para personalizar datos de usuario.
    │   │
    │   ├── Models/             # Entidades del dominio (tablas de la BD)
    │   │   ├── Vehiculo.cs     # Vehículo, con datos como marca, modelo, año, placa, etc.
    │   │   ├── TipoVehiculo.cs # Tipo de vehículo (SUV, Sedán, etc.)
    │   │   ├── Categoria.cs    # Categoría comercial de los vehículos.
    │   │   ├── Cliente.cs      # Información de clientes (identificación y contacto).
    │   │   └── Usuario.cs      # Usuarios internos (empleados del sistema).
    │   │
    │   └── Services/           # Lógica de negocio y acceso a datos vía servicios
    │       ├── IVehiculoService.cs
    │       ├── IClienteService.cs
    │       ├── IUsuarioService.cs
    │       ├── VehiculoService.cs
    │       ├── ClienteService.cs
    │       └── UsuarioService.cs
    │       # Servicios para operaciones CRUD y consultas sobre las entidades.
    │
    └── Web/                    # Capa de presentación (Blazor Server)
        └── Components/
            ├── _Imports.razor  # Usings globales para los componentes.
            ├── App.razor       # Componente raíz de la aplicación Blazor.
            ├── Routes.razor    # Definición de rutas (routing de la app, si aplica).
            │
            ├── Layout/
            │   └── MainLayout.razor
            │       # Layout principal: estructura base de páginas (header, body, etc.)
            │
            └── Account/        # Integración con Identity en Blazor
                ├── Shared/
                │   └── AccountLayout.razor
                │       # Layout específico para páginas de autenticación.
                │
                ├── IdentityComponentsEndpointRouteBuilderExtensions.cs
                │   # Extensiones para mapear las páginas/componentes de Identity.
                │
                └── Pages/
                    └── _Imports.razor
                    # Imports específicos para componentes/páginas de cuenta.

------------------------------------------------------------
## 2. Descripción general del proyecto

El proyecto **RentCar** modela el dominio básico de un sistema de renta de vehículos:

- **Vehículos**: información de inventario (marca, modelo, año, placa, estado, tipo y categoría).
- **Clientes**: datos de las personas que rentan los vehículos.
- **Usuarios internos**: personal que administra el sistema (empleados, administradores).
- **Tipos y categorías de vehículo**: permiten clasificar el inventario.

La capa de datos (`Data`) se encarga de:

- Definir las **entidades** (clases de modelo).
- Configurar el **DbContext** (`ApplicationDbContext`) que:

  - Hereda de `IdentityDbContext<ApplicationUser>`.
  - Expone las tablas a través de `DbSet<T>`.
  - Se conecta a SQL Server mediante la cadena de conexión en `appsettings.json`.

- Proveer **servicios** (`Services`) que encapsulan la lógica de negocio y acceso a la base de datos (CRUD, consultas, etc.).

La capa web (`Web/Components`) está construida con **Blazor Server** y define:

- La aplicación raíz (`App.razor`).
- El layout principal (`MainLayout.razor`).
- Los componentes relacionados con **Identity** (login, registro, etc.) bajo `Account/`.

------------------------------------------------------------
## 3. Tecnologías principales

- **.NET 8** (o versión similar)
- **Blazor Server** para la interfaz web.
- **Entity Framework Core** para mapeo objeto-relacional (ORM).
- **ASP.NET Core Identity** para usuarios, roles y autenticación.
- **SQL Server** como base de datos relacional.

------------------------------------------------------------
## 4. Flujo básico de la aplicación

1. **Inicio de la aplicación**  
   `Program.cs` configura:

   - Servicios de EF Core (`ApplicationDbContext`).
   - Identity (`ApplicationUser`, cookies, etc.).
   - Servicios de dominio (`IVehiculoService`, `IClienteService`, etc.).
   - Soporte para Blazor Server y el routing.

2. **Acceso a datos**  
   Los componentes Blazor consumen los servicios de la capa `Data/Services`, que usan el `ApplicationDbContext` para:

   - Crear, leer, actualizar y eliminar registros de:
     - Vehículos
     - Clientes
     - Usuarios internos
     - Tipos y categorías de vehículos

3. **Autenticación y autorización**  
   - Identity gestiona usuarios y roles.
   - La UI puede usar `AuthorizeView` y políticas de autorización para limitar acceso a partes de la app.

------------------------------------------------------------
## 5. Cómo ejecutar el proyecto (resumen)

1. Clonar el repositorio:

   git clone https://github.com/Rele00/ModeloRentaCar.git
   cd ModeloRentaCar

2. Abrir la solución `RentCar.sln` con Visual Studio 2022 (u otro IDE compatible).

3. Configurar la cadena de conexión en `RentCar/appsettings.json` (sección `ConnectionStrings`).

4. Aplicar migraciones de EF Core (si están creadas) o crear una inicial:

   # Desde la carpeta RentCar:
   dotnet ef migrations add Inicial
   dotnet ef database update

5. Ejecutar la aplicación:

   dotnet run

   Luego abrir el navegador en la URL indicada (por ejemplo, https://localhost:xxxx).

------------------------------------------------------------
## 6. Estado del proyecto

El proyecto se encuentra en fase de **plantilla/estructura inicial**, centrado en:

- Definir el modelo de datos y las entidades clave.
- Configurar EF Core e Identity.
- Montar la base de la interfaz con Blazor Server.

Se puede extender fácilmente añadiendo:

- Páginas CRUD completas para vehículos, clientes y usuarios.
- Módulo de rentas (contratos, devoluciones, etc.).
- Reportes y paneles de administración.
