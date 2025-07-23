<script setup lang="ts">
import { apiClient } from '@/api'
import { parseCallbackUri } from '@/lib/callback'

const router = useRouter()

const error = ref<string | null>(null)
const isProcessing = ref(true)

onMounted(async () => {
  console.log(window.location.href)

  try {
    const data = parseCallbackUri(window.location.href)
    const result = await apiClient.api.handleCallbackFlowDataEndpoint(data as any)
    console.log(result.data)

    if (result.data.redirectUri) {
      window.location.href = result.data.redirectUri
    }

    router.push('/')
  }
  catch (e) {
    error.value = (e as Error)?.message
  }
  finally {
    isProcessing.value = false
  }
})
</script>

<template>
  <div v-if="isProcessing" class="w-full">
    <Spinner class="mx-auto" />
  </div>
  <div v-else-if="error">
    <div class="text-2xl font-bold text-red-600 mx-auto">
      Error: {{ error }}
    </div>
    <Button variant="outline" @click="$router.push('/')">
      Back
    </Button>
  </div>
</template>
