{ config, ... }:

{
  services.flood = {
    enable = true;
    host = "0.0.0.0";
    port = 3701;
    openFirewall = true;
    extraArgs = [
      "--rtsocket=${config.services.rtorrent.rpcSocket}"
    ];
  };

  services.nginx.virtualHosts."flood.yutsu.wtf" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
      proxyPass = "http://192.168.100.1:${toString config.services.flood.port}";
      proxyWebsockets = true;
    };
  };

  systemd.services.flood.serviceConfig = {
    Environment = "HOME=/var/lib/flood";
    Group = "server";
    ReadWritePaths = [
      "/var/lib/flood"
      "/data/Media"
      "/data/downloads"
    ];
    SupplementaryGroups = [
      config.services.rtorrent.group
    ];
    User = "server";
  };
}