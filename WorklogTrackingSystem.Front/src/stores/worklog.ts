import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { User } from '@/interfaces/user'
import type { Worklog } from '@/interfaces/worklog'
import api from '@/utils/axios'

export const useWorklogStore = defineStore('worklogStore', () => {
  const isLoading = ref(false)
  const errors = ref<string[] | null>(null)
  const users = ref<User[] | null>([])
  const worklogs = ref<Worklog[] | null>([])

  const fetchUsers = async (page: number, pageSize: number) => {
    isLoading.value = true
    errors.value = null

    try {
      const response = await api.get(`/user/paged-users?page=${page}&pageSize=${pageSize}`)
      if (response.status === 200) {
        const { items, currentPage, pageSize, totalCount, totalPages } = response.data

        users.value = items

        return {
          currentPage,
          pageSize,
          totalCount,
          totalPages,
        }
      } else {
        throw new Error('Failed to fetch users')
      }
    } catch (error: any) {
      errors.value = [error.message]
      console.error('Error fetching users:', error)
    } finally {
      isLoading.value = false
    }
  }

  const createUser = async (user: User) => {
    isLoading.value = true
    errors.value = null

    try {
      const response = await api.post('/user/register', user)
      if (response.status === 200) {
        return true
      } else {
        throw response.data
      }
    } catch (error: any) {
      return error
    } finally {
      isLoading.value = false
    }
  }

  const updateUser = async (user: User) => {
    isLoading.value = true
    errors.value = null

    try {
      const response = await api.put(`/user/update/${user.id}`, user)
      if (response.status === 200) {
        return true
      } else {
        throw response.data
      }
    } catch (error: any) {
      return error
    } finally {
      isLoading.value = false
    }
  }

  const fetchWorklogs = async (userId: number, page: number, pageSize: number, filter: string) => {
    isLoading.value = true
    errors.value = null

    let reqUrl = `/worklog/summary/${userId}?&page=${page}&pageSize=${pageSize}`

    if (filter) {
      reqUrl += `&filter=${filter}`
    }

    try {
      const response = await api.get(reqUrl)
      if (response.status === 200) {
        const { items, currentPage, pageSize, totalCount, totalPages } = response.data

        worklogs.value = items

        return {
          currentPage,
          pageSize,
          totalCount,
          totalPages,
        }
      } else {
        throw new Error('Failed to fetch worklogs')
      }
    } catch (error: any) {
      errors.value = [error.message]
      console.error('Error fetching worklogs:', error)
    } finally {
      isLoading.value = false
    }
  }

  const createWorklog = async (worklog: Worklog) => {
    isLoading.value = true
    errors.value = null

    try {
      const response = await api.post('/worklog/insert', worklog)
      if (response.status === 200) {
        return true
      } else {
        throw response.data
      }
    } catch (error: any) {
      return error
    } finally {
      isLoading.value = false
    }
  }

  return {
    users,
    isLoading,
    errors,
    worklogs,
    fetchUsers,
    createUser,
    updateUser,
    fetchWorklogs,
    createWorklog,
  }
})
