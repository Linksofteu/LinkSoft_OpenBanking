function Set-Secret {
    param (
        [string]$SecretKey,
        [string]$SecretValue
    )
    
    Write-Host "Setting secret $SecretKey" -ForegroundColor DarkGray
                
    dotnet user-secrets set $SecretKey $SecretValue --project "../src/Workbench.Api/Workbench.Api.csproj"
                
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Failed to set secret $SecretKey" -ForegroundColor Red
    }
}

Set-Secret -SecretKey "ADAA:Sandbox:SoftwareStatementsEndpoint:ApiKey" -SecretValue "== SET VALUE HERE =="
Set-Secret -SecretKey "ADAA:Sandbox:TokenEndpoint:ApiKey" -SecretValue "== SET VALUE HERE =="
Set-Secret -SecretKey "ADAA:Sandbox:AccountDirectAccessEndpoint:ApiKey" -SecretValue "== SET VALUE HERE =="
