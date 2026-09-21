# Frontend

## Tech stack + used libraries
- Vue 3 + TypeScript
- Tailwind CSS is used for styling; prefer utility classes over custom CSS
- Uses [shadcn-vue project](https://www.shadcn-vue.com/) reusable components
	- use shadcn MCP server for installing new components
	- configuration of shadcn-vue CLI can be found at `src/Workbench.Frontend/components.json`

## Rules
- Imports: `vue`, `vue-router`, and `@vueuse/core` are auto-imported
- Imports: prefer `@/` alias for internal modules
- ESLint ignores `.vscode`, `src/**/*.generated.*`, `scripts/**/*.*`, and `eslint.config.js`
- Components: template component names are PascalCase (enforced by ESLint)
- Props: do not mutate props (`vue/no-mutating-props` is enforced)
- TypeScript: `strict` enabled; no unused locals/params; avoid `any`
- Prefer typed `defineProps` and `defineEmits` declarations
- Error handling: use `catch` with user feedback (e.g., `toast`)
- `alert` is banned; `console.log` and `debugger` are allowed (dropped in prod build)
- `import/first` is disabled for Vue SFC flexibility; keep a logical order anyway
- Avoid side-effect-only imports unless necessary (`noUncheckedSideEffectImports` is on)

## Frontend structure
- Pages live in `src/Workbench.Frontend/src/pages`
- Reusable components live in `src/Workbench.Frontend/src/components`
- Keep scoped styles minimal; prefer template classes
- Vite dev server expects certs in `src/Workbench.Frontend/cert`

## Generated code
- Do not edit `src/Workbench.Frontend/src/api/api.generated.ts`
	- regenerate via `pnpm generate` in `src/Workbench.Frontend` whenever `src/Workbench.Api/Workbench.Api.nswag.json` changes
