import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '@/utils/axios'
import { Claims } from '@/enums/claims'
import { Role } from '@/enums/roles'
import type { User } from '@/interfaces/user'
import type { LoginCredentials } from '@/interfaces/loginCredentials'
import type { LoggedUser } from '@/interfaces/loggedUser'

export const useAuthStore = defineStore('authStore', () => {
  const isLoading = ref(false)
  const errors = ref<string[]>([])
  const accessToken = ref<string | null>(localStorage.getItem('accessToken'))
  const refreshToken = ref<string | null>(localStorage.getItem('refreshToken'))
  const loggedUser = ref<LoggedUser | null>(JSON.parse(localStorage.getItem('user') || 'null'))
  const isAuthenticated = ref(!!accessToken.value)

  const login = async (credentials: LoginCredentials) => {
    isLoading.value = true
    errors.value = []
    try {
      const response = await api.post(`/auth/login?login`, credentials)

      if (response.status === 200) {
        const { accessToken: token, refreshToken: refToken } = response.data
        const decodedJwtData = JSON.parse(window.atob(token.split('.')[1]))
        localStorage.setItem('accessToken', token)

        accessToken.value = token
        refreshToken.value = refToken
        isAuthenticated.value = true
        loggedUser.value = {
          id: decodedJwtData[Claims.NameIdentifier],
          login: decodedJwtData[Claims.UserData].split(':')[1].trim(),
          firstName: decodedJwtData[Claims.Name],
          lastName: decodedJwtData[Claims.Surname],
          role: decodedJwtData[Claims.Role],
        }
        localStorage.setItem('user', JSON.stringify(loggedUser.value))
        errors.value = []
        return true
      } else {
        throw response.data
      }
    } catch (e: any) {
      if (e.errors) {
        Object.entries(e.errors).forEach(([_, value]) => {
          errors.value.push(`${value}`)
        })
      } else {
        errors.value = [e]
      }

      return false
    } finally {
      isLoading.value = false
    }
  }

  const logout = () => {
    accessToken.value = null
    refreshToken.value = null
    loggedUser.value = null
    localStorage.removeItem('accessToken')
    localStorage.removeItem('user')
    isAuthenticated.value = false
  }

  const hasRole = (role: Role) => {
    return loggedUser.value?.role === role
  }

  return {
    accessToken,
    refreshToken,
    user: loggedUser,
    isAuthenticated,
    isLoading,
    errors,
    login,
    logout,
    hasRole,
  }
})
