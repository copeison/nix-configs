{
  services.prowlarr = {
    enable = true;
    openFirewall = true;
  };

  services.nginx.virtualHosts."prowlarr.pawjob.online" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://127.0.0.1:9696";
        proxyWebsockets = true;
    };
  };
}