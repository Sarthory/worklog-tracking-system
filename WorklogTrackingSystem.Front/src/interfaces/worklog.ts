export interface Worklog {
  id: string
  userId: string
  date: string
  hours: number
  description: string | null
  taskId: number | null
}
