# Pack the project
```
dotnet pack /p:Version=1.0.1
```

# Source
```
https://danielpp95.pkgs.visualstudio.com/_packaging/danielpp95/nuget/v3/index.json
```

## Add azure artifacts as nuget source
- **username** free text
- **<APIKEY>** user api key
```
dotnet nuget add source "https://danielpp95.pkgs.visualstudio.com/Test/_packaging/artifacts/nuget/v3/index.json" --name "artifacts" --username artifacts --password <APIKEY>
```

# Update azure artifacts to update password (api key)
```
dotnet nuget update source artifacts --username az --password <APIKEY>
```

# Push nuget package to artifactory
```
dotnet nuget push --source "aquiOtroDaniel" --api-key <APIKEY> Infrastructure.Mongo.1.0.0.nupkg
```