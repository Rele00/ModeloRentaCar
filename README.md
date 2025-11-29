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

## 2. Estructura de archivos y carpetas antigua

```
ModeloRentaCar-master
├── .gitattributes
├── .gitignore
├── README.md
├── RentCar.sln
└── RentCar
    ├── Program.cs
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── Comentarios.md
    ├── RentCar.csproj
    ├── README.md
    │
    ├── Properties
    │   ├── launchSettings.json
    │   ├── serviceDependencies.json
    │   └── serviceDependencies.local.json
    │
    ├── Data
    │   ├── Context
    │   │   ├── ApplicationDbContext.cs
    │   │   └── ApplicationUser.cs
    │   │
    │   ├── Dtos
    │   │   ├── CategoriaDto.cs
    │   │   ├── ClienteDto.cs
    │   │   ├── MasUsadosDto.cs
    │   │   ├── MovimientoDto.cs
    │   │   ├── RentaCreateDto.cs
    │   │   ├── RentaDevolucionDtp.cs
    │   │   ├── RentaResumenDto.cs
    │   │   └── VehiculoDto.cs
    │   │
    │   ├── Models
    │   │   ├── Categoria.cs
    │   │   ├── Cliente.cs
    │   │   ├── Factura.cs
    │   │   ├── Movimiento.cs
    │   │   ├── Renta.cs
    │   │   └── Vehiculo.cs
    │   │
    │   ├── Services
    │   │   ├── ICategoriaService.cs
    │   │   ├── IClienteService.cs
    │   │   ├── IMasUsadosService.cs
    │   │   ├── IMovimientoService.cs
    │   │   ├── IRentaService.cs
    │   │   ├── IVehiculoService.cs
    │   │   ├── CategoriaService.cs
    │   │   ├── ClienteService.cs
    │   │   ├── MasUsadosService.cs
    │   │   ├── MovimientoService.cs
    │   │   ├── RentaService.cs
    │   │   └── VehiculoService.cs
    │   │
    │   └── Migrations
    │       ├── 00000000000000_CreateIdentitySchema.cs
    │       ├── 00000000000000_CreateIdentitySchema.Designer.cs
    │       ├── 20251024150524_InitialPrueba5_Remake.cs
    │       ├── 20251024150524_InitialPrueba5_Remake.Designer.cs
    │       ├── 20251024152149_InitialPrueba6_Ajustes.cs
    │       ├── 20251024152149_InitialPrueba6_Ajustes.Designer.cs
    │       ├── 20251031001023_InitialAgregue_Categoria.cs
    │       ├── 20251031001023_InitialAgregue_Categoria.Designer.cs
    │       ├── 20251114220314_DashBoard.cs
    │       ├── 20251114220314_DashBoard.Designer.cs
    │       ├── 20251122234407_CssFeo.cs
    │       ├── 20251122234407_CssFeo.Designer.cs
    │       ├── 20251126010238_Categoria.cs
    │       ├── 20251126010238_Categoria.Designer.cs
    │       ├── 20251126023346_Categoria2.cs
    │       ├── 20251126023346_Categoria2.Designer.cs
    │       ├── 20251127183144_Renta.cs
    │       ├── 20251127183144_Renta.Designer.cs
    │       └── ApplicationDbContextModelSnapshot.cs
    │
    ├── Web
    │   └── Components
    │       ├── App.razor
    │       ├── Component.razor
    │       ├── Routes.razor
    │       ├── _Imports.razor
    │       │
    │       ├── Account
    │       │   ├── IdentityComponentsEndpointRouteBuilderExtensions.cs
    │       │   ├── IdentityNoOpEmailSender.cs
    │       │   ├── IdentityRedirectManager.cs
    │       │   ├── IdentityRevalidatingAuthenticationStateProvider.cs
    │       │   ├── IdentityUserAccessor.cs
    │       │   │
    │       │   ├── Pages
    │       │   │   ├── AccessDenied.razor
    │       │   │   ├── ConfirmEmail.razor
    │       │   │   ├── ConfirmEmailChange.razor
    │       │   │   ├── ExternalLogin.razor
    │       │   │   ├── ForgotPassword.razor
    │       │   │   ├── ForgotPasswordConfirmation.razor
    │       │   │   ├── InvalidPasswordReset.razor
    │       │   │   ├── InvalidUser.razor
    │       │   │   ├── Lockout.razor
    │       │   │   ├── Login.razor
    │       │   │   ├── Login.razor.css
    │       │   │   ├── LoginWith2fa.razor
    │       │   │   ├── LoginWithRecoveryCode.razor
    │       │   │   ├── Logout.razor
    │       │   │   ├── Register.razor
    │       │   │   ├── Register.razor.css
    │       │   │   ├── RegisterConfirmation.razor
    │       │   │   ├── ResendEmailConfirmation.razor
    │       │   │   ├── ResetPassword.razor
    │       │   │   ├── ResetPasswordConfirmation.razor
    │       │   │   └── _Imports.razor
    │       │   │
    │       │   ├── Pages/Manage
    │       │   │   ├── ChangePassword.razor
    │       │   │   ├── Component.razor
    │       │   │   ├── DeletePersonalData.razor
    │       │   │   ├── Disable2fa.razor
    │       │   │   ├── Email.razor
    │       │   │   ├── EnableAuthenticator.razor
    │       │   │   ├── ExternalLogins.razor
    │       │   │   ├── GenerateRecoveryCodes.razor
    │       │   │   ├── Index.razor
    │       │   │   ├── PersonalData.razor
    │       │   │   ├── ResetAuthenticator.razor
    │       │   │   ├── SetPassword.razor
    │       │   │   ├── TwoFactorAuthentication.razor
    │       │   │   └── _Imports.razor
    │       │   │
    │       │   └── Shared
    │       │       ├── AccountLayout.razor
    │       │       ├── ExternalLoginPicker.razor
    │       │       ├── ManageLayout.razor
    │       │       ├── ManageNavMenu.razor
    │       │       ├── RedirectToLogin.razor
    │       │       ├── ShowRecoveryCodes.razor
    │       │       └── StatusMessage.razor
    │       │
    │       ├── Layout
    │       │   ├── MainLayout.razor
    │       │   ├── MainLayout.razor.css
    │       │   ├── NavMenu.razor
    │       │   └── NavMenu.razor.css
    │       │
    │       └── Pages
    │           ├── Auth.razor
    │           ├── Counter.razor
    │           ├── Error.razor
    │           ├── Weather.razor
    │           │
    │           ├── Agregar
    │           │   ├── Agregar.razor
    │           │   ├── Agregar.razor.css
    │           │   └── Aregar.razor.css
    │           │
    │           ├── Clientes
    │           │   ├── GestionClientes.razor
    │           │   └── GestionClientes.razor.css
    │           │
    │           ├── Conocenos
    │           │   ├── Conocenos.razor
    │           │   └── Conocenos.razor.css
    │           │
    │           ├── Contactos
    │           │   ├── Contactos.razor
    │           │   └── Contactos.razor.css
    │           │
    │           ├── Dashboard
    │           │   ├── Index.razor
    │           │   ├── Index.razor.css
    │           │   └── _Imports.razor
    │           │
    │           ├── Home
    │           │   ├── Home.razor
    │           │   ├── Home.razor.css
    │           │   ├── HomeLayout.razor
    │           │   └── HomeLayout.razor.css
    │           │
    │           ├── Movimientos
    │           │   ├── Movimientos.razor
    │           │   └── Movimientos.razor.css
    │           │
    │           ├── PanelNavegacion
    │           │   ├── PanelNavegacion.razor
    │           │   └── PanelNavegacion.razor.css
    │           │
    │           ├── Renta
    │           │   ├── DevolverRenta.razor
    │           │   ├── DevolverRenta.razor.css
    │           │   ├── RentarVehiculo.razor
    │           │   ├── RentarVehiculo.razor.css
    │           │   ├── RentasActivas.razor
    │           │   └── RentasActivas.razor.css
    │           │
    │           └── Vehiculos
    │               ├── ListaVehiculos
    │               │   ├── ListaVehiculos.razor
    │               │   └── ListaVehiculos.razor.css
    │               │
    │               ├── VehiculoCategoria
    │               │   ├── VehiculoCategoria.razor
    │               │   └── VehiculoCategoria.razor.css
    │               │
    │               └── VerVehiculos
    │                   ├── VerVehiculos.razor
    │                   └── VerVehiculos.razor.css
    │
    └── wwwroot
        ├── app.css
        ├── favicon.png
        │
        ├── bootstrap
        │   ├── bootstrap.min.css
        │   └── bootstrap.min.css.map
        │
        ├── img
        │   └── logo.jpg
        │
        ├── js
        │   └── Imprimir.js
        │
        └── Vehicle_Images
            └── Toyota_Corolla_2015.jpg
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
