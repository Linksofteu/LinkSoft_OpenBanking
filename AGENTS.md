# AGENTS.md

This repository contains a .NET librarise + backend and a Vue 3 + Vite frontend used as libraries test harness.
Use the commands and conventions below when acting as a coding agent.

## Project layout
- Solution: `src/LinkSoft.OpenBanking.sln`
- libraries: `src/lib/LinkSoft.OpenBanking.Komercka.*`
	- this is main product of this repo (NuGet packages to be published by GitHub actions)
- API (`src/Workbench.Api/Workbench.Api.csproj`) + Frontend app: (`src/Workbench.Frontend`)
	- this is just a test harness for testing functionality of libraries
- Frontend API client output: `src/Workbench.Frontend/src/api/api.generated.ts`
- C# generated clients: `src/lib/LinkSoft.OpenBanking.Komercka.Client/AccountDirectAccess/*.generated.cs`
- Backend endpoints: `src/Workbench.Api/Endpoints`
- Frontend pages: `src/Workbench.Frontend/src/pages`
- Frontend reusable UI: `src/Workbench.Frontend/src/components/ui`

## Requirements
- .NET SDK 8+ (API targets net9.0; libs target net8.0/net9.0)
- Node.js >= 22 and pnpm >= 10 (enforced by `preinstall` scripts)
- mkcert if you need HTTPS dev certs for the frontend (see `readme.md`)

## Build, lint, test
### Backend (.NET)
- Restore: `dotnet restore src/LinkSoft.OpenBanking.sln`
- Build all: `dotnet build src/LinkSoft.OpenBanking.sln`
- Build API only: `dotnet build src/Workbench.Api/Workbench.Api.csproj`
- Run API: `dotnet run --project src/Workbench.Api/Workbench.Api.csproj`
- OpenAPI + frontend client generation: `dotnet build src/Workbench.Api/Workbench.Api.csproj`
- OpenAPI generation runs NSwag and writes `Workbench.Api.nswag.json`
- Frontend client generation runs `node scripts/generate.api.mjs` after `pnpm install`
- Tests: no test projects are present in this repo
- Single test pattern (when tests exist): `dotnet test <project>.csproj --filter "FullyQualifiedName~SomeName"`

### Frontend (Vue + Vite)
- Install: `pnpm install` (from `src/Workbench.Frontend`)
- Dev server: `pnpm dev` (https://localhost:3000)
- Build: `pnpm build`
- Preview: `pnpm preview`
- Run a typecheck: `pnpm typecheck`
- Lint: `pnpm lint`
- Lint + fix: `pnpm lint:fix`
- Generate API client: `pnpm generate`
- Tests: no frontend test runner configured
- Single test pattern (when added): use the runner's filter flag (e.g., `pnpm vitest -t "name"`)

## Code style and conventions
### General
- Respect `.editorconfig` for C# and follow existing formatting in other files
- Use LF line endings in C# files; do not add a final newline to C# files
- Keep lines under 180 characters in C#
- Wrap long binary expressions with the operator at the beginning of the next line
- Avoid editing generated files (see Generated code section)
- Prefer minimal diffs and keep edits localized

### C# (.NET)
- Indentation: 4 spaces, no tabs
- Namespaces: use file-scoped namespaces (preferred in this repo)
- Usings: place outside namespace; do not separate into groups; do not force System-first sorting
- Global usings are used (see `GlobalUsings.cs` and `Program.cs`)
- Types: prefer explicit types; `var` is discouraged
- Accessibility: add explicit access modifiers for non-interface members
- Braces: always required for control blocks
- Expression-bodied members: ok for accessors/properties; avoid for methods/constructors unless already used nearby
- Nullability: nullable reference types enabled; guard against nulls explicitly
- Async: suffix `Async` for `Task` returning methods and use `CancellationToken` parameters
- Pattern matching and switch expressions are preferred when they improve clarity
- Prefer `readonly` fields and init-only or auto-properties where possible
- Prefer null propagation and coalesce expressions where they simplify code
- Prefer collection and object initializers over manual population
- Prefer collection expressions when types are loosely compatible
- Prefer simplified boolean expressions and interpolation
- Prefer explicit tuple names and inferred names where applicable
- Prefer index and range operators when they improve readability
- Prefer static local functions when closures are not required
- Modifier order follows `.editorconfig` (public, private, protected, internal, file, static, ...)
- Spacing: spaces around binary operators; no extra spaces inside parentheses or brackets
- Formatting: open braces on new lines (`csharp_new_line_before_open_brace = all`)
- `using` directive placement is outside namespaces
- Simple `using` statements are preferred over extra blocks
- Naming: interfaces use `I` prefix; types/methods/properties/events use PascalCase
- Naming: private fields use `_camelCase` (see endpoints for examples)

### Tech Stack
- FastEndpoints (API)
- Frontend uses Vue 3 + TypeScript	

## Generated code
- Do not edit `src/Workbench.Api/Workbench.Api.nswag.json`, regenerate via `dotnet build src/Workbench.Api/Workbench.Api.csproj`
- Do not edit `src/lib/LinkSoft.OpenBanking.Komercka.Client/AccountDirectAccess/*.generated.cs`
	- generated automatically on each `LinkSoft.OpenBanking.Komercka.Client` build

## Configuration and secrets
- Backend uses user-secrets; see scripts in `scripts/` referenced in `readme.md`
- Production requires certificate settings (`Workbench:Certificate` and `Workbench:CertificatePassword`)
- Vite proxy uses `VITE_APP_API_URL` from environment

## Tips for agents
- Prefer minimal changes and follow local patterns in each project
- Keep changes scoped to backend or frontend unless required
- If you touch both sides, ensure API client generation still works
- Avoid editing generated clients directly; change sources instead
- When unsure about style, copy from neighboring files in the same folder
- Respect HTTPS needs for frontend and production certificate requirements
- Use descriptive exception messages for configuration and runtime errors
- If adding tests later, document the new runner commands in this file
