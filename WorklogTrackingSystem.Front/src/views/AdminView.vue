<script setup lang="ts">
import CreateUserModal from '@/components/CreateUserModal.vue'
import UsersList from '@/components/UsersList.vue'
</script>

<template>
  <div class="admin-view">
    <header class="admin-view__header">
      <h2>Users</h2>
      <CreateUserModal />
    </header>

    <UsersList />
  </div>
</template>

<style lang="scss" scoped>
.admin-view {
  padding: 1.5rem;

  &__header {
    margin-bottom: 1rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
