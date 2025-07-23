<script setup lang="ts">
import type { Account } from '@/api'

const props = defineProps<{ accounts: Account[] }>()
const emit = defineEmits<{ (e: 'select', account: Account): void }>()

const selectedAccountId = ref<string | null>(null)

function selectAccount(account: Account) {
  selectedAccountId.value = account.accountId
  emit('select', account)
}
</script>

<template>
  <Table>
    <TableCaption>Accounts</TableCaption>
    <TableHeader>
      <TableRow>
        <TableHead class="w-[100px]">
          IBAN
        </TableHead>
        <TableHead>Currency</TableHead>
        <TableHead>Name</TableHead>
        <TableHead>
          Product
        </TableHead>
      </TableRow>
    </TableHeader>
    <TableBody>
      <TableRow
        v-for="account in props.accounts"
        :key="account.id"
        class="cursor-pointer"
        :data-state="account.accountId === selectedAccountId ? 'selected' : null"
        @click="selectAccount(account)"
      >
        <TableCell class="font-medium">
          {{ account.iban }}
        </TableCell>
        <TableCell>{{ account.currency }}</TableCell>
        <TableCell>{{ account.nameI18N }}</TableCell>
        <TableCell>{{ account.productI18N }}</TableCell>
      </TableRow>
    </TableBody>
  </Table>
</template>

<style scoped>
</style>
