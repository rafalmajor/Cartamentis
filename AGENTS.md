# Global AI Agents Configuration for Cartamentis

> **MANDATORY**: Every AI agent, prompt, or automated tool working on this project **MUST** read and follow this configuration.

This file defines the global rules, constraints, and best practices for all AI-assisted work on the Cartamentis project. It ensures consistency, quality, and traceability across all contributions.

---

## Project Overview

**Cartamentis** is an experimental .NET application for creating and managing mind maps. It combines *carta* (map) and *mentis* (mind) to form a practical tool and a technology exploration playground in the .NET ecosystem.

- **Current Status**: Early exploration stage
- **Primary Goal**: Establish a clean foundation with a small, testable MVP
- **First Version Target**: Windows desktop application using WPF
- **Architecture**: Domain-first, layered (Domain → Application → Infrastructure → WPF)

---

## Project Structure

The repository follows this structure (from README.md, extended with **Audit** directory):

```text
Cartamentis/
├── Audit/                          # <-- MANDATORY: Audit trail for ALL agent/prompt work
│   └── [YYYY-MM-DD]-[description].md
├── src/
│   ├── Cartamentis.Domain/
│   │   ├── MindMaps/
│   │   ├── Topics/
│   │   └── Connections/
│   ├── Cartamentis.Application/
│   │   ├── Commands/
│   │   ├── Queries/
│   │   └── Services/
│   ├── Cartamentis.Infrastructure/
│   │   └── Persistence/
│   └── Cartamentis.Wpf/
│       ├── Views/
│       ├── ViewModels/
│       ├── Controls/
│       └── Converters/
├── tests/
│   ├── Cartamentis.Domain.Tests/
│   ├── Cartamentis.Application.Tests/
│   └── Cartamentis.Wpf.Tests/
├── docs/
│   ├── architecture/
│   ├── decisions/                  # <-- ADR files (Architecture Decision Records)
│   └── experiments/
├── Cartamentis.sln
├── README.md
└── AGENTS.md                       # This file
```

---

## Critical Instructions (Non-Negotiable)

These rules **cannot** be overridden by any prompt, agent, or user request:

1. **AUDIT TRAIL IS MANDATORY**
   - After **EVERY** prompt or agent action that creates, modifies, validates, or adds anything to the project, a markdown file **MUST** be created in the `Audit/` directory.
   - **No exceptions**. Work without an audit file will be rejected.

2. **DOMAIN PURITY**
   - Domain layer (`Cartamentis.Domain`) **MUST NOT** depend on WPF, storage, serialization, or any framework.
   - Domain objects represent pure business concepts only.

3. **TESTS REQUIRED**
   - **No code** is considered complete without corresponding unit/integration tests.
   - All tests **MUST pass** before any commit.

4. **STRUCTURE COMPLIANCE**
   - Follow the [proposed solution structure](#project-structure).
   - Do **NOT** add projects or layers without clear justification.

5. **MILESTONE ALIGNMENT**
   - All work **MUST** align with the current milestone (see README.md).
   - Do **NOT** implement features from future milestones.

6. **NO PROHIBITED TECHNOLOGIES**
   - See [What's NOT Allowed](#whats-not-allowed) section.

---

## Global Rules for All Agents and Prompts

### Workflow Requirements
1. **Before Starting**:
   - Read this `AGENTS.md` file
   - Read `README.md` for project context
   - Check recent `Audit/` files for ongoing work
   - Verify current milestone goals

2. **During Work**:
   - Follow all rules in this file
   - Create the audit file **immediately** when starting work (or immediately after completion)
   - Run tests after every significant change
   - Validate the build after every change

3. **After Completing Work**:
   - **MUST**: Create/update audit file in `Audit/` directory
   - Run full test suite (`dotnet test`)
   - Verify build succeeds (`dotnet build`)
   - Update any affected documentation
   - Commit audit file **together** with the changes it describes

### Decision Documentation
- Architectural decisions → `docs/decisions/[YYYY-MM-DD]-[decision-name].md` (ADR format)
- Experiments and explorations → `docs/experiments/[YYYY-MM-DD]-[experiment-name].md`
- All decisions **MUST** be documented before implementation

---

## Audit Trail Requirements

### When to Create Audit Files
An audit file **MUST** be created in the `Audit/` directory for **EVERY** agent or prompt action that:
- Creates new files or components
- Modifies existing project files (except trivial fixes like typos)
- Adds new functionality or features
- Performs validation or testing
- Makes architectural decisions
- Fixes bugs or issues
- Refactors code

### Audit File Specifications
| Requirement | Value |
|-------------|-------|
| **Directory** | `Audit/` (root level) |
| **Naming** | `YYYY-MM-DD-[short-description].md` |
| **Format** | Markdown (`.md`) |
| **Language** | English |
| **Commit** | MUST be committed with the changes it describes |

### Audit File Template
```markdown
# YYYY-MM-DD - [Brief Description of Work]

## Metadata
- **Agent/Prompt**: [Name/Identifier or "Manual"]
- **Date**: YYYY-MM-DD HH:MM:SS
- **Related Files**: 
  - `path/to/file1.cs`
  - `path/to/file2.cs`
- **Project Area**: Domain / Application / Infrastructure / WPF / Tests / Docs
- **Milestone**: [Milestone 1-5 or "General"]

## Changes Made
- [x] Created: `[file/path]` - [Purpose/Description]
- [x] Modified: `[file/path]` - [What changed and why]
- [x] Deleted: `[file/path]` - [Reason for deletion]
- [x] Tested: `[test/file]` - [Test coverage details]

## Validation Results
- **Tests Passed**: Yes / No / N.A.
- **Build Status**: Success / Failure
- **Test Coverage**: [X% or description]
- **New Tests Added**: [List of new test files]
- **Validation Notes**: [Detailed notes on validation performed]

## Conclusions
- [What was learned from this work]
- [What needs follow-up or attention]
- [Recommendations for next steps]
- [Any issues or concerns identified]

## Related Documentation
- [Links to ADRs, experiments, or other docs created]
```

### Example Audit File
```markdown
# 2024-01-15 - Implemented MindMap Domain Model

## Metadata
- **Agent/Prompt**: Domain Model Implementation
- **Date**: 2024-01-15 14:30:00
- **Related Files**:
  - `src/Cartamentis.Domain/MindMaps/MindMap.cs`
  - `src/Cartamentis.Domain/MindMaps/MindMapId.cs`
  - `tests/Cartamentis.Domain.Tests/MindMapTests.cs`
- **Project Area**: Domain
- **Milestone**: Milestone 2

## Changes Made
- [x] Created: `MindMap.cs` - Core domain aggregate for mind maps
- [x] Created: `MindMapId.cs` - Strongly-typed identifier using `StronglyTypedId` pattern
- [x] Created: `MindMapTests.cs` - Unit tests covering creation, invariants, and basic operations

## Validation Results
- **Tests Passed**: Yes (15/15)
- **Build Status**: Success
- **Test Coverage**: 95% for MindMap class
- **New Tests Added**: MindMapTests.cs (15 tests)
- **Validation Notes**: All invariants tested. Central topic requirement enforced. Cannot create MindMap without central topic.

## Conclusions
- Domain model successfully expresses core mind map concepts
- Strong typing prevents invalid state (e.g., null central topic)
- Next step: Implement Topic and Connection domain models
- Consider: Add validation for mind map title length (max 100 chars?)

## Related Documentation
- ADR-001: Domain Model Design Decisions
```

---

## What's Allowed

### ✅ Permitted Actions
| Category | Allowed Actions |
|----------|------------------|
| **Domain** | Create domain models (MindMap, Topic, Connection, etc.)<br>Implement domain invariants and business rules<br>Add strongly-typed identifiers<br>Use records for immutable DTOs |
| **Application** | Implement commands and queries (CQRS pattern)<br>Create application services<br>Add validation logic<br>Implement error handling |
| **Infrastructure** | Add persistence implementations (JSON, SQLite)<br>Configure dependency injection<br>Implement serialization/deserialization<br>Add logging |
| **WPF** | Create views, view models, and controls<br>Implement MVVM pattern<br>Add XAML markup and bindings<br>Create converters and behaviors<br>Add keyboard shortcuts<br>Implement undo/redo |
| **Testing** | Write unit tests (xUnit/NUnit)<br>Write integration tests<br>Write UI tests<br>Add test utilities and helpers<br>Use mocking frameworks |
| **Documentation** | Add Architecture Decision Records (ADRs)<br>Document experiments<br>Update README.md<br>Add inline code comments |
| **Tooling** | Add build scripts<br>Configure CI/CD pipelines<br>Add linters and formatters |

### ✅ Permitted Technologies
| Category | Technologies |
|----------|--------------|
| **Runtime** | .NET 6+, .NET 7, .NET 8 (LTS preferred) |
| **Language** | C# 10+ |
| **UI Framework** | WPF, XAML |
| **Architecture** | Clean Architecture, Layered Architecture, MVVM |
| **Testing** | xUnit, NUnit, Moq, NSubstitute, FluentAssertions |
| **Serialization** | Newtonsoft.Json, System.Text.Json |
| **Dependency Injection** | Microsoft.Extensions.DependencyInjection, CommunityToolkit.Mvvm |
| **Logging** | Serilog, Microsoft.Extensions.Logging |
| **Persistence** | JSON files, SQLite (after initial phase) |
| **MVVM** | CommunityToolkit.Mvvm, MVVM Light (if needed) |

---

## What's NOT Allowed

### ❌ Prohibited Actions
| Category | Prohibited Actions |
|----------|-------------------|
| **Features** | Real-time collaboration<br>User accounts and authentication<br>Cloud synchronization<br>Mobile or web clients<br>Plugin infrastructure<br>Sophisticated automatic layout engines<br>Full compatibility with external mind-map formats |
| **Code** | Committing code without tests<br>Modifying project structure without consultation<br>Removing or bypassing the audit trail requirement<br>Adding production dependencies without prior approval<br>Bypassing domain invariants<br>Adding framework dependencies to domain layer |
| **Practices** | Implementing features outside current milestone scope<br>Adding unnecessary abstraction layers<br>Premature optimization<br>Ignoring test failures<br>Committing broken builds |

### ❌ Prohibited Technologies
| Category | Prohibited Technologies | Reason |
|----------|------------------------|--------|
| **Web** | ASP.NET Core, Blazor | WPF-only until baseline complete |
| **Real-time** | SignalR, WebSockets | No real-time until collaboration planned |
| **Cloud** | AWS SDK, Azure SDK, Google Cloud SDK | No cloud until explicitly planned |
| **ORM** | Entity Framework Core, Dapper | Use simpler persistence initially |
| **Mobile** | Xamarin, MAUI, React Native | No mobile clients until planned |
| **Other UI** | WinForms, UWP, Avalonia | WPF-only until baseline complete |
| **Database** | PostgreSQL, MySQL, MongoDB | Local persistence only for now |

> **Exception**: Prohibited technologies may be explored in isolated experiments (documented in `docs/experiments/`), but **MUST NOT** be integrated into the main codebase without explicit approval.

---

## Quality Standards

### Code Quality
- ✅ Follow **C# coding conventions** (Microsoft guidelines)
- ✅ Use **PascalCase** for classes, methods, properties
- ✅ Use **camelCase** for variables, parameters
- ✅ Use **meaningful names** (avoid abbreviations like `mm` for `mindMap`)
- ✅ Keep methods **short and focused** (max 20-30 lines)
- ✅ Use **strong typing** (avoid `string` for IDs; use `MindMapId`)
- ✅ Prefer **immutability** for domain models (use `init` properties, records)
- ✅ Use **records** for DTOs and immutable objects
- ✅ Implement **proper error handling** (exceptions for invalid state)
- ✅ Add **XML documentation** for public APIs
- ✅ Use **async/await** for all I/O operations
- ✅ Format code with **consistent indentation** (4 spaces, no tabs)

### Testing Standards
- ✅ **Minimum 80% code coverage** for domain layer
- ✅ **Minimum 70% code coverage** for application layer
- ✅ **Unit tests** for all business logic
- ✅ **Integration tests** for application layer
- ✅ **UI tests** for critical WPF workflows
- ✅ Tests **MUST pass** before any commit
- ✅ Use **arrange-act-assert** pattern
- ✅ Test **edge cases** and **invalid inputs**
- ✅ Test **domain invariants**
- ✅ Keep tests **fast and isolated**

### Architecture Standards
- ✅ **Domain layer** depends on **nothing** (no WPF, no persistence)
- ✅ **Application layer** depends only on **Domain**
- ✅ **Infrastructure layer** depends on **Domain** and **Application**
- ✅ **WPF layer** depends only on **Application**
- ✅ **Tests** mirror the structure of the code they test
- ✅ **Dependencies flow inward** (WPF → Application → Domain)

---

## Architecture Principles

### 1. Domain First
The mind-map model **MUST** express domain concepts without references to:
- WPF controls
- Coordinates or positioning
- Files or databases
- Serialization libraries
- Any framework-specific types

### 2. Small Vertical Slices
Development should proceed through **complete, usable workflows** rather than building all layers up front.

**Example First Slice**:
```
Create mind map → Add central topic → Add child topic → Display on canvas → Save to file
```

### 3. Separate Meaning from Presentation
- A `Topic` describes an idea (domain)
- Its screen position, color, shape belong to the **presentation layer** (WPF)
- Only add presentation concerns to domain if they become **meaningful domain concepts**

### 4. Experiments Must Be Replaceable
- Technology explorations **MUST** be isolated behind clear boundaries
- Replacing persistence, MVVM toolkit, or layout algorithm **MUST NOT** require rewriting the domain
- Document experiments in `docs/experiments/`

### 5. Decisions Must Be Recorded
- Important choices **MUST** be documented as ADRs in `docs/decisions/`
- Use lightweight **Architecture Decision Record** format
- Include: context, decision, alternatives considered, consequences

---

## ADR (Architecture Decision Records) Requirements

### When to Create an ADR

An **Architecture Decision Record (ADR)** **MUST** be created for:

1. **Significant architectural changes** – Any change that affects the overall project structure, technology stack, or fundamental design
2. **Technology selection or changes** – Choosing frameworks, libraries, or tools that will be used across the project
3. **Major design decisions** – Decisions that will have long-term impact on the codebase
4. **Large-scale refactoring** – Significant code restructuring that affects multiple components
5. **Cross-cutting concerns** – Decisions about logging, error handling, validation, etc. that affect the entire project
6. **Brainstorming outcomes** – When multiple solutions are considered for a problem, the **final decision** MUST be documented in an ADR

> **Rule of Thumb**: If the decision will affect future development or maintenance, or if it represents a significant choice between alternatives, **CREATE AN ADR**.

### When to Ask About ADR

If you are **unsure** whether a change requires an ADR, **ASK THE USER**:
- "Should I create an ADR for this [change/decision]?"
- Provide context about the change

**Typically create an ADR if the change involves:**
- Affecting multiple components or layers
- Introducing a new technology, framework, or library
- Changing the architecture or project structure
- Making a long-term commitment
- Being the result of brainstorming multiple options
- Having significant consequences (positive or negative)

### ADR Creation Process

1. **Identify the need** – Recognize that a decision needs to be recorded
2. **Gather context** – Document the problem, requirements, and constraints
3. **Consider alternatives** – List and evaluate possible solutions (especially for brainstorming sessions)
4. **Make decision** – Choose the best solution with clear rationale
5. **Document consequences** – Describe positive and negative outcomes
6. **Create file** – Save as `docs/decisions/ADR-XXX-[name].md`
7. **Number sequentially** – Use the next available number (ADR-001, ADR-002, etc.)

### ADR File Template

```markdown
# ADR-XXX: [Short Title]

## Status
✅ Accepted / ⚠️ Proposed / ❌ Deprecated / 🗑️ Superseded by [ADR-YYY]

## Context
[What is the problem we're trying to solve?]
[What are the requirements and constraints?]

## Decision
[What did we decide to do?]
[Why did we choose this solution?]

## Alternatives Considered
- **Option 1**: [Description] - Pros: [list] / Cons: [list]
- **Option 2**: [Description] - Pros: [list] / Cons: [list]

## Consequences
### Positive
- [Benefit 1]
- [Benefit 2]

### Negative
- [Drawback 1]
- [Drawback 2]

## Related
- [ADR-001](ADR-001-...md)
- [ADR-002](ADR-002-...md)
```

### ADR File Specifications

| Requirement | Value |
|-------------|-------|
| **Directory** | `docs/decisions/` |
| **Naming** | `ADR-XXX-[short-name].md` (e.g., `ADR-001-Technology-Stack.md`) |
| **Format** | Markdown (`.md`) |
| **Language** | English |
| **Numbering** | Sequential, starting from 001, **never reuse numbers** |
| **Status** | Must include one of: ✅ Accepted, ⚠️ Proposed, ❌ Deprecated, 🗑️ Superseded |

### ADR Workflow

1. **Before implementation** – Create ADR for significant decisions
2. **During brainstorming** – Document all considered options in the ADR under "Alternatives Considered"
3. **After decision** – Update ADR with final choice, rationale, and consequences
4. **When superseding** – Create new ADR and mark old one as **Superseded by [ADR-XXX]**
5. **When deprecated** – Update status to **Deprecated** with explanation

### Brainstorming and ADRs

When brainstorming solutions for a problem:

1. **Document the process** – Create an ADR file **before** starting the brainstorming
2. **List all alternatives** – Each considered solution goes under "Alternatives Considered" with pros/cons
3. **Record the decision** – The final choice with rationale goes under "Decision"
4. **Document consequences** – Both positive and negative outcomes
5. **The ADR is the source of truth** – Once accepted, the ADR represents the official decision

### Example Questions That Require ADRs

| Question | Requires ADR? | Reason |
|----------|---------------|--------|
| Should we use Entity Framework or Dapper? | ✅ Yes | Technology selection, cross-cutting concern |
| Should we implement CQRS pattern? | ✅ Yes | Architectural pattern decision |
| Should we switch from WPF to Avalonia? | ✅ Yes | Major UI framework change |
| Should we add real-time collaboration? | ✅ Yes | New major feature, architectural impact |
| Should we change our testing framework? | ✅ Yes | Cross-cutting concern, affects all tests |
| Should we restructure into microservices? | ✅ Yes | Major architectural change |
| Should we rename this method? | ❌ No | Trivial change, no long-term impact |
| Should we fix this typo? | ❌ No | Trivial fix |
| Should we add a new domain property? | ⚠️ Maybe | Ask if it has architectural implications |

### ADR Status Meanings

| Status | Icon | Meaning |
|--------|------|---------|
| **Accepted** | ✅ | Decision is active and being implemented |
| **Proposed** | ⚠️ | Decision is under consideration, not yet implemented |
| **Deprecated** | ❌ | Decision was reversed or abandoned |
| **Superseded** | 🗑️ | Replaced by a newer ADR (reference the new one) |

---

## File Naming Conventions

| Type | Convention | Example |
|------|------------|---------|
| **C# Class** | PascalCase | `MindMap.cs`, `TopicService.cs` |
| **C# Interface** | IPascalCase | `IMindMapRepository.cs` |
| **C# Record** | PascalCase | `CreateMindMapCommand.cs` |
| **Test Class** | `[ClassName]Tests` | `MindMapTests.cs` |
| **View (XAML)** | PascalCase + `View` | `MindMapView.xaml` |
| **ViewModel** | PascalCase + `ViewModel` | `MindMapViewModel.cs` |
| **Converter** | PascalCase + `Converter` | `BoolToVisibilityConverter.cs` |
| **Audit File** | `YYYY-MM-DD-[description]` | `2024-01-15-add-domain-model.md` |
| **ADR File** | `ADR-XXX-[name]` | `ADR-001-Domain-Model-Design.md` |
| **Experiment File** | `YYYY-MM-DD-[name]` | `2024-01-16-WPF-Canvas-Experiment.md` |

---

## Validation and Testing Requirements

### For Every Change
1. **✅ Code must compile** without warnings (treat warnings as errors: `<WarningsAsErrors>true</WarningsAsErrors>`)
2. **✅ All existing tests must pass**
3. **✅ New tests must be added** for new functionality
4. **✅ Code coverage must not decrease** below established baselines
5. **✅ Audit file must be created** and committed

### Domain Layer Validation
- Test all invariants (e.g., "MindMap must have a central topic")
- Test edge cases (null inputs, empty collections, boundary values)
- Test error conditions (throw appropriate exceptions)
- Test immutability (properties cannot be changed after creation)

### Application Layer Validation
- Test command execution and validation
- Test query results
- Test integration between domain and infrastructure
- Test error handling and exception propagation

### WPF Layer Validation
- Test view model logic
- Test binding behavior
- Test user interaction flows
- Test visual states

---

## Communication Standards

When reporting work completion (to users or in commit messages):

1. **Reference the audit file**: `See Audit/2024-01-15-add-domain-model.md`
2. **Summarize changes**: Brief description of what was done
3. **Report test results**: `All 15 tests passed, coverage at 95%`
4. **Note any issues**: `Warning: MindMap title validation needs refinement`
5. **Suggest next steps**: `Ready for: Topic domain model implementation`

### Commit Message Format
```
[Area] Brief description of changes

- Created/Modified/Deleted: [files]
- Added: [features/tests]
- Fixed: [issues]

Audit: [audit-file-name].md
Generated by Mistral Vibe.
Co-Authored-By: Mistral Vibe <vibe@mistral.ai>
```

**Example**:
```
[Domain] Add MindMap aggregate root

- Created: src/Cartamentis.Domain/MindMaps/MindMap.cs
- Created: src/Cartamentis.Domain/MindMaps/MindMapId.cs
- Added: Unit tests for MindMap invariants

Audit: 2024-01-15-add-mindmap-domain-model.md
Generated by Mistral Vibe.
Co-Authored-By: Mistral Vibe <vibe@mistral.ai>
```

---

## Technology Baseline

| Aspect | Technology |
|--------|------------|
| **Runtime** | .NET 8 (LTS) |
| **Language** | C# 12 |
| **UI Framework** | WPF, XAML |
| **Pattern** | MVVM |
| **Testing** | xUnit |
| **Mocking** | Moq or NSubstitute |
| **Assertions** | FluentAssertions (optional) |
| **Serialization** | System.Text.Json |
| **Dependency Injection** | Microsoft.Extensions.DependencyInjection |
| **Logging** | Microsoft.Extensions.Logging |
| **Persistence** | JSON files (initial), SQLite (future) |

---

## Project Milestones (from README.md)

All work **MUST** align with the current milestone. Do **NOT** implement features from future milestones.

| Milestone | Focus | Deliverables |
|-----------|-------|--------------|
| **M1: Walking Skeleton** | Project setup | Solution and projects created, WPF app starts, basic test pipeline |
| **M2: Minimal Domain** | Core domain | MindMap, Topic, Connection implemented, invariants enforced, unit tests |
| **M3: First Usable Map** | Basic UI | Create mind map, add/rename/remove topics, display on canvas |
| **M4: Persistence** | Save/Load | Save map locally, open saved map, handle invalid data |
| **M5: Editing Experience** | UX improvements | Move topics, keyboard shortcuts, undo/redo, selection, navigation |

> **Current Focus**: Refer to README.md for the current active milestone.

---

## Non-Goals (from README.md)

These capabilities are **explicitly out of scope** for the first version and **MUST NOT** be implemented:
- Real-time collaboration
- User accounts and cloud synchronization
- Mobile or web clients
- Plugin infrastructure
- Sophisticated automatic layout engine
- Full compatibility with existing mind-map formats

---

## Getting Started for Agents

1. **Read** this `AGENTS.md` file
2. **Read** `README.md` for project context
3. **Check** the `Audit/` directory for recent work
4. **Verify** the current milestone
5. **Create** an audit file for your work
6. **Implement** changes following all rules
7. **Test** thoroughly
8. **Document** in the audit file
9. **Commit** changes with audit file

---

## Enforcement

These rules are **mandatory** and **non-negotiable**:

| Violation | Consequence |
|-----------|-------------|
| Missing audit file | Work **rejected**, must be redone with audit file |
| Tests not passing | Work **rejected**, must fix tests |
| Build failing | Work **rejected**, must fix build |
| Prohibited technology used | Work **rejected**, must remove and use allowed technology |
| Domain layer impurity | Work **rejected**, must refactor to remove dependencies |
| Structure violation | Work **rejected**, must follow project structure |
| Milestone misalignment | Work **rejected**, must align with current milestone |

> **Note**: Repeated violations may result in agent access being revoked for this project.

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| v1.0 | 2024-01-15 | Initial global configuration |

---

## Notes

- This file **MUST** be kept up-to-date as project requirements evolve
- Extend with project-specific rules as needed
- Review before each major milestone
- Use as a reference for all agents and prompts working on this project
- For questions, refer to `README.md` or consult the project maintainer

---

*Last updated: 2024-01-15*
*Project: Cartamentis*
*Configuration: Global AI Agents*