<script setup>
import { Role } from '@/enums/roles'
import { reactive, ref } from 'vue'
import { useWorklogStore } from '@/stores/worklog'

const worklogStore = useWorklogStore()
const isOpen = ref(false)
const isLoading = ref(false)
const error = ref(null)

const formData = reactive({
  firstName: '',
  lastName: '',
  login: '',
  password: '',
  confirmPassword: '',
  dailyMinHours: 4,
  dailyMaxHours: 8,
  role: null,
})

const handleSubmitUserData = async () => {
  isLoading.value = true
  error.value = null

  if (
    !formData.firstName ||
    !formData.lastName ||
    !formData.login ||
    !formData.password ||
    !formData.confirmPassword ||
    !formData.dailyMinHours ||
    !formData.dailyMaxHours ||
    !formData.role
  ) {
    error.value = 'Please fill in all fields.'
    isLoading.value = false
    return
  }

  if (formData.password !== formData.confirmPassword) {
    error.value = 'Passwords do not match.'
    isLoading.value = false
    return
  }

  const req = await worklogStore.createUser(formData)

  if (req === true) {
    worklogStore.fetchUsers(1, 10)
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
  Object.keys(formData).forEach((key) => (formData[key] = ''))
}
</script>

<template>
  <v-dialog v-model="isOpen" max-width="600">
    <template v-slot:activator="{ props: activatorProps }">
      <v-btn
        class="text-none"
        prepend-icon="mdi-account"
        text="Create User"
        color="primary"
        v-bind="activatorProps"
      />
    </template>

    <v-card prepend-icon="mdi-account" title="Create User">
      <v-card-text>
        <v-row dense>
          <v-col cols="12" md="6" sm="6">
            <v-text-field
              v-model="formData.firstName"
              density="compact"
              label="First name"
              required
            />
          </v-col>

          <v-col cols="12" md="6" sm="6">
            <v-text-field
              v-model="formData.lastName"
              label="Last name"
              density="compact"
              required
            />
          </v-col>

          <v-col cols="12" md="4" sm="6">
            <v-text-field v-model="formData.login" label="Login" density="compact" required />
          </v-col>

          <v-col cols="12" md="4" sm="6">
            <v-text-field
              v-model="formData.password"
              label="Password"
              density="compact"
              type="password"
              required
            />
          </v-col>

          <v-col cols="12" md="4" sm="6">
            <v-text-field
              v-model="formData.confirmPassword"
              density="compact"
              label="Confirm Password"
              type="password"
              required
            />
          </v-col>

          <v-col cols="12" md="4" sm="6">
            <v-select
              v-model="formData.dailyMinHours"
              density="compact"
              :items="[2, 3, 4, 5, 6]"
              label="Daily Min Hours"
              required
            />
          </v-col>

          <v-col cols="12" md="4" sm="6">
            <v-select
              v-model="formData.dailyMaxHours"
              density="compact"
              :items="[6, 7, 8, 9, 10, 11, 12]"
              label="Daily Max Hours"
              required
            />
          </v-col>

          <v-col cols="12" md="4" sm="6">
            <v-select
              v-model="formData.role"
              density="compact"
              :items="[Role.Admin, Role.User]"
              label="Role"
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
          @click="handleSubmitUserData"
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
