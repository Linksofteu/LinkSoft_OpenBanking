import antfu from '@antfu/eslint-config'

export default antfu({
  formatters: true,
  vue: {
    overrides: {
      'vue/no-mutating-props': ['error', {
        shallowOnly: true,
      }],

      'vue/component-name-in-template-casing': ['error', 'PascalCase', {
        registeredComponentsOnly: true,
        ignores: ['slot'],
      }],
    },
  },
  ignores: [
    '.vscode',
    'src/**/*.generated.*',
    'eslint.config.js',
    'scripts/**/*.*',
  ],
},
{
  rules: {
    /*
      `console.log`, `console.debug` and `debugger` are automaticaly removed from production build (see Vite config)
    */
    'no-console': 'off',
    'no-debugger': 'off',
    'no-alert': 'error',

    // NOTE - this is something we want but unfortunatelly it does not work wery well in Vue SFCs files
    // with script setup where we also want normal script to define and export prop types
    'import/first': 'off',
  },
}
)
