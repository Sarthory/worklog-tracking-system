<script setup>
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()

const props = defineProps({
  entries: {
    type: Array,
    required: true,
  },
})
</script>

<template>
  <v-table height="300px" fixed-header density="compact">
    <thead>
      <tr>
        <th class="text-left">Task ID</th>
        <th class="text-left">Worked Hours</th>
        <th class="text-left">Description</th>
        <th class="text-right" v-if="authStore.user?.role === 'User'">Actions</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="item in props.entries" :key="item.id">
        <td>{{ item.taskId ? item.taskId : '-' }}</td>
        <td>{{ item.workedHours }}</td>
        <td>{{ item.description }}</td>
        <td v-if="authStore.user?.role === 'User'" class="text-right">
          <v-btn
            v-tooltip:start="'Delete Entry'"
            icon="mdi-delete"
            density="comfortable"
            color="red"
            variant="plain"
            @click="$emit('delete', item.id)"
          />
          &nbsp;
          <v-btn
            v-tooltip:start="'Edit Entry'"
            density="comfortable"
            icon="mdi-pencil"
            color="blue"
            variant="plain"
            @click="$emit('edit', item.id)"
          />
        </td>
      </tr>
    </tbody>
  </v-table>
</template>

<style lang="scss" scoped></style>
