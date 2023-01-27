# Create the index
$index_settings = @"
{
  "mappings": {
    "properties": {
      "Question": {
        "type": "text"
      },
      "Answer": {
        "type": "text",
        "index": "false"
      }
    }
  }
}
"@
Invoke-WebRequest -Method PUT -Uri "http://localhost:9200/kb" -ContentType 'application/json' -Body $index_settings

# Add data to the index
$data = Get-Content -Path "sample.json" -Raw
Invoke-WebRequest -Method POST -Uri "http://localhost:9200/kb/_bulk" -ContentType 'application/json' -Body $data