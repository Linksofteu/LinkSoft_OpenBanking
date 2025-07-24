<script lang="ts">
const templateDocument: Omit<SoftwareStatementRegistrationDocumentModel, 'generateSoftwareId'> = {
  softwareName: 'Kačka (DEV LOCAL)',
  softwareNameEn: 'The Duck (DEV LOCAL)',
  softwareId: '',
  softwareVersion: '0.1',
  softwareUri: 'https://kacka.example.org',
  redirectUris: [
    'https://localhost:3000/callback',
  ],
  registrationBackUri: 'https://localhost:3000/callback',
  contacts: [
    'email: michal.sindelar@linksoft.cz',
  ],
  logoUri: 'https://client.example.org/logo.png',
  tosUri: 'https://client.example.org/tos',
  policyUri: 'https://client.example.org/policy',
}
</script>

<script setup lang="ts">
import type { SoftwareStatementRegistrationDocumentModel } from '@/api'
import { toast } from 'vue-sonner'
import { apiClient } from '@/api'

const generateSoftwareId = ref(true)
const softwareStatementDocument = ref(JSON.stringify(templateDocument, null, 2))

const router = useRouter()

function handleSubmit() {
  let parsedDocument: SoftwareStatementRegistrationDocumentModel | null = null
  try {
    parsedDocument = JSON.parse(softwareStatementDocument.value)
  }
  catch (e) {
    toast('Software statement is not valid JSON', {
      description: e instanceof Error ? e.message : 'Unknown error',
    })

    return
  }

  if (parsedDocument) {
    parsedDocument.generateSoftwareId = generateSoftwareId.value

    console.log(parsedDocument)

    apiClient.api.createApplicationEndpoint(parsedDocument)
      .then(() => {
        toast('Application created')
        router.push('/')
      })
      .catch((e) => {
        console.log(e)
        toast('Failed to create application', {
          description: e instanceof Error ? e.message : 'Unknown error',
          action: {
            label: 'Undo',
            onClick: () => console.log('Undo'),
          },
        })
      })
  }
}
</script>

<template>
  <Card class="w-xl">
    <CardHeader>
      <CardTitle>Create new application</CardTitle>
    </CardHeader>
    <CardContent class="flex flex-col gap-4">
      <div class="flex space-x-2">
        <Checkbox id="generateSoftwareId" v-model="generateSoftwareId" />
        <Label for="terms">Generate application ID ("softwareId" below)</Label>
      </div>
      <div class="flex flex-col space-y-2 w-full">
        <Label for="message-2">Software Statement:</Label>
        <Textarea id="softwareStatementDocument" v-model="softwareStatementDocument" />
      </div>
    </CardContent>

    <CardFooter class="flex justify-evenly">
      <Button @click="handleSubmit">
        Submit
      </Button>
      <Button variant="outline" @click="$router.back()">
        Cancel
      </Button>
    </CardFooter>
  </Card>
</template>

<style scoped></style>
