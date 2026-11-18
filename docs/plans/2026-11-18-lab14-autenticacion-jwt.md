# Laboratorio 14 Autenticación JWT Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Incorporar registro, login, JWT, BCrypt y autorización por roles a la solución veterinaria de cuatro capas.

**Architecture:** Se conservarán las capas de Laboratorio 13. Entidades recibirá Usuario y DTOs; Datos el mapeo y repositorio; Negocio el servicio de autenticación y generación JWT; API configurará JwtBearer, Swagger Bearer, AuthController y permisos sobre los controladores existentes.

**Tech Stack:** .NET 10, ASP.NET Core JWT Bearer 10, EF Core 10, BCrypt.Net-Next, System.IdentityModel.Tokens.Jwt 8, SQL Server, Swagger.

---

### Task 1: Modelo y script de usuarios
**Files:** `database/veterinaria_db.sql`, `Veterinaria.Entidades/Entidades/Usuario.cs`, DTOs de autenticación, `Veterinaria.Datos/Context/VeterinariaContext.cs`.
1. Crear tabla Usuarios con correo único, hash y rol.
2. Incorporar Usuario y DbSet/mapeo EF.
3. Compilar.

### Task 2: Repositorio y servicio de autenticación
**Files:** repositorio de usuarios, `JwtOpciones`, `IAuthServicio`, `AuthServicio`.
1. Instalar BCrypt y JWT 8 en Negocio.
2. Implementar registro con rol Recepcionista y correo único.
3. Implementar login, BCrypt.Verify y token con sub, email y rol.
4. Compilar.

### Task 3: API, Swagger y permisos
**Files:** `AuthController.cs`, controladores existentes, `Program.cs`, `appsettings.json`.
1. Instalar JwtBearer 10.
2. Configurar validación de emisor, audiencia, expiración y firma.
3. Configurar Swagger Bearer y botón Authorize.
4. Aplicar AllowAnonymous, Authorize y roles según matriz.
5. Compilar.

### Task 4: Evidencia y verificación local
**Files:** `ENTREGABLES_LAB14.md`.
1. Documentar matriz, pruebas, token, sustentación, observaciones y conclusiones.
2. Solicitar al usuario ejecutar el script y configurar User Secrets antes de pruebas reales.
3. Probar respuestas 200, 201, 204, 400, 401, 403, 404 y 409 sin inventar evidencia.
4. Configurar el nuevo remoto y publicar `main` solo cuando el usuario proporcione la URL.
