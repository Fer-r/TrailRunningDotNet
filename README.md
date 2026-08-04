# Racehub (.NET 10 + React SPA)

Este proyecto es el trabajo final del curso de programación .NET, en el que se ha migrado el backend original de una plataforma de Trail Running (construido en PHP/Symfony) a una moderna **Web API en ASP.NET Core 10**.

## 🏗️ Origen del Proyecto y Repositorios Originales

El proyecto se basa en una arquitectura dividida donde se ha reemplazado el backend original por completo:

*   **Frontend (React Vite):** Proviene del repositorio original [Fer-r/trailrunning](https://github.com/Fer-r/trailrunning). Es una Single Page Application construida con React 18, React Router, y TailwindCSS.
*   **Backend (Reemplazado):** El backend original en Symfony alojado en [Jfranbm04/racehub](https://github.com/Jfranbm04/racehub) ha sido sustituido al 100% por una nueva Web API en **.NET 10 (C# 14)** usando **Entity Framework Core 10** y SQLite.

## 🚀 Arquitectura y Tecnologías

El repositorio actual se ha reestructurado como un **monorepo**, donde coexisten ambas partes bajo la carpeta `src/`:

*   **`src/RacehubApi/`**: Backend ASP.NET Core (.NET 10). Proveerá autenticación mediante **JWT Bearer**, validación de contraseñas con **BCrypt**, documentación con **Swagger/OpenAPI**, e integrará vistas **Razor Pages** para el formulario de registro (`/register`).
*   **`src/RacehubWeb/`**: Frontend React SPA. Mantiene su código original sin alteraciones significativas de componentes, enlazándose directamente al nuevo servidor .NET.

## ⚙️ Requisitos Previos

*   [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
*   [Node.js](https://nodejs.org/) (v18+)
*   NPM o Yarn

## 🛠️ Cómo Iniciar el Proyecto (Desarrollo Local)

Se ha provisto un `Makefile` en la raíz del proyecto para facilitar las tareas más comunes de desarrollo:

### 1. Inicialización e Instalación
Para instalar los paquetes NuGet del backend y los node_modules del frontend, y para aplicar las migraciones de SQLite iniciales:
```bash
make init
```
*(Alternativa manual: Ejecutar `dotnet restore` en `src/RacehubApi` y `npm install` en `src/RacehubWeb`)*

### 2. Levantar los Servidores (Frontend y Backend a la vez)
```bash
make up
```
Esto arrancará:
*   La API de .NET en `http://localhost:5000`
*   El servidor de desarrollo de React en `http://localhost:5173`

*(Alternativa manual: Ejecutar `dotnet run` en el backend y `npm run dev` en el frontend en consolas separadas).*

### Otros comandos útiles:
*   `make db`: Aplica migraciones de EF Core pendientes en SQLite.
*   `make migration NAME=X`: Crea una nueva migración con el nombre `X`.
*   `make clean`: Borra la carpeta de `node_modules` y las carpetas de binarios `bin/obj` de .NET.
*   `make test`: Ejecuta la suite de pruebas unitarias/integración (xUnit).

## 📄 Licencia y Autores

Proyecto de migración realizado como **Proyecto Final** aplicando buenas prácticas de C# 14, inyección de dependencias, DTOs inmutables (records), y optimización de EF Core.
