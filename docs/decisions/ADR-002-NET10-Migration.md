# ADR-002: Migrate Runtime from .NET 8 to .NET 10

## Status
✅ Accepted

## Context
The project initially selected **.NET 8 (LTS)** as the runtime in [ADR-001: Technology Stack Selection](ADR-001-Technology-Stack-Selection.md). However, .NET 10 has been released and offers several advantages for this project:

- .NET 10 is the latest stable release with new language features and performance improvements
- Provides better alignment with modern C# development practices
- Offers improved tooling and IDE support
- Allows exploration of cutting-edge .NET capabilities
- The project's goal includes technology exploration, making newer versions preferable

The previous decision to use .NET 8 was made when it was the latest LTS version. With .NET 10 now available and stable, migrating provides better long-term benefits for a learning/exploration project.

## Decision
We will **migrate the project from .NET 8 to .NET 10** as the primary runtime.

### Updated Technology Stack
| Component | Technology |
|-----------|------------|
| **Runtime** | .NET 10 |
| **Language** | C# 12 |
| **UI Framework** | WPF with XAML |
| **Architecture** | Layered: Domain → Application → Infrastructure → WPF |
| **Pattern** | MVVM (Model-View-ViewModel) |

## Alternatives Considered

### 1. Stay on .NET 8 (LTS)
**Pros:**
- Long-term support guarantees
- More stable and battle-tested
- Less risk of breaking changes
- Better compatibility with existing libraries

**Cons:**
- Misses out on latest language features (C# 12 improvements in .NET 10)
- Less educational value for learning modern .NET
- Doesn't align with project's exploration goals
- **Decision**: Rejected - exploration and learning take priority over stability for this project

### 2. Use .NET 9
**Pros:**
- Newer than .NET 8
- Some improvements over .NET 8

**Cons:**
- Not the latest version
- .NET 10 offers more features and improvements
- **Decision**: Rejected - .NET 10 is preferable for maximum learning value

### 3. Use .NET 10 (CHOSEN)
**Pros:**
- Latest stable .NET release
- Full C# 12 support with latest features
- Performance improvements over .NET 8
- Better tooling and IDE integration
- Aligns with project's technology exploration goals
- Future-proof for upcoming development
- Allows learning most current .NET practices

**Cons:**
- Slightly less mature than LTS versions
- Potential for minor breaking changes from .NET 8
- Some third-party libraries may need time to catch up
- **Decision**: Accepted - benefits outweigh risks for this exploration project

## Consequences

### Positive
- ✅ Access to latest C# 12 features and improvements
- ✅ Better performance and optimization
- ✅ Enhanced developer experience with improved tooling
- ✅ Future-proof foundation for the project
- ✅ Aligns with project's learning and exploration objectives
- ✅ Consistent with AGENTS.md v1.1 which updated preferred runtime to .NET 10

### Negative
- ❌ May require updates to some dependencies
- ❌ Potential for minor compatibility issues
- ❌ Less long-term support compared to LTS versions (though this is acceptable for a learning project)
- ❌ Team members need to install .NET 10 SDK

### Migration Plan
1. Update all `.csproj` files to target `net10.0`
2. Verify all dependencies support .NET 10
3. Test domain, application, and WPF layers
4. Update CI/CD pipelines if applicable
5. Ensure all tests pass with new runtime

### Rollback Strategy
If significant issues arise, we can revert to .NET 8 by:
1. Reverting `.csproj` file changes
2. Re-targeting to `net8.0`
3. This is a low-risk change as the domain layer remains framework-agnostic

## Related
- [ADR-001: Technology Stack Selection](ADR-001-Technology-Stack-Selection.md) - Original stack decision (superseded by this ADR for runtime)
- [AGENTS.md](../../AGENTS.md) - Global configuration (v1.1 updated runtime preference to .NET 10)
- [README.md](../../README.md) - Project overview and goals
