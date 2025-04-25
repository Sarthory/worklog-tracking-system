import axios from 'axios'
import { useAuthStore } from '@/stores/auth'

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL

console.log('apiBaseUrl', apiBaseUrl)

const api = axios.create({
  baseURL: apiBaseUrl,
})

api.interceptors.request.use((config) => {
  const authStore = useAuthStore()
  if (authStore && authStore.accessToken) {
    config.headers.Authorization = `Bearer ${authStore.accessToken}`
  }
  return config
})

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status >= 400 && error.response.status <= 500) {
      return Promise.resolve(error.response)
    }
    return Promise.reject(error)
  },
)

export default api
