# Servicios Estudiantes — API Backend

API REST desarrollada en **.NET 8** con arquitectura **Hexagonal**, patrón **CQRS** con MediatR, autenticación **JWT + Refresh Token** y acceso a datos con **Dapper + SQL Server**.

---

## Requisitos previos

| Herramienta | Versión mínima | Descarga |
|---|---|---|
| .NET SDK | 8.0 | https://dotnet.microsoft.com/download/dotnet/8 |
| SQL Server | 2019 / Express | https://www.microsoft.com/sql-server/sql-server-downloads |
| Visual Studio | 2022 (o VS Code) | https://visualstudio.microsoft.com |

---

## Estructura del proyecto

```
back/
├── src/
│   ├── Servicios_Estudiantes.Api            # Capa de presentación (controllers, middlewares)
│   ├── Servicios_Estudiantes.Aplicacion     # Casos de uso (commands, queries, handlers)
│   ├── Servicios_Estudiantes.Dominio        # Entidades y contratos (interfaces)
│   └── Servicios_Estudiantes.Infraestructura# Repositorios Dapper, implementaciones
├── tests/
│   ├── Servicios_Estudiantes.Aplicacion.Tests
│   └── Servicios_Estudiantes.Dominio.Tests
└── Database/
    └── Scripts/                             # Scripts SQL ordenados para setup
```

---

## Configuración de la base de datos

### 1. Ejecutar los scripts SQL en orden

Abrí **SQL Server Management Studio (SSMS)** o cualquier cliente SQL y ejecutá los scripts en este orden:

```
Database/Scripts/00_CreateDatabase.sql   → Crea la base de datos EstudiantesDB
Database/Scripts/01_Tablas.sql          → Crea todas las tablas y FK
Database/Scripts/02_Semillas.sql        → Inserta datos iniciales (programa, profesores, materias, admin)
Database/Scripts/03_Procedimientos.sql  → SPs de inscripción académica
Database/Scripts/04_Usuarios_Auth.sql   → SPs de autenticación y refresh tokens
Database/Scripts/06_SP_CRUD.sql         → SPs CRUD de todas las entidades
Database/Scripts/05_Verificacion.sql    → (Opcional) Verifica que todo quedó correcto
```

### 2. Usuario administrador creado por defecto

| Campo | Valor |
|---|---|
| Usuario | `admin` |
| Contraseña | `Admin123!` |
| Rol | `Administrador` |

---

## Configuración de la aplicación

### Connection string

El archivo `src/Servicios_Estudiantes.Api/appsettings.json` viene preconfigurado para **SQL Server Express con Windows Authentication**:

```json
"ConnectionStrings": {
  "Estudiantes": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=EstudiantesDB;Integrated Security=True;TrustServerCertificate=True;"
}
```

Si usás **SQL Server Authentication** (usuario y contraseña), reemplazá por:

```json
"Estudiantes": "Data Source=localhost;Initial Catalog=EstudiantesDB;User Id=sa;Password=TuPassword;TrustServerCertificate=True;"
```

### JWT Secret Key

En `appsettings.json`, reemplazá la clave por una de al menos 32 caracteres:

```json
"JwtSettings": {
  "SecretKey": "REEMPLAZAR_CON_CLAVE_SECRETA_DE_32_O_MAS_CARACTERES"
}
```

---

## Instalación y ejecución

### Opción A — Visual Studio 2022

1. Abrí el archivo `Servicios_Estudiantes.slnx`
2. Establecé `Servicios_Estudiantes.Api` como proyecto de inicio
3. Presioná **F5** o el botón **Ejecutar**

### Opción B — CLI

```bash
# Restaurar dependencias
dotnet restore

# Ejecutar la API
dotnet run --project src/Servicios_Estudiantes.Api/Servicios_Estudiantes.Api.csproj
```

La API queda disponible en:

```
https://localhost:7090
http://localhost:5090
```

---

## Documentación de la API (Swagger)

Una vez levantada la API, accedé a:

```
https://localhost:7090/swagger
```

Desde Swagger podés probar todos los endpoints. Para los endpoints protegidos, primero hacé login y luego ingresá el token en el botón **Authorize** con el formato:

```
Bearer {token}
```

---

## Endpoints principales

### Autenticación
| Método | Ruta | Descripción |
|---|---|---|
| POST | `/api/auth/login` | Login → devuelve JWT + refresh token |
| POST | `/api/auth/refresh` | Renueva el JWT con el refresh token |
| POST | `/api/auth/logout` | Revoca el refresh token |

### Estudiantes
| Método | Ruta | Acceso |
|---|---|---|
| GET | `/api/estudiantes` | Admin |
| GET | `/api/estudiantes/{id}` | Admin / propio estudiante |
| POST | `/api/estudiantes` | Admin |
| PUT | `/api/estudiantes/{id}` | Admin |
| DELETE | `/api/estudiantes/{id}` | Admin |

### Inscripciones
| Método | Ruta | Descripción |
|---|---|---|
| POST | `/api/inscripciones` | Inscribir materias (máx. 3, sin profesor repetido) |
| GET | `/api/inscripciones/{estudianteId}` | Ver materias inscriptas |
| GET | `/api/inscripciones/{estudianteId}/companeros` | Ver compañeros por materia |
| DELETE | `/api/inscripciones/{estudianteId}` | Cancelar todas las inscripciones |
| DELETE | `/api/inscripciones/{estudianteId}/{materiaId}` | Cancelar una materia específica |

### Otras entidades (Admin)
- `/api/materias` — CRUD de materias
- `/api/profesores` — CRUD de profesores
- `/api/programascredito` — CRUD de programas de crédito
- `/api/usuarios` — CRUD de usuarios

---

## Ejecutar los tests

```bash
dotnet test
```

---

## Solución de problemas frecuentes

**Error de conexión a SQL Server**
- Verificá que el servicio SQL Server esté corriendo en Windows
- Comprobá que el nombre de la instancia en la connection string sea correcto (`SQLEXPRESS` o el tuyo)
- Si usás autenticación de Windows, asegurate de que tu usuario tenga acceso a `EstudiantesDB`

**Puerto en uso**
- Cambiá el puerto en `src/Servicios_Estudiantes.Api/Properties/launchSettings.json`

**Error de certificado SSL en desarrollo**
- Ejecutá: `dotnet dev-certs https --trust`
