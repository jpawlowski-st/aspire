$config = "Release"
$targets = @{
    "publish.dashboard" = {
        $out = "publish\dashboard\win-x64"
        # make sure we don't use any leftover files from previous publishes
        if (Test-Path $out) {
            write-host "Removing old publish artifacts..."
            Remove-Item $out -Recurse -Force
        }


        Write-host "publishing dashoard..."
        dotnet publish src\Aspire.Dashboard\Aspire.Dashboard.csproj -c $config -r win-x64 --self-contained true -o $out
    }
    "build.dashboard" = {
        dotnet build src\Aspire.Dashboard\Aspire.Dashboard.csproj -c $config
    }
    "start.dashboard" = {
        param([switch][bool]$watch = $false)

        $env:ASPIRE_RESOURCE_SERVICE_ENDPOINT_URL = "http://localhost:20261"
        $env:DASHBOARD__RESOURCESERVICECLIENT__AUTHMODE = "ApiKey"
        $env:DASHBOARD__RESOURCESERVICECLIENT__APIKEY = "local-dev-key"

        $env:ASPIRE_DASHBOARD_OTLP_ENDPOINT_URL = "http://localhost:4319"
        $env:ASPIRE_DASHBOARD_OTLP_HTTP_ENDPOINT_URL = "http://localhost:4320"
        $env:DASHBOARD__FRONTEND__AUTHMODE = "Unsecured"
        $env:DASHBOARD__FRONTEND__BROWSERTOKEN = "local-dev-token"

        $w = if ($watch) { "watch" } else { "" }
        dotnet $w run --project src\Aspire.Dashboard\Aspire.Dashboard.csproj
    }
    "clean" = {
        rm "artifacts" -Recurse -Force
        rm "publish" -Recurse -Force
        rm .\src\Aspire.Dashboard\artifacts -Recurse -Force
    }
    "restore" = {
        dotnet restore
    }
}

return $targets
