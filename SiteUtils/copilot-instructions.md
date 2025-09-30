# Copilot instructions for TimeTracker solution

Purpose
- Help GitHub Copilot generate code that matches this repository's architecture, coding style and runtime constraints.
- Prefer safe, buildable, and review-friendly suggestions that require minimal human edits.

Repository summary (important facts)
- Target framework: .NET 9.
- Architecture: Follow SOLID principals and put logic in separated responsible classes. Use dependency injection for passing class instances

General style rules
- Follow existing formatting:
  - Four-space indentation.
  - Use `namespace` blocks, top `using` directives
  - Use var instead of explicit types declaration
  - Use immutable query DTOs: constructor + read-only properties.
  - File/class names reflect the primary type (one class per file).
- Keep suggestions buildable and compile-ready (include required usings and types).
- Use `async` only when the project-level interfaces support it. Do not introduce `async` public APIs that conflict with existing interfaces unless the change is deliberate and covered by a follow-up refactor (see "Async guidance").

Dependency injection and registration
- Keep constructor injection minimal and explicit (only required services).


VS / editor settings (recommended)
- Add or keep an `__EditorConfig__` at solution root with C# conventions (indentation, file header rules).
- In Visual Studio, check __Tools > Options > Text Editor > C# > Code Style__ and __Text Editor > C# > Formatting__ for consistent behavior across contributors.

What Copilot should do when generating code
- Prefer generation that compiles against the current interfaces and project references (do not assume async unless interfaces support it).
- Add `using` statements the file needs.
- Avoid large refactors in suggestions; propose them separately with migration steps.
- If missing types or symbols are referenced, either:
  - Ask for clarification (preferred), or
  - Provide minimal, named stubs in a separate file and mark them clearly so reviewers can accept or replace them.

-- end --