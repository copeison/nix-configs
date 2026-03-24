{config, inputs, system, ...}:
let
  # pterodactyl-wings-nix represents both ptero and pelican wings.
  wings = inputs.pterodactyl-wings-nix.packages.${system}.pterodactyl-wings;
in
{
  environment.systemPackages = [
    wings
  ];

  virtualisation.docker.enable = true;

  services.nginx.virtualHosts."us.nv.bcdn2.r33.ca" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
      proxyPass = "http://127.0.0.1:8080";
      proxyWebsockets = true;
    };
  };

  users.groups.pterodactyl = {};
  users.users.pterodactyl = {
    isSystemUser = true;
  };
  users.users.pterodactyl.group = "pterodactyl";
  users.users.pterodactyl.extraGroups = [ "docker" ];

  services.wings = {
    enable = true;
    tokenFile = config.age.secrets.nv-bcdn2-wings-token.path;
    user = "root";
    group = "root";
    package = wings;
    config = {
      uuid = "1aa99d6a-ab6f-49c9-ab5b-401212c8aba1";
      token_id = "BGyPjpn5qN8KVp2B";
      remote = "https://client.r33.ca";
      ignore_panel_config_updates = true;
      docker = {
        container_pid_limit = 0;
      };
      api = {
        host = "127.0.0.1";
        port = 8080;
        ssl = {
          enabled = false;
        };
      };
    };
  };
}