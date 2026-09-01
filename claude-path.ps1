# irm https://claude.ai/install.ps1 | iex
# powershell -ExecutionPolicy Bypass -File claude-path.ps1

$caminhoParaAdicionar = "C:\Users\alunolab11\.local\bin"
$pathAtual = [Environment]::GetEnvironmentVariable("Path", "User")

if ($pathAtual -notlike "*$caminhoParaAdicionar*") {
    $pathAtual = "$pathAtual;$caminhoParaAdicionar"
    Write-Host "Adicionado com sucesso!"
} else {
    Write-Host "Esse caminho já estava no PATH."
}

# Limpa entradas vazias e duplicadas
$partes = $pathAtual -split ';' | Where-Object { $_ -ne '' } | Select-Object -Unique
$pathFinal = $partes -join ';'

[Environment]::SetEnvironmentVariable("Path", $pathFinal, "User")
Write-Host "PATH atualizado e limpo! Reabra o terminal para aplicar."
