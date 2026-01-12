{config, ...}:
{
  # Define VPN network namespace
  vpnNamespaces.wg = {
    enable = true;
    wireguardConfigFile = config.age.secrets.wg0.path;
    accessibleFrom = [
      "192.168.0.0/24"
    ];
    portMappings = [
      #{ from = 9091; to = 9091; }
    ];
  };
}