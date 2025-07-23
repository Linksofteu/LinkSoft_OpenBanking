function removeSurroundingCharactersIfPresent(inputStr: string, char: string): string {
  if (inputStr && inputStr.startsWith(char) && inputStr.endsWith(char)) {
    return inputStr.slice(1, -1)
  }
  return inputStr
}

export interface ApplicationRegistrationCallbackData {
  salt: string
  encryptedData: string
  state: string | null
}

export function parseCallbackUri(uri: string): Record<string, string> {
  const [, queryString = ''] = uri.split('?')
  const params = new URLSearchParams(queryString)

  const result: Record<string, string> = {}
  for (const [key, value] of params) {
    if (value)
    // Fix for a known (and reported KB Sandbox issue)
      result[key] = removeSurroundingCharactersIfPresent(value, '"')
  }
  return result
}
