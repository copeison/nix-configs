{ config, lib, pkgs, ... }:

{
  services.zipline = {
    enable = true;
    environmentFiles = [ config.age.secrets.zipline.path ];
    settings = {
      CORE_HOSTNAME = "0.0.0.0";
      CORE_PORT = 3000;
      DATASOURCE_LOCAL_DIRECTORY = "/data/Services/zipline/uploads";
      DATASOURCE_TYPE = "local";
    };
  };

  systemd.services.zipline.serviceConfig.ReadWritePaths = [ "/data/Services/zipline/uploads" ];

  services.nginx.virtualHosts."s.yutsu.wtf" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://192.168.100.1:3000";
        proxyWebsockets = true;
    };
  };

  services.nginx.virtualHosts."furryporn.ca" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://192.168.100.1:3000";
        proxyWebsockets = true;
    };
  };

  services.nginx.virtualHosts."s.r33.ca" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://192.168.100.1:3000";
        proxyWebsockets = true;
    };
  };

  services.nginx.virtualHosts."catgirltitjob.online" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://192.168.100.1:3000";
        proxyWebsockets = true;
    };
  };
}