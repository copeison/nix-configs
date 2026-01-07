{ config, ... }:

{
  services.radarr = {
    enable = true;
    user = config.services.jellyfin.user;
    group = config.services.jellyfin.group;
    openFirewall = true;
  };

  services.nginx.virtualHosts."radarr.pawjob.online" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://127.0.0.1:7878";
        proxyWebsockets = true;
    };
  };
}