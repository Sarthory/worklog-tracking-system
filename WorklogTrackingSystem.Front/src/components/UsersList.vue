<script setup>
import { useWorklogStore } from '@/stores/worklog'
import { ref } from 'vue'
import EditUserModal from '@/components/EditUserModal.vue'
import UserWorklogsModal from './UserWorklogsModal.vue'

const worklogStore = useWorklogStore()

const itemsPerPage = ref(10)
const totalItems = ref(0)

const headers = ref([
  {
    title: 'ID',
    align: 'start',
    sortable: false,
    key: 'id',
  },
  {
    title: 'First Name',
    align: 'start',
    sortable: false,
    key: 'firstName',
  },
  {
    title: 'Last Name',
    align: 'start',
    sortable: false,
    key: 'lastName',
  },
  {
    title: 'Login',
    align: 'start',
    sortable: false,
    key: 'login',
  },
  {
    title: 'Daily Min Hours',
    align: 'start',
    sortable: false,
    key: 'dailyMinHours',
  },
  {
    title: 'Daily Max Hours',
    align: 'start',
    sortable: false,
    key: 'dailyMaxHours',
  },
  {
    title: 'Role',
    align: 'start',
    sortable: false,
    key: 'role',
  },
  {
    title: 'Actions',
    align: 'end',
    value: 'action',
  },
])

const loadItems = async ({ page, itemsPerPage }) => {
  worklogStore.fetchUsers(page, itemsPerPage).then((res) => {
    totalItems.value = res.totalCount
  })
}
</script>

<template>
  <v-data-table-server
    v-model:items-per-page="itemsPerPage"
    :items-per-page-options="[10, 20, 50, 100]"
    :headers="headers"
    :items="worklogStore.users"
    :items-length="totalItems"
    :loading="worklogStore.isLoading"
    item-value="name"
    @update:options="loadItems"
  >
    <template v-slot:item.action="{ item }">
      <EditUserModal :user="item" />
      &nbsp;
      <UserWorklogsModal :user="item" />
    </template>
  </v-data-table-server>
</template>

<style lang="scss" scoped></style>
