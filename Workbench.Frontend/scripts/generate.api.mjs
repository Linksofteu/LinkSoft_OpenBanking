import { dirname, resolve } from 'node:path'
import { argv, cwd } from 'node:process'
import { fileURLToPath } from 'node:url'
import { generateApi } from 'swagger-typescript-api'

const __filename = fileURLToPath(import.meta.url)
const __dirname = dirname(__filename)

console.warn(cwd())

const outputPath = resolve(__dirname, '../src/api/')
generateApi({
  fileName: 'api.generated.ts',
  input: resolve(__dirname, '../../Workbench.Api/Workbench.Api.nswag.json'),
  output: outputPath,
  httpClientType: 'fetch', // or "axios"
  defaultResponseAsSuccess: false,
  generateRouteTypes: false,
  generateResponses: true,
  extractRequestParams: false,
  extractRequestBody: false,
  silent: argv.includes('silent'),
  defaultResponseType: 'void',
  singleHttpClient: false,
  cleanOutput: false,
  enumNamesAsValues: false,
  moduleNameFirstTag: false,
  generateUnionEnums: false,
  sortTypes: false,
  extraTemplates: [],
  hooks: {
    // onParseSchema: (originalSchema, parsedSchema) => {
    //   // Convert type string to Date when needed
    //   // https://github.com/acacode/swagger-typescript-api/issues/105
    //   if (originalSchema?.type === 'string' && ['date-time', 'date'].includes(originalSchema?.format))
    //     parsedSchema.content = 'Date'

    //   return parsedSchema
    // },
  },
}).catch(e => console.error(e))
