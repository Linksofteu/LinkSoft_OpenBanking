<script setup lang="ts">
import type { SVGAttributes } from 'vue'
import { Loader2 } from 'lucide-vue-next'
import { cn } from '@/lib/utils'

const props = defineProps<{
  class?: SVGAttributes['class']
}>()
</script>

<template>
  <Loader2 :class="cn('my-28 h-16 w-16 text-primary/60 animate-spin', props.class)" />
</template>

<style scoped>

</style>
