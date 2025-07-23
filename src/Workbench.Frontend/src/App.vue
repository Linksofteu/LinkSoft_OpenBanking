<script setup lang="ts">
import { Toaster } from '@/components/ui/sonner'
import 'vue-sonner/style.css'
</script>

<template>
  <body>
    <Toaster position="top-right" rich-colors />
    <div class="container mx-auto p-4">
      <h1 class="text-3xl font-bold text-blue-600">
        OpenBanking Workbench
      </h1>
      <RouterView />
    </div>
  </body>
</template>

<style scoped></style>
