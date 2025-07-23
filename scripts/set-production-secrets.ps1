function Set-Secret {
    param (
        [string]$SecretKey,
        [string]$SecretValue
    )
    
    Write-Host "Setting secret $SecretKey" -ForegroundColor DarkGray
                
    dotnet user-secrets set $SecretKey $SecretValue --project "../Workbench.Api/Workbench.Api.csproj"
                
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Failed to set secret $SecretKey" -ForegroundColor Red
    }
}

Set-Secret -SecretKey "ADAA:Production:SoftwareStatementsEndpoint:ApiKey" -SecretValue "== SET VALUE HERE =="
Set-Secret -SecretKey "ADAA:Production:ApplicationRegistration:EncryptionKey" -SecretValue "== SET VALUE HERE =="
Set-Secret -SecretKey "ADAA:Production:TokenEndpoint:ApiKey" -SecretValue "== SET VALUE HERE =="
Set-Secret -SecretKey "ADAA:Production:AccountDirectAccessEndpoint:ApiKey" -SecretValue "== SET VALUE HERE =="
Set-Secret -SecretKey "Workbench:Certificate" -SecretValue "== SET VALUE HERE =="
Set-Secret -SecretKey "Workbench:CertificatePassword" -SecretValue "== SET VALUE HERE =="

