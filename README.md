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

- Modelar el dominio de un sistema de alquiler de vehículos (vehículos, clientes, usuarios, categorías, tipos de vehículo).
- Persistir datos con EF Core y SQL Server.
- Integrar autenticación y autorización con ASP.NET Identity.
- Consumir servicios y datos desde componentes Blazor Server.
- Mantener una arquitectura limpia y escalable.

---

## 2. Estructura de archivos y carpetas actual

```
-- 📁 Estructura del proyecto RentCar

RentCar/
├── Program.cs                               # Configuración principal: servicios, Identity, DbContext y registro de endpoints Blazor.
├── appsettings.json                         # Configuración de entorno: cadena de conexión (DefaultConnection), logging, etc.
├── Comentarios.md                           # Notas y documentación técnica adicional.
├── README.md                                # Documentación principal del proyecto.
│
├── Data/                                    # Capa de acceso a datos (Entity Framework Core + lógica de persistencia)
│   ├── Context/                             # Contextos y clases relacionadas con EF Core e Identity
│   │   ├── ApplicationDbContext.cs          # Hereda de IdentityDbContext<ApplicationUser>; define DbSet<> y configuración Fluent API.
│   │   └── ApplicationUser.cs               # Clase personalizada de usuario (extiende IdentityUser) — opcional.
│   │
│   ├── Models/                              # Entidades del dominio (tablas del modelo relacional)
│   │   ├── Vehiculo.cs                      # Entidad Vehículo (marca, modelo, año, tipo, categoría, relaciones).
│   │   ├── TipoVehiculo.cs                  # Tipos de vehículo (SUV, Sedan, etc.) — relación con Vehiculo.
│   │   ├── Categoria.cs                     # Categorías comerciales — relación con Vehiculo.
│   │   ├── Cliente.cs                       # Entidad Cliente (datos personales, contacto, historial).
│   │   └── Usuario.cs                       # Entidad para usuarios internos (Id, Nombre, Email, Teléfono, Rol, EsActivo).
│   │
│   └── Services/                            # Servicios que encapsulan la lógica de negocio y persistencia
│       ├── IVehiculoService.cs              # Interfaz CRUD y consultas para Vehículo.
│       ├── IClienteService.cs               # Interfaz CRUD para Cliente.
│       ├── IUsuarioService.cs               # Interfaz CRUD para Usuario.
│       ├── VehiculoService.cs               # Implementación de IVehiculoService.
│       ├── ClienteService.cs                # Implementación de IClienteService.
│       └── UsuarioService.cs                # Implementación de IUsuarioService.
│
└── Web/                                     # Capa de presentación (interfaz Blazor Server)
    └── Components/                          # Componentes organizados por responsabilidad
        ├── _Imports.razor                   # Usings/imports globales para todos los componentes.
        ├── App.razor                        # Componente raíz de la aplicación Blazor.
        ├── Routes.razor                     # Definición de rutas (si existe).
        │
        ├── Layout/                          # Layouts y componentes de estructura visual
        │   └── MainLayout.razor             # Layout principal (navbar, sidebar, footer, estructura de página).
        │
        └── Account/                         # Componentes y páginas relacionadas con cuentas/Identity
            ├── Shared/
            │   └── AccountLayout.razor      # Layout específico para las páginas de autenticación (maneja HttpContext).
            │
            ├── IdentityComponentsEndpointRouteBuilderExtensions.cs
            │                                # Extensiones para registrar endpoints de páginas de Identity como componentes Blazor.
            │
            └── Pages/
                └── _Imports.razor           # Imports específicos para los componentes/páginas de cuenta.




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

Esta documentación proporciona una visión general de la arquitectura y estructura del proyecto **Rent Car**, sirviendo como guía para el desa
