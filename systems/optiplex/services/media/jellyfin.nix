{ config, lib, pkgs, ... }:

{
  environment.systemPackages = [ pkgs.jellyfin-ffmpeg ];

  services.jellyfin = {
    enable = true;
    openFirewall = true;
    group = config.services.jellyfin.user;
    user = "server";
  };

  users = {
    groups.server = {};
  };

  systemd.services.jellyfin.serviceConfig.SupplementaryGroups = [
    "video"
    "render"
  ];

  services.nginx.virtualHosts."iwanta.pawjob.online" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://127.0.0.1:8096";
        proxyWebsockets = true;
    };
  };
}