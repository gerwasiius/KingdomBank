# domain_refactor_in_place.ps1
$ErrorActionPreference = "Stop"

function Ensure-Dir { param([string]$p) if (!(Test-Path $p)) { New-Item -ItemType Directory -Path $p | Out-Null } }
function Move-And-RetargetNs {
  param([string]$src,[string]$dst,[string]$oldNs,[string]$newNs)
  if (Test-Path $src) {
    Ensure-Dir (Split-Path $dst -Parent)
    try { git mv $src $dst 2>$null } catch { Move-Item $src $dst -Force }
    # update namespace (file-scoped & block)
    (Get-Content $dst) |
      ForEach-Object {
        $_ -replace ("^\s*namespace\s+$([regex]::Escape($oldNs))\s*;","namespace $newNs;") `
           -replace ("^\s*namespace\s+$([regex]::Escape($oldNs))\b","namespace $newNs")
      } | Set-Content $dst
  }
}

Write-Host "=== Kreiranje domen-svjesnih foldera (bez novih projekata) ==="
# Bank.API
Ensure-Dir "Bank.API\Controllers\Identity"
Ensure-Dir "Bank.API\Controllers\System"

# Bank.App
Ensure-Dir "Bank.App\Identity\Security\Interfaces"
Ensure-Dir "Bank.App\Identity\Models"

# Bank.Infrastructure
Ensure-Dir "Bank.Infrastructure\Identity\Ef\Entities"
Ensure-Dir "Bank.Infrastructure\Identity\Ef\EntityConfigs"
Ensure-Dir "Bank.Infrastructure\Identity\Ef\Migrations"
Ensure-Dir "Bank.Infrastructure\Identity\Repositories"
Ensure-Dir "Bank.Infrastructure\Identity\KeyVault"

# Bank.Shared
Ensure-Dir "Bank.Shared\Identity\Contracts\Security"

# Bank.Workers (ako kasnije dodaš worker fajlove)
Ensure-Dir "Bank.Workers\Identity"

Write-Host "=== Pomjeranje fajlova i promjena namespace-a (logika ostaje ista) ==="

# --- API ---
Move-And-RetargetNs "Bank.API\Controllers\JwksController.cs" "Bank.API\Controllers\Identity\JwksController.cs" "Bank.API.Controllers" "Bank.API.Controllers.Identity"
Move-And-RetargetNs "Bank.API\Controllers\PingController.cs" "Bank.API\Controllers\System\PingController.cs" "Bank.Api.Controllers" "Bank.API.Controllers.System"

# --- APP (Application sloj) ---
Move-And-RetargetNs "Bank.App\Security\Interfaces\IKeyGenerator.cs"        "Bank.App\Identity\Security\Interfaces\IKeyGenerator.cs"        "Bank.App.Security.Interfaces" "Bank.App.Identity.Security.Interfaces"
Move-And-RetargetNs "Bank.App\Security\Interfaces\IKeyRotationRepository.cs" "Bank.App\Identity\Security\Interfaces\IKeyRotationRepository.cs" "Bank.App.Security.Interfaces" "Bank.App.Identity.Security.Interfaces"
Move-And-RetargetNs "Bank.App\Security\Interfaces\IKeyVault.cs"              "Bank.App\Identity\Security\Interfaces\IKeyVault.cs"              "Bank.App.Security.Interfaces" "Bank.App.Identity.Security.Interfaces"

Move-And-RetargetNs "Bank.App\Security\RsaKeyGenerator.cs"   "Bank.App\Identity\Security\RsaKeyGenerator.cs"   "Bank.App.Security" "Bank.App.Identity.Security"
Move-And-RetargetNs "Bank.App\Security\EcdsaKeyGenerator.cs" "Bank.App\Identity\Security\EcdsaKeyGenerator.cs" "Bank.App.Security" "Bank.App.Identity.Security"
Move-And-RetargetNs "Bank.App\Security\KeyAlgorithm.cs"      "Bank.App\Identity\Models\KeyAlgorithm.cs"       "Bank.App.Security" "Bank.App.Identity.Models"
Move-And-RetargetNs "Bank.App\Security\KeyDescriptor.cs"     "Bank.App\Identity\Models\KeyDescriptor.cs"      "Bank.App.Security" "Bank.App.Identity.Models"

# --- INFRASTRUCTURE ---
Move-And-RetargetNs "Bank.Infrastructure\BankDbContext.cs"                 "Bank.Infrastructure\Identity\Ef\BankDbContext.cs"                 "Bank.Infrastructure"               "Bank.Infrastructure.Identity.Ef"
Move-And-RetargetNs "Bank.Infrastructure\DesignTimeFactory.cs"             "Bank.Infrastructure\Identity\Ef\DesignTimeFactory.cs"             "Bank.Infrastructure"               "Bank.Infrastructure.Identity.Ef"
Move-And-RetargetNs "Bank.Infrastructure\Entities\KeyRotationEntity.cs"    "Bank.Infrastructure\Identity\Ef\Entities\KeyRotationEntity.cs"    "Bank.Infrastructure.Entities"      "Bank.Infrastructure.Identity.Ef.Entities"
Move-And-RetargetNs "Bank.Infrastructure\EntityConfigs\KeyRotationConfig.cs" "Bank.Infrastructure\Identity\Ef\EntityConfigs\KeyRotationConfig.cs" "Bank.Infrastructure.EntityConfigs" "Bank.Infrastructure.Identity.Ef.EntityConfigs"

# Repozitoriji & KeyVault (ako postoje pod Security/)
if (Test-Path "Bank.Infrastructure\Security\KeyRotationEfRepository.cs") {
  Move-And-RetargetNs "Bank.Infrastructure\Security\KeyRotationEfRepository.cs" "Bank.Infrastructure\Identity\Repositories\KeyRotationEfRepository.cs" "Bank.Infrastructure.Security" "Bank.Infrastructure.Identity.Repositories"
}
if (Test-Path "Bank.Infrastructure\Security\FileKeyVault.cs") {
  Move-And-RetargetNs "Bank.Infrastructure\Security\FileKeyVault.cs" "Bank.Infrastructure\Identity\KeyVault\FileKeyVault.cs" "Bank.Infrastructure.Security" "Bank.Infrastructure.Identity.KeyVault"
}

# EF Migrations (zadrži sadržaj; samo izmijeni namespace prema novoj mapi)
Get-ChildItem "Bank.Infrastructure\Migrations" -Filter *.cs -File -ErrorAction SilentlyContinue | ForEach-Object {
  $dst = Join-Path "Bank.Infrastructure\Identity\Ef\Migrations" $_.Name
  try { git mv $_.FullName $dst 2>$null } catch { Move-Item $_.FullName $dst -Force }
  (Get-Content $dst) |
    ForEach-Object {
      $_ -replace ("namespace\s+Bank\.Infrastructure\.Migrations","namespace Bank.Infrastructure.Identity.Ef.Migrations")
    } | Set-Content $dst
}

# --- SHARED (Contracts) ---
Move-And-RetargetNs "Bank.Shared\Security\Jwk.cs" "Bank.Shared\Identity\Contracts\Security\Jwk.cs" "Bank.Shared.Security" "Bank.Shared.Identity.Contracts.Security"

Write-Host "=== Gotovo. Prati korake ispod za brzu provjeru. ==="
