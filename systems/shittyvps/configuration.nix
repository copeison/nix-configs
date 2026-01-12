{ config, lib, pkgs, modulesPath, ... }:

{
  imports = [
    "${modulesPath}/installer/scan/not-detected.nix"

    modules/boot.nix
    modules/agenix.nix
    modules/wireguard.nix

    services/system/openssh.nix

  ];

  environment.systemPackages = with pkgs; [
    btop
    conntrack-tools
    dig
    fastfetch
    gdb
    git
    inetutils
    iperf
    ncdu
    ndisc6
    net-tools
    openssl
    screen
    tcpdump
    wget
  ];

  fileSystems = {
    "/" = {
      device = "zpool/root";
      fsType = "zfs";
    };
    "/home" = {
      device = "zpool/home";
      neededForBoot = true;
      fsType = "zfs";
    };
    "/nix" = {
      device = "zpool/nix";
      neededForBoot = true;
      fsType = "zfs";
    };
    "/boot" = {
      label = "NIXOS_BOOT";
      fsType = "ext4";
    };
  };

  networking = {
    defaultGateway = "74.208.73.1";
    defaultGateway6 = {
      address = "fe80::1";
      interface = "eth0";
    };
    firewall = {
      allowedTCPPorts = [
        80 # HTTP
        443 # HTTPS
        3700 # Peer port
        5201 # iperf3
        9101 # Node Exporter
      ];
      allowedUDPPorts = [
        3700 # Peer port
        5201 # iperf3
        6990 # DHT
      ];
    };
    hostId = "55354a89";
    hostName = "shittyvps";
    interfaces.eth0 = {
      ipv4.addresses = [{
        address = "74.208.73.245";
        prefixLength = 24;
      }];
      ipv6.addresses = [{
        address = "2607:f1c0:f01e:fa00::1";
        prefixLength = 80;
      }];
    };
    nameservers = [
      "1.1.1.1"
      "8.8.8.8"
      "2001:4860:4860::8888"
      "2606:4700:4700::1111"
    ];
    useDHCP = false;
    usePredictableInterfaceNames = false;
  };

  security.acme = {
    acceptTerms = true;
    defaults.email = "unisonsolos@gmail.com";
  };

  # 1GiB Swap
  swapDevices = [{
    device = "/dev/disk/by-label/NIXOS_SWAP";
  }];
}