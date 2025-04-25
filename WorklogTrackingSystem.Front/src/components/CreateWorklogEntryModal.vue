<script setup>
import { Role } from '@/enums/roles'
import { onUpdated, reactive, ref } from 'vue'
import { useWorklogStore } from '@/stores/worklog'
import { useAuthStore } from '@/stores/auth'

const worklogStore = useWorklogStore()
const authStore = useAuthStore()
const isOpen = ref(false)
const isLoading = ref(false)
const error = ref(null)

const formData = reactive({
  userId: null,
  date: null,
  workedHours: 0,
  description: null,
  taskId: null,
})

onUpdated(() => {
  if (authStore.user) {
    formData.userId = authStore.user.id
  }
})

const handleSubmitData = async () => {
  isLoading.value = true
  error.value = null

  if (!formData.date || !formData.workedHours || !formData.description) {
    error.value = 'Please fill in required* fields.'
    isLoading.value = false
    return
  }

  if (formData.workedHours <= 0) {
    error.value = 'Worked hours must be greater than 0.'
    isLoading.value = false
    return
  }

  const req = await worklogStore.createWorklog(formData)

  if (req === true) {
    worklogStore.fetchWorklogs(authStore.user.id, 1, 10)
    handleClose()
  } else {
    error.value = req
    isLoading.value = false
  }
}

const handleClose = () => {
  isOpen.value = false
  isLoading.value = false
  error.value = null
  Object.keys(formData).forEach((key) => (formData[key] = null))
}
</script>

<template>
  <v-dialog v-model="isOpen" max-width="600">
    <template v-slot:activator="{ props: activatorProps }">
      <v-btn
        class="text-none"
        prepend-icon="mdi-timer-plus-outline"
        text="New Entry"
        color="primary"
        v-bind="activatorProps"
      />
    </template>

    <v-card prepend-icon="mdi-timer-plus-outline" title="Insert Worked Hours">
      <v-card-text>
        <v-row dense>
          <v-col cols="12" md="4" sm="6">
            <v-text-field
              type="date"
              :min="new Date(new Date().getFullYear(), 0, 1).toISOString().split('T')[0]"
              :max="new Date().toISOString().split('T')[0]"
              v-model="formData.date"
              density="compact"
              label="Date*"
              required
            />
          </v-col>

          <v-col cols="12" md="4" sm="6">
            <v-number-input
              :min="0"
              :max="12"
              density="compact"
              v-model="formData.workedHours"
              :reverse="false"
              controlVariant="stacked"
              label="Worked Hours*"
              :hideInput="false"
              required
            />
          </v-col>

          <v-col cols="12" md="4" sm="6">
            <v-number-input
              :min="0"
              density="compact"
              v-model="formData.taskId"
              :reverse="false"
              controlVariant="stacked"
              label="Task ID"
              :hideInput="false"
              required
            />
          </v-col>

          <v-col cols="12" md="12" sm="6">
            <v-text-field
              v-model="formData.description"
              label="Description*"
              density="compact"
              required
            />
          </v-col>
        </v-row>

        <small v-if="error" class="error">
          {{ error }}
        </small>
      </v-card-text>

      <v-divider />

      <v-card-actions>
        <v-btn
          prepend-icon="mdi-close"
          text="Close"
          color="error"
          variant="plain"
          @click="handleClose"
        />
        <v-spacer />
        <v-btn
          color="success"
          prepend-icon="mdi-content-save"
          :loading="isLoading"
          text="Save"
          variant="elevated"
          @click="handleSubmitData"
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
