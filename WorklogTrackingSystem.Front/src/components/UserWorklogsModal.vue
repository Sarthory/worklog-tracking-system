<script setup>
import { ref } from 'vue'
import WorklogsList from './WorklogsList.vue'

const props = defineProps({
  user: {
    type: Object,
    required: true,
  },
})

const isOpen = ref(false)
</script>

<template>
  <v-dialog v-model="isOpen" fullscreen scrollable>
    <template v-slot:activator="{ props: activatorProps }">
      <v-btn
        v-tooltip:start="`See ${user.firstName} ${user.lastName}'s worklogs`"
        density="comfortable"
        color="primary"
        variant="tonal"
        icon="mdi-calendar-clock"
        v-bind="activatorProps"
      />
    </template>

    <v-card>
      <v-toolbar>
        <v-toolbar-title prepend-icon="mdi-timetable">
          <v-icon icon="mdi-calendar-clock" />&nbsp;&nbsp; {{ user.firstName }}
          {{ user.lastName }}'s worklogs
        </v-toolbar-title>

        <v-toolbar-items>
          <v-btn icon="mdi-close" @click="isOpen = false" />
        </v-toolbar-items>
      </v-toolbar>

      <v-card-text>
        <WorklogsList :userId="user.id" />
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<style lang="scss" scoped>
.error {
  color: #b30000;
  font-size: 0.8rem;
  margin-top: 0.5rem;
}
</style>
