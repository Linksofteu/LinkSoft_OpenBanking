import { Api } from './api.generated'

export const apiClient = new Api({
  // baseUrl: import.meta.env.VITE_APP_API_URL,
})

export * from './api.generated'
