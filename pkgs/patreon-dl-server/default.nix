{
  lib,
  buildDotnetModule,
  dotnet-sdk_8,
  dotnet-aspnetcore_8,
}:

buildDotnetModule {
  pname = "patreon-dl-server";
  version = "0.1.0";

  src = ./.;
  projectFile = "src/PatreonDlServer/PatreonDlServer.csproj";
  nugetDeps = ./deps.json;

  executables = [ "PatreonDlServer" ];

  dotnet-sdk = dotnet-sdk_8;
  dotnet-runtime = dotnet-aspnetcore_8;

  selfContainedBuild = false;
  useAppHost = true;

  meta = with lib; {
    description = "Patreon Dump Viewer";
    platforms = platforms.linux;
    mainProgram = "PatreonDlServer";
  };
}