{config, ...}:
{
  # Define VPN network namespace
  vpnNamespaces.wg = {
    enable = true;
    wireguardConfigFile = config.age.secrets.wg0.path;
    accessibleFrom = [
      "192.168.0.0/24"
      "10.0.0.0/8"
      "127.0.0.1"
      "10.127.0.0/24"
      "fd00:127::/64"
    ];
    portMappings = [
      { from = 9091; to = 9091; }
    ];
  };
}