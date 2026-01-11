{ config, ... }:

{
  services.sonarr = {
    enable = true;
    user = config.services.jellyfin.user;
    group = config.services.jellyfin.group;
    openFirewall = true;
  };
}