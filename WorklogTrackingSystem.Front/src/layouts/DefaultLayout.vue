<!-- <template>
  <div class="layout">
    <header class="header">
      <div class="header-content">
        <h1>Worklog Tracking System</h1>
        <button @click="logout" class="logout-button">Logout</button>
      </div>
    </header>
    <div class="main-content">
      <nav class="sidebar">
        <router-link to="/" class="nav-link"> Dashboard </router-link>

        <router-link v-if="authStore.user.role === Role.Admin" to="/admin" class="nav-link">
          Admin
        </router-link>

        <router-link v-if="authStore.user.role === Role.User" to="/user" class="nav-link">
          User
        </router-link>
      </nav>
      <main class="content">
        <router-view />
      </main>
    </div>
  </div>
</template> -->
<template>
  <v-layout>
    <v-navigation-drawer expand-on-hover rail v-model="drawer">
      <v-list>
        <v-list-item
          prepend-icon="mdi-account-circle"
          :title="authStore?.user?.firstName + ' ' + authStore?.user?.lastName"
          :subtitle="authStore?.user?.role"
        />
      </v-list>

      <v-divider></v-divider>

      <v-list density="compact" nav>
        <v-list-item
          v-if="authStore.user?.role === Role.User"
          prepend-icon="mdi-calendar-clock"
          title="My Worklogs"
          @click="router.push('/')"
        />
        <v-list-item
          v-if="authStore.user?.role === Role.Admin"
          @click="router.push('/admin')"
          prepend-icon="mdi-account-multiple"
          title="Users"
        />
        <v-list-item prepend-icon="mdi-chart-box-multiple-outline" title="Reports" />
        <v-list-item prepend-icon="mdi-cog" title="Settings" />
      </v-list>
    </v-navigation-drawer>

    <v-main>
      <header class="header">
        <div class="header-content">
          <v-btn
            icon="mdi-menu"
            density="comfortable"
            color="primary"
            @click.stop="drawer = !drawer"
            v-if="width < 1280"
          />

          <h1>Worklog Tracking System</h1>

          <v-btn icon="mdi-logout" density="comfortable" color="error" @click="logout" />
        </div>
      </header>

      <router-view />
    </v-main>
  </v-layout>
</template>

<script setup lang="ts">
import { Role } from '@/enums/roles'
import { useAuthStore } from '@/stores/auth'
import { useWindowSize } from '@vueuse/core'
import { ref } from 'vue'
import { useRouter } from 'vue-router'
const { width, height } = useWindowSize()

const drawer = ref(true)

const authStore = useAuthStore()
const router = useRouter()

const logout = () => {
  authStore.logout()
  router.push('/login')
}
</script>

<style lang="scss" scoped>
.header {
  background-color: #2c3e50;
  color: white;
  padding: 1rem;

  .header-content {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin: 0 auto;
  }
}

.main-content {
  display: flex;
  flex: 1;
}

.sidebar {
  width: 200px;
  background-color: #f8f9fa;
  padding: 1rem;
}

.content {
  flex: 1;
  padding: 1rem;
}

.nav-link {
  display: block;
  padding: 0.5rem;
  color: #2c3e50;
  text-decoration: none;
}

.nav-link:hover {
  background-color: #e9ecef;
}

.logout-button {
  background-color: #dc3545;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
}

.logout-button:hover {
  background-color: #c82333;
}
</style>
