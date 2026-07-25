{ pkgs, ... }:

{
  systemd.services.patreon-dl-server = {
    enable = true;
    description = "Patreon Dump Viewer daemon";
    serviceConfig = {
      ExecStart = "${pkgs.patreon-dl-server}/bin/PatreonDlServer";
      WorkingDirectory = "/var/lib/patreon-dl-server";
    };
    wantedBy = [ "multi-user.target" ];
  };
}