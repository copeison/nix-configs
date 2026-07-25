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
}