import type { Role } from '@/enums/roles'

export interface LoggedUser {
  id: string
  login: string
  firstName: string
  lastName: string
  role: Role
}
