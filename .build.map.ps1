$framework = "net8.0"
$targets = @{
    "publish.dashboard" = {
        dotnet publish src\Aspire.Dashboard\Aspire.Dashboard.csproj -c Debug -r win-x64 --self-contained false -o publish\dashboard\win-x64
    }
    "build.dashboard" = {
        dotnet build src\Aspire.Dashboard\Aspire.Dashboard.csproj -c Debug
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
        dotnet $w run -f $framework --project src\Aspire.Dashboard\Aspire.Dashboard.csproj
    }
    "clean" = {
        rm "artifacts" -Recurse -Force
        rm .\src\Aspire.Dashboard\artifacts -Recurse -Force
    }
    "restore" = {
        dotnet restore
    }
}

return $targets
