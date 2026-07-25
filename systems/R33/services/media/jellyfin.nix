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
}