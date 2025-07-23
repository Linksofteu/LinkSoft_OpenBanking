<script setup lang="ts">
import type { Account } from '@/api'
import { apiClient } from '@/api'
import AccountList from '@/components/AccountList.vue'
import TransactionsPageView from '@/components/TransactionsPageView.vue'

const route = useRoute()
const error = ref<string | null>(null)

const { state: accountsState, isLoading: accountsLoading, isReady: accountsReady } = useAsyncState(
  () => apiClient.api.getAccountsEndpoint(route.params.id as string).then(response => response.data),
  [],
  {
    shallow: true,
    onError: (e) => {
      console.log(e)
      error.value = JSON.stringify(e, null, 4)
    },
  },

)
const selectedAccount = ref <Account | null> (null)

const { state: transactionsState, isLoading: transactionsLoading, execute: loadTransactions, isReady: transactionsReady } = useAsyncState(
  (account: Account) => {
    if (!account) {
      return Promise.resolve(null)
    }
    return apiClient.api.getAccountTransactionsEndpoint(route.params.id as string, account.accountId).then(response => response.data)
  },
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

watchEffect(() => {
  if (selectedAccount.value) {
    loadTransactions(100, selectedAccount.value)
  }
})
</script>

<template>
  <div class="text-1xl">
    Application ID:{{ $route.params.id }}
  </div>
  <div class="mt-4">
    <Button @click="$router.push('/')">
      Back
    </Button>
  </div>
  <div v-if="accountsLoading" class="w-full">
    <Spinner class="mx-auto" />
  </div>
  <div v-if="error" class="w-full m-10">
    <div class="text-red-600">
      <h2>Something went wrong</h2>
      <pre>{{ error }}</pre>
    </div>
  </div>
  <div v-if="accountsReady" class="flex flex-col mx-auto p-4 gap-2">
    <AccountList :accounts="accountsState" @select="selectedAccount = $event" />
    <div v-if="selectedAccount && transactionsLoading" class="w-full">
      <Spinner class="mx-auto" />
    </div>
    <div v-if="selectedAccount && transactionsReady && transactionsState" class="flex flex-col mx-auto p-4 gap-2">
      <TransactionsPageView :account="selectedAccount" :data="transactionsState" />
    </div>
  </div>
</template>
