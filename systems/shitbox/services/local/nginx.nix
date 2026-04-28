{
  services.nginx.virtualHosts."pihole.localnet" = {
    forceSSL = false;
    locations."/" = {
      proxyPass = "http://10.0.0.152:9810";
      proxyWebsockets = true;
    };
  };
  services.nginx.virtualHosts."downloader.localnet" = {
    forceSSL = false;
    locations."/" = {
      proxyPass = "http://10.0.0.152:8081";
      proxyWebsockets = true;
    };
  };
}