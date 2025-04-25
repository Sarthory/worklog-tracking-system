<script setup>
import { useWorklogStore } from '@/stores/worklog'
import { ref, watch } from 'vue'
import WorklogEntriesModal from './WorklogEntriesModal.vue'

const worklogStore = useWorklogStore()

const props = defineProps({
  userId: {
    type: Number,
    required: true,
  },
})

const itemsPerPage = ref(10)
const currentPage = ref(null)
const totalItems = ref(0)
const filter = ref('')

const headers = ref([
  {
    title: 'Date',
    align: 'start',
    sortable: false,
    key: 'date',
    value: 'date',
  },
  {
    title: 'Total Worked Hours',
    align: 'start',
    sortable: false,
    key: 'totalWorkedHours',
    value: 'totalWorkedHours',
  },
  {
    title: 'Registers Count',
    align: 'start',
    sortable: false,
    key: 'worklogCount',
  },
  {
    title: 'Actions',
    align: 'end',
    value: 'action',
  },
])

const loadItems = async ({ page, itemsPerPage }) => {
  currentPage.value = page
  worklogStore.fetchWorklogs(props.userId, page, itemsPerPage, filter.value).then((res) => {
    totalItems.value = res.totalCount
  })
}

watch(
  () => filter.value,
  () => loadItems({ page: currentPage.value, itemsPerPage: itemsPerPage.value }),
)
</script>

<template>
  <div class="filter-switches">
    <v-switch
      v-model="filter"
      :label="'Undertime Only'"
      value="Undertime"
      hide-details
      :loading="filter === 'Undertime' && worklogStore.isLoading"
    />

    <v-switch
      v-model="filter"
      :label="'Overtime Only'"
      value="Overtime"
      hide-details
      :loading="filter === 'Overtime' && worklogStore.isLoading"
    />
  </div>
  <v-data-table-server
    v-model:items-per-page="itemsPerPage"
    :items-per-page-options="[10, 20, 50, 100]"
    :headers="headers"
    :items="worklogStore?.worklogs"
    :items-length="totalItems"
    :loading="worklogStore.isLoading"
    item-value="date"
    @update:options="loadItems"
  >
    <template v-slot:item.date="{ item }">
      <strong>{{ item.date }}</strong>
    </template>
    <template v-slot:item.totalWorkedHours="{ item }">
      <strong>
        {{ item.totalWorkedHours }} Hours
        <v-badge
          v-tooltip:top="'Undertime'"
          v-if="item.situation === 'Undertime'"
          color="error"
          content="UT"
          inline
        />
        <v-badge
          v-tooltip:top="'Overtime'"
          v-if="item.situation === 'Overtime'"
          color="warning"
          content="OT"
          inline
        />
      </strong>
    </template>
    <template v-slot:item.action="{ item }">
      <WorklogEntriesModal :entries="item.entries" :date="item.date" />
    </template>
  </v-data-table-server>
</template>

<style lang="scss" scoped>
.filter-switches {
  display: flex;
  justify-content: flex-start;
  gap: 2rem;
  margin-bottom: 1rem;
}
</style>
