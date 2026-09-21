<script setup lang="ts">
import type { Account, PageSlice, TransactionCounterparty } from '@/api'

const props = defineProps<{ account: Account, data: PageSlice }>()

function getCounterpartyColumnValue(counterParty?: TransactionCounterparty | null) {
  if (!counterParty) {
    return ''
  }

  if (counterParty.accountNo) {
    let result = counterParty.accountNo
    if (counterParty.bankCode)
      result = `${result}/${counterParty.bankCode}`
    if (counterParty.bankName)
      result = `${result} (${counterParty.bankName})`

    return result
  }

  if (counterParty.iban) {
    let result = counterParty.iban
    if (counterParty.bankName)
      result = `${result} (${counterParty.bankName})`

    return result
  }

  return ''
}
</script>

<template>
  <div v-if="props.data">
    <Table>
      <TableCaption>Transactions for account {{ props.account?.iban }} </TableCaption>
      <TableHeader>
        <TableRow>
          <TableHead>Status</TableHead>
          <TableHead class="w-[100px]">
            Booking Date
          </TableHead>
          <TableHead>CREDIT/DEBIT</TableHead>
          <TableHead>Type</TableHead>
          <TableHead class="text-right">
            Amount
          </TableHead>
          <TableHead>Counterparty</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody v-if="props.data.content">
        <TableRow v-for="transaction in props.data.content" :key="transaction.id">
          <TableCell>
            {{ transaction.status }}
          </TableCell>
          <TableCell class="font-medium">
            {{ transaction.bookingDate }}
          </TableCell>
          <TableCell>
            {{ transaction.creditDebitIndicator }}
          </TableCell>
          <TableCell>
            {{ transaction.transactionType }}
          </TableCell>
          <TableCell class="text-right">
            {{ transaction.amount.value }} {{ transaction.amount.currency }}
          </TableCell>
          <TableCell>{{ getCounterpartyColumnValue(transaction.counterParty) }}</TableCell>
        </TableRow>
      </TableBody>
    </Table>
  </div>
</template>

<style scoped>

</style>
