{ config, ... }:

{
  services.sonarr = {
    enable = true;
    user = config.services.jellyfin.user;
    group = config.services.jellyfin.group;
    openFirewall = true;
  };

  services.nginx.virtualHosts."sonarr.pawjob.online" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://127.0.0.1:8989";
        proxyWebsockets = true;
    };
  };
}