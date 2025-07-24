# .NET integration of ADA (Account Direct Access) API [KB.cz](https://www.kb.cz/)

## Supported runtimes

- .NET 6+

## DEV setup

1. Configure backend (`Workbench.Api`)
   - backend (`Workbench.Api`) is by default configured to use KB Sandbox environment (config key `Workbench:TargetEnvironment` - value "Sandbox" or "Production")
   - configure API keys for the selected target environment (get them using [KB Developer portal](https://developers.kb.cz/) after registration) 
     - for Sandbox secrets see `scripts\set-sandbox-secrets.ps1`
     - for Production secrets see `scripts\set-production-secrets.ps1`
       - as with Sandbox, you need to generate API keys
       - in addition, you need to choose [encryption key](https://github.com/komercka/adaa-client/wiki/03-Application-Registration-OAuth2#application-registration---oauth2) for application registration 
         - config key: `ADAA:Production:ApplicationRegistration:EncryptionKey`
         - 32 byte long Base64 encoded key
         - for Sandbox it is hardcoded directly in app config (cannot be changed - this is Sandbox limitation)
       - when running against Production, a valid certificate with a private key must also be configured (`Workbench:Certificate` and `Workbench:CertificatePassword`)
2. Configure frontend (`Workbench.Frontend`)
   1. you need to generate HTTPS certificate localhost (HTTPS is required for KB Production environment)
      - install [mkcert](https://github.com/FiloSottile/mkcert) and follow setup instructions
      - run `src\Workbench.Frontend\scripts\generateLocalhostCertificate.ps1`
      - the script will generate required certificate files in `src\Workbench.Frontend\cert` folder
      - alternatively, you can use openssl and change Vite config as you wish...

## TODO

- is there any way to check if the SoftwareStatement registration is still valid using the API ?

## Known issues

- NSwag toolchain vs. deprecated parameters
    - some ADAA parameters (`fromDate`/`toDate` for endpoint `/accounts/{accountId}/transactions`) are marked as
      deprecated
    - When generating a C# client, NSwag does not automatically ignore these parameters
    - workaround is to specify them in the settings (.nswag file) in "excludedParameterNames"
- application registration flow
    - Sandbox sends the parameters in quotes ("...") to the callback - reported by KB and supposedly fixed
    - authorization flow on Sandbox does not return the `state` parameter at all - also reported but they take it as "suggestion"
    - in contrast to what is described in the documentation, the `state` parameter in the context of registering the application to the KB client account does not return
      unencrypted in the redirect URL but in the encrypted returned data (should be fixed)

## Design decisions

### Generated clients

There is OpenAPI (swagger) specification for most of the ADAA interfaces. 
However, not all clients in this solution are generated for the reasons described bellow. 

#### ClientRegistration client

Automatic code generation of the ClientRegistration client is not possible right now due to:

1. mismatch of OpenAPI spec and actual behavior
   - code 400 response sometimes does not match OpenAPI spec (different JSON shape returned)
   - server should return 201 Created, but returns 200 OK (Sandbox only)
2. NSwag toolchain (C# templates) missing support for enum collections serializable as array of strings
3. NSwag toolchain (C# templates) not handling well operations returning different content types for different response status codes

#### ADAA client

The ADAA client itself is generated from the OpenAPI specification.
However, this client is completely unusable on its own, because ADAA always works in the context of a specific KB client.
Data about this client cannot be inserted into the client API (or specific calls) in any way.
The context is made of ClientId, ClientSecret and OAuth2 Refresh token.
This information is obtained during the process of application registration (browser flow, when KB client approves the application access -
generates ClientId and ClientSecret) and authorization (again browser flow, when KB client authorizes the application to access
selected objects - here generates refresh token).

## Credits

- parts of the code around access token management are heavily inspired by amazing [Duende.AccessTokenManagement](https://www.nuget.org/packages/Duende.AccessTokenManagement).