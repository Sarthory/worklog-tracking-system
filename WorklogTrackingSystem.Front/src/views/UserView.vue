<script setup lang="ts">
import CreateWorklogEntryModal from '@/components/CreateWorklogEntryModal.vue'
import WorkLogsList from '@/components/WorklogsList.vue'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
</script>

<template>
  <div class="user-view">
    <header class="user-view__header">
      <h2>My Worklogs</h2>
      <CreateWorklogEntryModal />
    </header>

    <WorkLogsList :userId="authStore.user?.id" />
  </div>
</template>

<style lang="scss" scoped>
.user-view {
  padding: 1.5rem;

  &__header {
    margin-bottom: 1rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
