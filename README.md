# SkyrimDnDBot 🎲

Bot de Discord para gestión de personajes de D&D ambientado en Skyrim.

## 🔐 Configuración Segura (Importante para GitHub)

Este proyecto usa el patrón de configuración estándar de .NET para manejar secretos de forma segura.

### Archivos de configuración:

| Archivo | Descripción | ¿Se sube a GitHub? |
|---------|-------------|-------------------|
| `appsettings.json` | Configuración base con placeholders | ✅ **SÍ** |
| `appsettings.Development.json` | Tus secretos REALES para desarrollo | ❌ **NO** (en .gitignore) |

### ⚙️ Setup para desarrollo local:

1. **Edita `appsettings.Development.json`** con tus credenciales reales:
```json
{
  "Discord": {
    "Token": "TU_TOKEN_REAL_DE_DISCORD_AQUI",
    "CommandPrefix": "%"
  },
  "Supabase": {
    "Url": "https://tu-proyecto.supabase.co",
    "Key": "tu-anon-key-aqui"
  },
  "Persistence": {
    "Type": "Supabase"
  }
}
```

2. **Para usar almacenamiento en memoria** (sin Supabase):
```json
{
  "Persistence": {
    "Type": "InMemory"
  }
}
```

### 🗄️ Setup de Supabase:

1. Crea una cuenta en [Supabase](https://supabase.com)
2. Crea un nuevo proyecto
3. Ve a SQL Editor y ejecuta:
```sql
CREATE TABLE characters (
    id UUID PRIMARY KEY,
    name TEXT NOT NULL UNIQUE,
    class TEXT NOT NULL,
    level INTEGER NOT NULL DEFAULT 1
);
```
4. Copia tu URL y ANON KEY desde Project Settings → API

### 🔑 Variables de entorno (opcional)

También puedes usar variables de entorno (tienen prioridad sobre appsettings.json):
```
Discord__Token=tu-token
Supabase__Url=tu-url
Supabase__Key=tu-key
```

## 🏗️ Arquitectura (Conceptos SOLID)

Este proyecto demuestra principios de clean architecture:

```
Core/                       → Dominio puro, sin dependencias
  Domain/                   → Entidades de negocio
  Interfaces/               → Contratos (abstracción)
  Configuration/            → Configuración tipada

Application/                → Casos de uso
  Services/                 → Lógica de negocio
  Commands/                 → Comandos de Discord

Infrastructure/             → Implementaciones concretas
  Persistence/              
    InMemoryCharacterRepository.cs     → Implementación en memoria
    SupabaseCharacterRepository.cs     → Implementación en Supabase
  Discord/                  → Integración con Discord
```

### 💡 Principios SOLID aplicados:

**S** - Single Responsibility: Cada clase tiene una única responsabilidad
- `Character`: Solo representa un personaje
- `CharacterService`: Solo maneja lógica de personajes
- `SupabaseCharacterRepository`: Solo maneja persistencia en Supabase

**O** - Open/Closed: Puedes agregar nuevas implementaciones sin modificar código existente
- Agregaste `SupabaseCharacterRepository` sin tocar `CharacterService`
- Puedes agregar `SqlServerRepository` sin tocar nada más

**L** - Liskov Substitution: Cualquier `ICharacterRepository` funciona igual
- `InMemoryCharacterRepository` y `SupabaseCharacterRepository` son intercambiables

**I** - Interface Segregation: Interfaces pequeñas y específicas
- `ICharacterRepository` solo tiene los métodos necesarios
- `ICommandHandler` solo tiene `CanHandle` y `HandleAsync`

**D** - Dependency Inversion: Código depende de abstracciones, no de implementaciones
- `CharacterService` depende de `ICharacterRepository`, no de una implementación específica
- Cambias la implementación en `Program.cs`, no en toda la app

## 🚀 Ejecutar

```bash
dotnet run
```

## 📝 Comandos disponibles

- `%ping` - Verifica que el bot funciona
- `%help` - Muestra ayuda
- `%character create <nombre> <clase> <nivel>` - Crea un personaje
- `%character get <nombre>` - Busca un personaje

---

**Tip de seguridad:** Nunca hagas commit de `appsettings.Development.json`. Si accidentalmente subes secretos a GitHub, revócalos inmediatamente y genera nuevos.
