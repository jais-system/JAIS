dotnet publish -c Release -o Build -r linux-arm -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true src/JAIS/JAIS.csproj;
#dotnet publish -c Release -o Build -r linux-arm -p:PublishSingleFile=true --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true src/JAIS/JAIS.csproj;
