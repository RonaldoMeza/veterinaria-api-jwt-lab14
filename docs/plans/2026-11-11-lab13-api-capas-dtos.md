# Laboratorio 13 API en Capas Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Transformar la API de veterinaria del Laboratorio 12 en una solución de cuatro capas que exponga solo DTOs mediante AutoMapper.

**Architecture:** La solución contendrá Entidades (modelo Scaffold y DTOs), Datos (DbContext y repositorios), Negocio (servicios, excepciones y MappingProfile) y Api (controladores y composición). Las referencias serán Api → Negocio → Datos → Entidades; los controladores dependerán exclusivamente de interfaces de servicio.

**Tech Stack:** .NET 10, ASP.NET Core Web API, EF Core 10 SQL Server, AutoMapper 16, Swagger.

---

### Task 1: Crear la solución y cuatro proyectos

**Files:**
- Create: `Veterinaria.sln`
- Create: `Veterinaria.Entidades/`
- Create: `Veterinaria.Datos/`
- Create: `Veterinaria.Negocio/`
- Create: `Veterinaria.Api/`

**Step 1:** Crear los cuatro proyectos y las referencias Api → Negocio → Datos → Entidades.

**Step 2:** Mover el proyecto web existente a `Veterinaria.Api` y conservar `database/` y `docs/` en la raíz.

**Step 3:** Ejecutar `dotnet build Veterinaria.sln`.

### Task 2: Separar entidades y datos

**Files:**
- Create: `Veterinaria.Entidades/Entidades/*.cs`
- Create: `Veterinaria.Entidades/DTOs/*.cs`
- Create: `Veterinaria.Datos/Models/VeterinariaContext.cs`
- Create: `Veterinaria.Datos/Repositorios/*.cs`

**Step 1:** Mover las entidades Scaffold a Entidades, eliminar `JsonIgnore` y agregar DTOs de lectura, creación y actualización.

**Step 2:** Ubicar el DbContext Scaffold en Datos y crear los repositorios e interfaces asíncronos.

**Step 3:** Compilar la solución.

### Task 3: Implementar negocio y mapeos

**Files:**
- Create: `Veterinaria.Negocio/Servicios/*.cs`
- Create: `Veterinaria.Negocio/Excepciones/ReglaNegocioException.cs`
- Create: `Veterinaria.Negocio/MappingProfile.cs`

**Step 1:** Implementar `CreateMap` entidad → DTO y DTO de entrada → entidad, incluyendo `ForMember(... MapFrom(...))` para el nombre del propietario en MascotaDto.

**Step 2:** Implementar servicios e interfaces con reglas de DNI duplicado y eliminación con mascotas mediante `ReglaNegocioException`.

**Step 3:** Compilar la solución.

### Task 4: Adaptar API y comprobar contratos

**Files:**
- Create: `Veterinaria.Api/Controllers/*.cs`
- Modify: `Veterinaria.Api/Program.cs`
- Modify: `Veterinaria.Api/veterinaria-api.csproj`

**Step 1:** Registrar DbContext, repositorios, servicios y AutoMapper por DI.

**Step 2:** Reescribir controladores usando exclusivamente DTOs e interfaces de servicio.

**Step 3:** Ejecutar compilación y comprobar Swagger y los endpoints GET locales.

### Task 5: Documentar y publicar la rama

**Files:**
- Create: `ENTREGABLES_LAB13.md`

**Step 1:** Documentar el sistema, mapeos, pruebas y capturas solicitadas.

**Step 2:** Ejecutar `dotnet build Veterinaria.sln` y verificación de secretos.

**Step 3:** Confirmar commit y push de `lab13-api-capas-dtos`, sin tocar `main`.
