import fs from 'node:fs'
import path from 'node:path'
import process from 'node:process'
import tailwindcss from '@tailwindcss/vite'
import vue from '@vitejs/plugin-vue'
import AutoImport from 'unplugin-auto-import/vite'
import Components from 'unplugin-vue-components/vite'
import { defineConfig, loadEnv } from 'vite'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  process.env = { ...process.env, ...loadEnv(mode, process.cwd()) }

  const isProd: boolean = process.env.NODE_ENV === 'production'

  return {
    plugins: [
      vue(),
      AutoImport({
        include: [
          /\.[tj]sx?$/, // .ts, .tsx, .js, .jsx
          /\.vue$/,
          /\.vue\?vue/, // .vue
          /\.vue\.[tj]sx?\?vue/, // .vue (vue-loader with experimentalInlineMatchResource enabled)
          /\.md$/, // .md
        ],
        imports: [
          // presets
          'vue',
          'vue-router',
          '@vueuse/core',
        ],
      }),
      Components({ /* options */ }),
      tailwindcss(),
    ],
    resolve: {
      alias: {
        '@': path.resolve(__dirname, './src'),
      },
    },
    esbuild: {
      drop: isProd ? ['debugger'] : [],
      pure: isProd ? ['console.log', 'console.debug'] : [],
    },
    server: {
      https: {
        key: fs.readFileSync(path.resolve(__dirname, './cert/localhost+1-key.pem')),
        cert: fs.readFileSync(path.resolve(__dirname, './cert/localhost+1.pem')),
      },
      proxy: {
        '/api': {
          target: `${process.env.VITE_APP_API_URL}`,
          changeOrigin: true,
        },
      },
    },
  }
})
