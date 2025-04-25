import type { Role } from '@/enums/roles'

export interface User {
  id: string
  login: string
  firstName: string
  lastName: string
  dailyMinHours: number
  dailyMaxHours: number
  role: Role
}
