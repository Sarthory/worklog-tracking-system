<script setup>
import { onMounted, ref } from 'vue'
import WorklogEntriesList from './WorklogEntriesList.vue'

const props = defineProps({
  entries: {
    type: Array,
    required: true,
  },
  date: {
    type: String,
    required: true,
  },
})

const isOpen = ref(false)
const isLoading = ref(false)
</script>

<template>
  <v-dialog v-model="isOpen" max-width="600">
    <template v-slot:activator="{ props: activatorProps }">
      <v-btn
        v-tooltip:start="'View Date Entries'"
        density="comfortable"
        color="primary"
        variant="tonal"
        icon="mdi-timetable"
        v-bind="activatorProps"
      />
    </template>

    <v-card prepend-icon="mdi-timetable" :title="`Worked Hours Entries (${props.date})`">
      <v-card-text>
        <WorklogEntriesList :entries="props.entries" />
      </v-card-text>

      <v-divider />

      <v-card-actions>
        <v-btn
          prepend-icon="mdi-close"
          text="Close"
          color="primary"
          variant="plain"
          @click="isOpen = false"
        />
      </v-card-actions>
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
