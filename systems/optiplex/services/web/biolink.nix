{ pkgs, ... }:

{
  systemd.services.biolink = {
    enable = true;
    description = "BioLink daemon";
    serviceConfig = {
      ExecStart = "${pkgs.nodejs_25}/bin/node ${pkgs.biolink}/lib/node_modules/biolink/server.js";
    };
    wantedBy = [ "multi-user.target" ];
  };

  services.nginx.virtualHosts."pawjob.online" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
      proxyPass = "http://127.0.0.1:6969";
      proxyWebsockets = true;
      extraConfig = ''
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $remote_addr;
        proxy_set_header CF-Connecting-IP $remote_addr;
      '';
    };
  };
}