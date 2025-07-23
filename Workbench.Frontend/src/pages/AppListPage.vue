<script setup lang="ts">
import { Plus } from 'lucide-vue-next'
import { apiClient } from '@/api'

import Spinner from '@/components/ui/spinner/Spinner.vue'

const { state, isLoading, execute } = useAsyncState(
  () => apiClient.api.getAllApplicationsEndpoint().then(response => response.data),
  [],
)
</script>

<template>
  <div v-if="isLoading" class="w-full">
    <Spinner class="mx-auto" />
  </div>

  <div v-if="!isLoading" class="flex flex-col mx-auto p-4 gap-2">
    <ApplicationCard v-for="app in state" :key="app.id" :app-manifest="app" class="flex-2" @app-updated="() => execute()" />
    <Card class="cursor-pointer" @click="$router.push('/create')">
      <CardHeader>
        <CardTitle>Create new application</CardTitle>
      </CardHeader>
      <CardContent>
        <Plus :size="64" class="mx-auto" />
      </CardContent>
    </Card>
  </div>
</template>

<style scoped>

</style>
