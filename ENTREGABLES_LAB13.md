# Entregables — Laboratorio 13

## Sistema veterinario
La API administra propietarios y mascotas de una veterinaria. Un propietario puede tener varias mascotas y cada mascota pertenece a un propietario mediante `IdPropietario`. Las reglas de negocio impiden DNI duplicados y eliminar propietarios con mascotas; ambas producen 409 Conflict.

## Repositorio
Rama del laboratorio: https://github.com/RonaldoMeza/veterinaria-api-lab12/tree/lab13-api-capas-dtos

## Capturas de arquitectura
1. Abrir `Veterinaria.slnx` y capturar los cuatro proyectos: `Veterinaria.Entidades`, `Veterinaria.Datos`, `Veterinaria.Negocio` y `Veterinaria.Api`.
2. Capturar `Veterinaria.Negocio/Negocio.cs`, mostrando `MappingProfile` completo.
3. Capturar las referencias: Api → Negocio → Datos → Entidades.

## Sustentación de MappingProfile
| Mapeo | Origen → Destino | ¿Por qué en ese sentido? | Configuración especial |
|---|---|---|---|
| Propietario → PropietarioDto | Entidad → DTO lectura | La API devuelve un contrato seguro, no la entidad. | Ninguna. |
| PropietarioCrearDto → Propietario | DTO entrada → Entidad | Convierte el JSON de creación para guardarlo. | Ninguna. |
| PropietarioActualizarDto → Propietario | DTO entrada → Entidad existente | Actualiza una entidad rastreada por EF Core. | `Map(origen, destino)` en servicio. |
| Mascota → MascotaDto | Entidad → DTO lectura | Devuelve datos de mascota sin navegación completa. | `ForMember` + `MapFrom` compone `NombrePropietario`. |
| MascotaCrearDto → Mascota | DTO entrada → Entidad | Convierte datos válidos para insertar. | Ninguna. |
| MascotaActualizarDto → Mascota | DTO entrada → Entidad existente | Actualiza la entidad rastreada. | `Map(origen, destino)` en servicio. |

## Pruebas Swagger local
Ejecutar `dotnet run --project Veterinaria.Api` y abrir la URL mostrada con `/swagger`.

| Prueba | Solicitud | Resultado/captura |
|---|---|---|
| Listados, detalle, filtro `?especie=Perro&nombre=Max` y `/api/Propietarios/1/mascotas` | GET | 200; capturar cada ruta. |
| Recurso `9999` | GET | 404. |
| Crear propietario con DNI nuevo | POST `/api/Propietarios` | 201; guardar su id. |
| Repetir DNI existente | POST propietario | 409. |
| DNI de tres dígitos | POST propietario | 400 por Data Annotations. |
| Actualizar el propietario creado | PUT | 204. |
| Eliminar propietario 1 con mascotas | DELETE | 409. |
| Crear mascota para el propietario temporal | POST `/api/Mascotas` | 201; guardar id. |
| Mascota con `idPropietario:9999` o `peso:0` | POST mascota | 400. |
| Actualizar y borrar mascota temporal | PUT / DELETE | 204 y 204. |
| Borrar propietario temporal ya sin mascota | DELETE | 204. |

## Observaciones
1. Reutilicé el modelo generado con Scaffold y separé sus responsabilidades sin modificar la base de datos.
2. Implementé DTOs y AutoMapper para que las entidades no aparezcan en Swagger ni en respuestas HTTP.
3. Coloqué las reglas de negocio en servicios y los controladores solo dependen de interfaces.

## Conclusiones
1. Aprendí a organizar una API REST en cuatro capas con dependencias controladas.
2. Comprobé que AutoMapper reduce código repetitivo al crear y actualizar entidades.
3. Apliqué repositorios y servicios para aislar EF Core de los controladores.

## Checklist
- [x] Cuatro proyectos y referencias en cadena.
- [x] DTOs, repositorios, servicios, AutoMapper y `MappingProfile`.
- [x] Dos reglas 409 con excepción propia.
- [x] Compilación de la solución sin errores ni advertencias.
- [ ] Capturas de proyectos, MappingProfile y pruebas Swagger.
