# .NET Integrace na API ADA (Account Direct Access) Komercni banky

## Supported runtimes

- .NET 6+

## DEV setup

- backend (`Workbench.Api`) je by default nakonfigurovany na praci proti KB Sandboxu (config key
  `Workbench:TargetEnvironment` - hodnota "Sandbox" nebo "Production")
- pred spustenim je treba pro zvolene cilove prostredi nakonfigurovat API keys (ziskate
  v [KB Developer portal](https://developers.kb.cz/) po registraci)

- Sandbox secrets
    - viz. `Data\set-sandbox-secrets.ps1`
- Production secrets
    - viz. `Data\set-production-secrets.ps1`
    - stejne jako u Sandbox je treba vygenerovat API keys
    - navic je treba zvolit
      si [sifrovaci klic](https://github.com/komercka/adaa-client/wiki/03-Application-Registration-OAuth2#application-registration---oauth2)
      pro registraci aplikace - `ADAA:Production:ApplicationRegistration:EncryptionKey` (32 bytu dlouhy klic zakodovany
      pomoci Base64)
        - pro Sandbox je vlozen primo v app config nebot se zde musi pouzit presne predepsana hodnota
    - pri provozu proti Production je treba mit take nakonfigurovany validni certifikat s privatnim klicem
      `Workbench:Certificate` a `Workbench:CertificatePassword`

## TODO

- je nejaka moznost jak pomoci API zjistit, zda je SoftwareStatement registrace stale platna ?

## Known issues

- NSwag toolchain vs. deprecated parameters
    - nektere ADAA parametry (`fromDate`/`toDate` pro endpoint `/accounts/{accountId}/transactions`) jsou oznaceny jako
      deprecated
    - Pri generovani C# klienta NSwag tyto parametry neignoruje automaticky
    - workaround je uvest je v nastaveni (.nswag soubor) v "excludedParameterNames"
- application registration flow
    - Sandbox posila na callback parametry v uvozovkach ("...") - nahlaseno KB a pry opravi
    - authorization flow na Sandboxu vubec nevraci parametr `state` - take nahlaseno ale berou to jako "suggestion"
    - oproti tomu, co je popsano v dokumentaci se parametr `state` v ramci registrace aplikace k uctu KB klienta nevraci
      zpet nezasifrovany v redirect URL ale v ramci zasifrovanych vracenych dat (melo by byt opraveno)

## Design decisions

### Generovany klient

ADAA klient samotny je generovany z OpenAPI specifikace.
Nicmene tento klient je samostatne zcela nepouzitelny, protoze ADAA vzdy pracuje v kontextu konkretniho klienta KB.
Udaje o tomto klientovi nelze do API klienta (respektive konkretnich volani) nijak vlozit.
Kontext je tvoren z ClientId, ClientSecret a OAuth2 Refresh tokenu.
Tyto informace jsou ziskany behem procesu registrace aplikace (browser flow kdy klient KB schvali pristup aplikace -
vznika ClientId a ClientSecret) a autorizace (opet browser flow, kdy klient KB autorizuje aplikaci pro pristup k
vybranym uctum - zde vznika refresh token).

### Pouziti externi library

Vzhledem k pozadovane funkcionalite byla samozrejme prvni uvaha o pouziti nejake existujici OSS library.
Vetsina vyhledavani vracela odkazy na [Duende.IdentityModel](https://www.nuget.org/packages/Duende.IdentityModel/)
a [Duende.AccessTokenManagement](https://www.nuget.org/packages/Duende.AccessTokenManagement).
Prvni z nich implementuje zakladni infrastrukturu pro praci s OAuth2 a OpenID Connect.
Druha pak [automatizuje praci s Access tokens](https://docs.duendesoftware.com/accesstokenmanagement/workers/) (
automaticke pripojeni Access Token k requestu a jeho refresh v pripade, ze se vrati 401)

#### [Duende.IdentityModel](https://www.nuget.org/packages/Duende.IdentityModel/)

- nakonec nemohla byt pouzita jako Nuget, protoze neni k dispozici pro .NET 6.0
- potrebne casti kodu byly prevzaty z [GitHubu](https://github.com/DuendeSoftware/foss) (commit hash:
  dcadd315f84c2dee860e7ffd3a728558f292f980) a vznikla tak asssembly LinkSoft.OAuth
- Struktura projektu a názvy souborů/tříd jsou záměrně zachovány stejné, aby budoucí migrace na balíčky Duende NuGet (v
  případě potřeby) byla co nejjednodušší

#### [Duende.AccessTokenManagement](https://www.nuget.org/packages/Duende.AccessTokenManagement)

- podobne jako predchozi nemohla byt pouzita, protoze potrebujeme podporu pro .NET 6.0
- navic obsazena funkcionalita neumoznuje implementovat to, co potrebujeme:
    1. predpoklada ziskavani Access Token za pomoci Client Credentials flow (predani ClientId a ClientSecret) ve kterem
       vubec nefiguruje RefreshToken (toto nelze zmenit)
    2. Predpoklada, ze ClientId/ClientSecret pouzite pri refresh jsou konstantni (vazana na konkretni jednu instanci
       HttpClient)
        - ADAA ale pouziva jednu sadu ClientId/ClientSecret (a pro ne generovane RefreshToken a nasledne AccessToken)
          pro kazdy "napojeny" ucet klienta KB