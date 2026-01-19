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
}