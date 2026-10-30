# ADR-001: Technology Stack Selection

## Status
✅ Accepted

## Context
The project needs a technology stack for building a mind-mapping application. The primary goal is to create a **practical tool** while also serving as a **learning playground** for exploring .NET technologies, architectural approaches, and UI patterns.

Key requirements:
- Run on Windows desktop (initial target platform)
- Support incremental development and experimentation
- Allow exploration of modern .NET features
- Keep the domain layer clean and framework-agnostic
- Be fun to work with (explicit project goal: "for fun" / "dla zabawy")

## Decision
We will build a **WPF (Windows Presentation Foundation) desktop application using C#** on the .NET platform.

### Technology Stack
| Component | Technology |
|-----------|------------|
| **Runtime** | .NET 8 (LTS) |
| **Language** | C# 12 |
| **UI Framework** | WPF with XAML |
| **Architecture** | Layered: Domain → Application → Infrastructure → WPF |
| **Pattern** | MVVM (Model-View-ViewModel) |

### Starting Point
Development will begin with:
1. **Domain Project** (`Cartamentis.Domain`) - Core business logic for mind maps, topics, and connections
2. **Test Project** (`Cartamentis.Domain.Tests`) - Unit tests for the domain layer using xUnit

## Alternatives Considered

### 1. WPF + C# (CHOSEN)
**Pros:**
- Native Windows desktop experience
- Mature framework with excellent tooling
- Strong separation of concerns via XAML
- Rich UI capabilities for mind map visualization
- Good for learning .NET ecosystem
- Fun to work with (matches "for fun" requirement)

**Cons:**
- Windows-only (no cross-platform support)
- Steeper learning curve for XAML
- Less "modern" than newer frameworks
- Requires Windows development environment

### 2. Avalonia UI + C#
**Pros:**
- Cross-platform (Windows, macOS, Linux)
- Modern XAML-based framework
- Similar to WPF (easier migration)
- Open-source with active community

**Cons:**
- Less mature than WPF
- Smaller ecosystem and fewer resources
- Additional complexity for cross-platform support
- **Decision**: Rejected for now - we want to focus on learning WPF first

### 3. Blazor (WebAssembly or Server)
**Pros:**
- Web-based, runs in browser
- Uses C# and .NET
- Modern web technologies
- Cross-platform by nature

**Cons:**
- Not a desktop application
- Different UI paradigm (HTML/CSS vs XAML)
- Less suitable for complex canvas-based visualization
- **Decision**: Rejected - project explicitly targets desktop

### 4. WinForms + C#
**Pros:**
- Simple and familiar
- Windows-only (matches target platform)
- Good performance

**Cons:**
- Outdated technology
- Poor separation of concerns
- Limited UI capabilities for mind maps
- Not fun or educational for modern development
- **Decision**: Rejected - not suitable for learning/exploration goals

### 5. Console Application
**Pros:**
- Simplest possible approach
- Fast to develop
- Good for focusing on domain logic

**Cons:**
- No visual representation of mind maps
- Doesn't meet the "mind mapping application" requirement
- **Decision**: Rejected - visualization is core to the project

## Consequences

### Positive
- ✅ Clear, focused technology stack
- ✅ Strong foundation for learning WPF and .NET
- ✅ Domain layer remains framework-agnostic (can be reused if UI changes)
- ✅ Good tooling and IDE support (Visual Studio, Rider)
- ✅ Fun and educational development experience
- ✅ Matches project's "for fun" philosophy

### Negative
- ❌ Windows-only application (for now)
- ❌ Cannot run on macOS or Linux without Windows VM
- ❌ WPF has a learning curve (XAML, data binding, MVVM)
- ❌ May need to migrate to cross-platform solution in the future
- ❌ Limited mobile/web deployment options

### Future Considerations
- If cross-platform becomes a requirement, consider migrating to **Avalonia UI** (similar architecture, can reuse domain and application layers)
- The layered architecture allows swapping WPF for another UI framework with minimal impact on domain logic
- Domain layer decisions (C#, .NET) are independent of UI choice and can evolve separately

## Related
- [README.md](../../README.md) - Project overview and goals
- [AGENTS.md](../../AGENTS.md) - Global configuration for AI agents
