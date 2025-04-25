<template>
  <div class="login-container">
    <div class="login-container__login-box">
      <h2>Worklog Tracking</h2>
      <div class="login-form">
        <v-text-field
          density="compact"
          type="text"
          label="Login"
          v-model="login"
          required
          class="form-control"
          id="login"
          @keydown.enter="handleLogin"
        />

        <v-text-field
          density="compact"
          type="password"
          label="Password"
          v-model="password"
          required
          class="form-control"
          id="password"
          @keydown.enter="handleLogin"
        />

        <v-btn
          :loading="authStore.isLoading"
          type="submit"
          @click="handleLogin"
          color="primary"
          class="login-form__login-button"
        >
          Login
        </v-btn>

        <div v-if="authStore.errors" class="errors">
          <span class="errors__message" v-for="error in authStore.errors">
            {{ error }}
          </span>
        </div>

        <router-link to="/help" class="login-form__help-link">Need help?</router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { useRouter } from 'vue-router'
import { Role } from '@/enums/roles'
const authStore = useAuthStore()
const router = useRouter()

const login = ref('')
const password = ref('')

const handleLogin = async () => {
  const success = await authStore.login({
    login: login.value,
    password: password.value,
  })

  if (success) {
    if (authStore.user?.role === Role.Admin) {
      router.push('/admin')
    } else {
      router.push('/user')
    }
  }
}
</script>

<style lang="scss" scoped>
.login-container {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 100vh;
  background-color: #fafafa;

  &__login-box {
    background-color: #ddd;
    padding: 2rem;
    border-radius: 0.5rem;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    width: 300px;

    > h2 {
      margin-bottom: 1rem;
      text-align: center;
      color: #2c3e50;
    }

    .login-form {
      display: flex;
      flex-direction: column;

      &__help-link {
        color: #2c3e50;
        text-decoration: none;
        font-size: 0.875rem;
        font-weight: bold;
        align-self: flex-end;
        margin-top: 1rem;

        &:hover {
          text-decoration: underline;
        }
      }

      .errors {
        display: flex;
        flex-direction: column;
        margin-top: 1rem;

        &__message {
          color: #b30000;
          font-size: 0.875rem;
        }
      }
    }
  }
}
</style>
