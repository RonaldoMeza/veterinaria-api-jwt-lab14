# Entregables — Laboratorio 14: Autenticación JWT

## Sistema veterinario y roles

La API gestiona propietarios y mascotas de una veterinaria. La autenticación JWT permite identificar al usuario sin enviar su contraseña en cada solicitud.

- **Recepcionista:** registra y administra propietarios y mascotas.
- **Veterinario:** consulta propietarios y mascotas, y actualiza la información de las mascotas.

El registro público asigna siempre `Recepcionista`; el cliente nunca elige su rol. Las contraseñas se almacenan exclusivamente como hashes BCrypt.

## Repositorio

- URL: **pendiente de crear/configurar el repositorio del Laboratorio 14**.

## Matriz de permisos

| Endpoint | Sin token | Recepcionista | Veterinario |
|---|---:|---:|---:|
| `POST /api/auth/registro` | Sí | Sí | Sí |
| `POST /api/auth/login` | Sí | Sí | Sí |
| `GET /api/auth/perfil` | 401 | Sí | Sí |
| `GET /api/mascotas` | Sí | Sí | Sí |
| `GET /api/mascotas/{id}` | 401 | Sí | Sí |
| `POST /api/mascotas` | 401 | Sí | 403 |
| `PUT /api/mascotas/{id}` | 401 | Sí | Sí |
| `DELETE /api/mascotas/{id}` | 401 | Sí | 403 |
| `GET /api/propietarios` | 401 | Sí | Sí |
| `GET /api/propietarios/{id}` | 401 | Sí | Sí |
| `GET /api/propietarios/{id}/mascotas` | 401 | Sí | Sí |
| `POST /api/propietarios` | 401 | Sí | 403 |
| `PUT /api/propietarios/{id}` | 401 | Sí | 403 |
| `DELETE /api/propietarios/{id}` | 401 | Sí | 403 |

## Guía mínima de pruebas en Swagger

Usar la clave académica local `Admin123!` para los usuarios iniciales. No capturar la clave JWT configurada en User Secrets.

Verificación técnica realizada el 18/11/2026: Swagger y los endpoints devolvieron correctamente 200, 201, 204, 400, 401, 403, 404 y 409. Los registros temporales de propietario y mascota fueron eliminados al finalizar; se conservó únicamente un usuario de verificación creado mediante el endpoint de registro. Las capturas definitivas siguen pendientes y deben realizarse manualmente en Swagger.

| # | Prueba | Datos | Resultado esperado | Captura sugerida |
|---:|---|---|---|---|
| 1 | Swagger y endpoint público | `GET /api/mascotas` sin token | 200 | `01-publico-200.png` |
| 2 | DTO inválido | Registro con clave `123` | 400 | `02-validacion-400.png` |
| 3 | Registro | `nuevo@veterinaria.com`, `Prueba123!` | 201 | `03-registro-201.png` |
| 4 | Correo repetido | Repetir prueba 3 | 409 | `04-correo-409.png` |
| 5 | Login inválido | Correo correcto y clave incorrecta | 401 | `05-login-401.png` |
| 6 | Login Recepcionista | `recepcionista@veterinaria.com`, `Admin123!` | 200 y token | `06-login-token-200.png` |
| 7 | Perfil | Autorizar con token Recepcionista y ejecutar `GET /api/auth/perfil` | 200 con correo y rol | `07-perfil-200.png` |
| 8 | Sin token | Quitar autorización y ejecutar `GET /api/propietarios` | 401 | `08-sin-token-401.png` |
| 9 | Recurso inexistente | Con token: `GET /api/propietarios/9999` | 404 | `09-no-encontrado-404.png` |
| 10 | Crear propietario temporal | Con Recepcionista: `POST /api/propietarios` con DNI nuevo | 201 | `10-propietario-201.png` |
| 11 | Actualizar propietario temporal | `PUT` usando el id creado | 204 | `11-propietario-204.png` |
| 12 | Login Veterinario | `veterinario@veterinaria.com`, `Admin123!` | 200 y token | `12-veterinario-200.png` |
| 13 | Rol insuficiente | Con Veterinario: `DELETE /api/propietarios/{idTemporal}` | 403 | `13-rol-403.png` |
| 14 | Eliminar temporal | Con Recepcionista: `DELETE /api/propietarios/{idTemporal}` | 204 | `14-delete-204.png` |

JSON para la prueba 10:

```json
{
  "nombres": "Elena",
  "apellidos": "Prueba Local",
  "dni": "80123456",
  "telefono": "999888777",
  "correo": "elena.prueba@example.com",
  "direccion": "Av. Pruebas 123"
}
```

## Captura del token decodificado

1. Ejecutar el login y copiar únicamente el valor de `token`.
2. Abrir `https://jwt.io` y pegar el token en **Encoded**.
3. Capturar el payload mostrando `sub`, `email`, el claim de rol, `iss`, `aud` y `exp`.
4. No escribir ni mostrar `Jwt:Clave` en jwt.io ni en la captura. El token de prueba puede descartarse reiniciando/cambiando el secreto después de la evidencia.

## Sustentación de seguridad

### 1. Diferencia entre 401 y 403

Un 401 ocurre cuando la solicitud no presenta un token válido, por ejemplo `GET /api/propietarios` sin usar Authorize. Un 403 ocurre cuando el token sí es válido, pero su rol no tiene permiso, por ejemplo un Veterinario intentando eliminar un propietario.

### 2. Claims incluidos

Incluí `sub` para identificar al usuario, `email` para reconocer su cuenta y `role` para aplicar autorización. Nunca incluiría contraseñas, hashes, la clave de firma, datos bancarios ni información sensible en un JWT, porque su payload puede decodificarse.

### 3. Riesgo de guardar contraseñas en texto

Si la base de datos fuera comprometida, las contraseñas quedarían expuestas inmediatamente y podrían reutilizarse contra otros servicios. BCrypt almacena un hash con salt y costo de cálculo, por lo que la contraseña original no se guarda ni se recupera directamente.

### 4. Por qué el cliente no asigna el rol

Si el registro aceptara el rol enviado por el cliente, una persona podría registrarse con privilegios superiores. El servidor asigna `Recepcionista` y los cambios de rol se realizan por un proceso administrativo controlado. El token firmado transporta ese rol y cualquier alteración invalida su firma.

## Observaciones

1. Implementé JWT respetando las cuatro capas y mantuve los controladores dependientes de servicios y DTOs.
2. Comprobé que la contraseña se transforma con BCrypt antes de persistirse y que el rol no forma parte del DTO de registro.
3. Separé autenticación (identidad) de autorización (permisos por rol) mediante middleware y atributos.

## Conclusiones

1. Aprendí a generar y validar tokens con emisor, audiencia, expiración y firma.
2. Comprendí la diferencia práctica entre una respuesta 401 y una respuesta 403.
3. Apliqué almacenamiento seguro de contraseñas y autorización basada en claims sin romper la arquitectura existente.

## Checklist

- [x] Entidad Usuario y tabla SQL con correo único, hash y rol.
- [x] DTOs de registro, login, token y perfil.
- [x] Repositorio y servicio de autenticación.
- [x] BCrypt y JWT con claims mínimos.
- [x] Roles Recepcionista y Veterinario.
- [x] Swagger configurado con Bearer Authorize.
- [x] Compilación sin errores ni advertencias.
- [x] Script SQL ejecutado y usuarios verificados en SQL Server local.
- [x] Secreto JWT configurado mediante User Secrets.
- [x] Verificación automática de códigos 200, 201, 204, 400, 401, 403, 404 y 409.
- [ ] Pruebas Swagger y capturas realizadas manualmente.
- [ ] Repositorio GitHub del Laboratorio 14 configurado y publicado.
