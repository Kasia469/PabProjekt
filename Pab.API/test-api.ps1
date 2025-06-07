param(
    [string]$baseUrl = "https://localhost:7251"
)


[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = { $true }

Write-Host "1) Loguję się: $baseUrl/api/Auth/login"
$loginBody = @{
    Email    = "admin@pab.local"
    Password = "Admin!123"
} | ConvertTo-Json

try {
    $loginResp = Invoke-RestMethod -Method Post `
        -Uri "$baseUrl/api/Auth/login" `
        -ContentType "application/json" `
        -Body $loginBody
} catch {
    Write-Error "Logowanie nie powiodło się: $_"
    exit 1
}

if (-not $loginResp.token) {
    Write-Error "Nie otrzymano tokena."
    exit 1
}

$token = $loginResp.token
Write-Host "   → Token: $token`n"


Write-Host "2) Pobieram listę produktów"
try {
    $products = Invoke-RestMethod -Method Get `
        -Uri "$baseUrl/api/Products" `
        -Headers @{ Authorization = "Bearer $token" }
    $products | ConvertTo-Json -Depth 5
} catch {
    Write-Error "GET /api/Products nie powiódł się: $_"
}
Write-Host "`n"


Write-Host "3) Tworzę nowy produkt"
$newProdBody = @{
    Name  = "SkryptTest"
    Price = 42.99
} | ConvertTo-Json

try {
    $newProduct = Invoke-RestMethod -Method Post `
        -Uri "$baseUrl/api/Products" `
        -Headers @{ Authorization = "Bearer $token" } `
        -ContentType "application/json" `
        -Body $newProdBody
    $newProduct | ConvertTo-Json -Depth 5
} catch {
    Write-Error "POST /api/Products nie powiódł się: $_"
}
Write-Host "`n"


Write-Host "4) Pobieram listę zamówień"
try {
    $orders = Invoke-RestMethod -Method Get `
        -Uri "$baseUrl/api/Orders" `
        -Headers @{ Authorization = "Bearer $token" }
    $orders | ConvertTo-Json -Depth 5
} catch {
    Write-Error "GET /api/Orders nie powiódł się: $_"
}
Write-Host "`n"
