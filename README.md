# Cartamentis

> An experimental .NET application for creating and managing mind maps.

**Cartamentis** combines the Latin-inspired ideas of *carta* (map) and *mentis* (mind). The project is intended both as a practical mind-mapping application and as a playground for exploring technologies, architectural approaches, and UI patterns in the .NET ecosystem.

## Project status

Cartamentis is at an early exploration stage.

The first version will be a small Windows desktop application built with WPF. Its initial scope is deliberately limited: establish a clean foundation, implement a small domain model, and deliver the shortest useful workflow for creating a mind map.

## Goals

- Create and edit mind maps.
- Keep the core domain independent from the UI framework.
- Explore modern .NET technologies and engineering practices.
- Experiment with graph-oriented modeling and visualization.
- Grow the application incrementally, starting with a small, testable MVP.
- Document architectural decisions and lessons learned.

## Initial MVP

The first milestone should allow a user to:

1. Create a new mind map.
2. Define its central topic.
3. Add child topics.
4. Edit and remove topics.
5. Connect topics using relationships.
6. Display the resulting structure in a WPF application.
7. Save and load a mind map locally.

Advanced layout, collaboration, cloud synchronization, and rich formatting are intentionally outside the initial scope.

## Domain model

The initial domain will stay small and focused on mind-map management.

### Main concepts

- **MindMap**: the aggregate representing one mind map.
- **Topic**: a thought, idea, note, or concept placed on the map.
- **Connection**: a directed relationship between two topics.
- **TopicId / MindMapId**: strongly typed identifiers.

### Initial domain rules

- A mind map has a title and one central topic.
- A topic belongs to exactly one mind map.
- A connection cannot reference a topic outside its mind map.
- A topic cannot be connected to itself.
- Removing a topic also removes its related connections.
- Domain objects do not depend on WPF, storage, or rendering concerns.

These rules are an initial hypothesis and may change as the project evolves.

## Proposed solution structure

```text
Cartamentis/
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
│   ├── decisions/
│   └── experiments/
├── Cartamentis.sln
└── README.md
```

The structure is a starting point, not a commitment to unnecessary layers. Projects should be introduced only when they provide a clear boundary or support a concrete experiment.

## Technology baseline

- **.NET** and **C#**
- **WPF** and **XAML** for the first desktop user interface
- **MVVM** for separating presentation logic from views
- A unit-test framework such as **xUnit**
- Local persistence, initially using a simple format such as JSON

The exact .NET target version and supporting libraries will be selected when the solution is initialized.

## Architecture principles

### Domain first

The mind-map model should express domain concepts without references to WPF controls, coordinates, files, databases, or serialization libraries.

### Small vertical slices

Development should proceed through complete, usable workflows rather than building all architectural layers up front. The first slice may be:

```text
Create mind map → add topic → display topic → save map
```

### Separate meaning from presentation

A `Topic` describes an idea. Its screen position, color, shape, and animation belong to the presentation or layout model unless they become meaningful domain concepts.

### Experiments must be replaceable

Technology explorations should be isolated behind clear boundaries. Replacing a persistence mechanism, MVVM toolkit, or graph-layout algorithm should not require rewriting the domain.

### Decisions should be recorded

Important choices and experiment results should be documented in `docs/decisions` using lightweight Architecture Decision Records.

## Planned explorations

Cartamentis may be used to explore:

- WPF rendering with standard controls, `Canvas`, and custom drawing.
- MVVM patterns and available .NET MVVM toolkits.
- Commands, undo/redo, and change tracking.
- Graph traversal and automatic layout algorithms.
- JSON, SQLite, or another local persistence mechanism.
- Incremental and asynchronous map construction.
- Import and export formats.
- Dependency injection, logging, and configuration.
- Automated testing of domain, application, and presentation logic.
- Alternative .NET user-interface technologies after the WPF baseline is established.

An exploration is not automatically a production dependency. Each experiment should document its purpose, result, trade-offs, and final decision.

## Suggested first milestones

### Milestone 1: Walking skeleton

- Create the solution and projects.
- Start the WPF application.
- Connect the WPF project to the application and domain layers.
- Add a basic automated test pipeline.

### Milestone 2: Minimal domain

- Implement `MindMap`, `Topic`, and `Connection`.
- Enforce identifiers and basic invariants.
- Cover domain behavior with unit tests.

### Milestone 3: First usable map

- Create a mind map from the UI.
- Add, rename, and remove topics.
- Show topics and connections on a simple canvas.

### Milestone 4: Persistence

- Save a map locally.
- Open a previously saved map.
- Handle unsupported or invalid data safely.

### Milestone 5: Editing experience

- Move topics.
- Add keyboard shortcuts.
- Introduce undo and redo.
- Improve selection and navigation.

## Non-goals for the first version

- Real-time collaboration.
- User accounts and cloud synchronization.
- Mobile or web clients.
- Plugin infrastructure.
- A sophisticated automatic layout engine.
- Full compatibility with existing mind-map formats.

These capabilities may be explored later, but they should not delay the first working version.

## Getting started

The repository is still in its initial stage. Once the solution is created, the expected workflow will be similar to:

```powershell
dotnet restore
dotnet build
dotnet test
dotnet run --project src/Cartamentis.Wpf
```

WPF runs on Windows, so the desktop application requires a Windows development environment.

## Definition of success for the first release

The first release is successful when a user can create a small mind map, modify its topics and connections, save it, close the application, and open the map again, while the core behavior remains covered by automated tests.

## Contributing

Cartamentis currently serves as an experimental and learning-oriented project. Contributions should remain small, focused, and easy to evaluate.

When proposing a technology or architectural change, include:

- the problem being solved,
- the considered alternatives,
- the expected benefits,
- the costs and risks,
- and the criteria for accepting or rejecting the experiment.

## License

No license has been selected yet. Until a license file is added, the project should not be treated as open-source software with granted reuse rights.
