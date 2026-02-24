{ config, ...}:
{
  networking.wg-quick.interfaces = {
    wg0 = {
      address = [
        "10.0.1.3/32"
      ];
      # use dnscrypt, or proxy dns as described above
      dns = [ "10.0.1.1"
              "1.1.1.1"
              "8.8.8.8"
              "bcdn.internal"
            ];
      privateKeyFile = config.age.secrets.wireguard-ps3.path;
      peers = [
        {
          # bt wg conf
          publicKey = "/KhgPQJ/P63bg2ugPVKK1r25wCzdYZCx4o5+UKON6is=";
          allowedIPs = [
            "172.16.0.0/24"
          ];
          endpoint = "71.94.197.125:51821";
        }
      ];
    };
  };
}