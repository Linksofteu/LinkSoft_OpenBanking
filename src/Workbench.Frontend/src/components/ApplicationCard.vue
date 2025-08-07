<script setup lang="ts">
import type { AccountDirectAccessApplicationManifest } from '@/api'
import { toast } from 'vue-sonner'
import { apiClient } from '@/api'

const props = defineProps<{
  appManifest: AccountDirectAccessApplicationManifest
}>()

const emit = defineEmits<{
  (e: 'appUpdated', appManifest: AccountDirectAccessApplicationManifest): void
}>()

function handleSoftwareStatementRegistration() {
  apiClient.api.registerSoftwareStatementEndpoint({ applicationId: props.appManifest.id! })
    .then((response) => {
      toast('Software statement is now registered')
      emit('appUpdated', response.data)
    })
    .catch((e) => {
      console.log(e)
      toast('Error registering Software statement', {
        description: e instanceof Error ? e.message : 'Unknown error',
      })
    })
}

function handleAppRegistration() {
  apiClient.api.getApplicationRegistrationUrlEndpoint(props.appManifest.id!)
    .then((response) => {
      window.location.href = response.data
    })
    .catch((e) => {
      console.log(e)
    })
}

function handleAppAuthorization() {
  apiClient.api.getApplicationAuthorizationUrlEndpoint(props.appManifest.id!)
    .then((response) => {
      window.location.href = response.data
    })
    .catch((e) => {
      console.log(e)
    })
}

function getYesNo(condition: boolean) {
  return condition ? 'Yes' : 'No'
}
</script>

<template>
  <Card class="w-full">
    <CardHeader>
      <CardTitle>{{ props.appManifest.softwareStatementRegistrationDocument?.softwareName }}</CardTitle>
      <CardDescription>ID: {{ props.appManifest.softwareStatementRegistrationDocument?.softwareId }}</CardDescription>
    </CardHeader>
    <CardContent>
      <div>Target environment: <b>{{ props.appManifest.targetEnvironment }}</b></div>
      <div>SoftwareStatement registered: <b>{{ getYesNo(!!props.appManifest.softwareStatement) }}</b> </div>
      <div>App registered to client account: <b>{{ getYesNo(!!props.appManifest.applicationRegistration) }}</b></div>
      <div>
        App authorized to access client account (until): <b>{{ props.appManifest.applicationAuthorization ? new
          Date(props.appManifest.applicationAuthorization.refreshTokenExpirationUtc).toLocaleDateString() : 'No' }}</b>
      </div>
    </CardContent>

    <CardFooter class="gap-2 flex-wrap">
      <Button
        v-if="props.appManifest.softwareStatementRegistrationDocument"
        :variant="props.appManifest.softwareStatementRegistrationDocument ? 'destructive' : 'default'"
        @click="handleSoftwareStatementRegistration"
      >
        Register Software Statement
      </Button>
      <Button
        v-if="props.appManifest.softwareStatement"
        :variant="props.appManifest.applicationRegistration ? 'destructive' : 'default'" @click="handleAppRegistration"
      >
        Register to client account
      </Button>
      <Button
        v-if="props.appManifest.applicationRegistration"
        :variant="props.appManifest.applicationAuthorization ? 'destructive' : 'default'" @click="handleAppAuthorization"
      >
        Authorize account access
      </Button>
      <Button
        v-if="props.appManifest.applicationAuthorization"
        @click="() => $router.push(`/apps/${props.appManifest.id}`)"
      >
        Use the app...
      </Button>
    </CardFooter>
  </Card>
</template>
