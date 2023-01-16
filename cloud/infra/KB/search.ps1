Connect-AzAccount

$indexName = "KB"
$fields = @{
    "Question" = @{
        "type" = "Edm.String";
        "searchable" = $true
    };
    "Answer" = @{
        "type" = "Edm.String";
        "retrievable" = $true
    }
}

$index = New-AzSearchIndex -Name $indexName -Fields $fields -ResourceGroupName <ResourceGroupName> -ServiceName <SearchServiceName>

$jsonData = Get-Content sample.json | Out-String | ConvertFrom-Json

$uri = "https://<SearchServiceName>.search.windows.net/indexes/$indexName/docs/index?api-version=<api-version>"
Invoke-RestMethod -Uri $uri -Method Post -Body $jsonData -ContentType "application/json" -Headers @{ "api-key" = $index.PrimaryKey }