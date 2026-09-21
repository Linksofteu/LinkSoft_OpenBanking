<script setup lang="ts">
import type { Account } from '@/api'
import { apiClient } from '@/api'
import TransactionsView from '@/components/TransactionsView.vue'

const props = defineProps<{ applicationId: string, account: Account | null }>()
const error = ref<string | null>(null)

const { state: transactionsState, isLoading: transactionsLoading, execute: loadTransactions, isReady: transactionsReady } = useAsyncState(
  (account: Account) => apiClient.api.getAccountTransactionsEndpoint(props.applicationId, account.accountId).then(response => response.data),
  null,
  {
    immediate: false,
    shallow: true,
    onError: (e) => {
      console.log(e)
      error.value = JSON.stringify(e, null, 4)
    },
  },
)

watch(() => props.account, (account) => {
  if (account) {
    error.value = null
    transactionsState.value = null
    loadTransactions(100, account)
  }
}, { immediate: true })
</script>

<template>
  <div v-if="props.account">
    <div v-if="transactionsLoading" class="w-full">
      <Spinner class="mx-auto" />
    </div>
    <div v-if="error" class="w-full m-10">
      <div class="text-red-600">
        <h2>Something went wrong</h2>
        <pre>{{ error }}</pre>
      </div>
    </div>
    <div v-if="transactionsReady && transactionsState" class="flex flex-col mx-auto p-4 gap-2">
      <TransactionsView :account="props.account" :data="transactionsState" />
    </div>
  </div>
</template>

<style scoped>

</style>
