{ config, lib, pkgs, modulesPath, ... }:
{
  imports = [
    "${modulesPath}/installer/scan/not-detected.nix"
    services/openssh.nix
    services/dockge.nix
    services/nginx.nix
    services/jellyfin.nix
    services/prowlarr.nix
    services/radarr.nix
    services/sonarr.nix
    services/nfs.nix
    services/pihole.nix
    services/biolink.nix
    ./boot.nix
    ./env.nix
    ./users.nix
  ];

  environment.systemPackages = with pkgs; [
    biolink
    btop
    conntrack-tools
    dig
    fastfetch
    gdb
    git
    inetutils
    iperf
    minica
    ncdu
    ndisc6
    net-tools
    openssl
    screen
    tcpdump
    wget
  ];

  hardware.cpu.intel.updateMicrocode = true;

  fileSystems = {
    "/" = {
      device = "NIXOS_ROOTFS";
      fsType = "ext4";
    };
    "/boot" = {
      label = "NIXOS_BOOT";
      fsType = "vfat";
      options = [ "fmask=0022" "dmask=0022" ];
    };
    "/data" = {
      device = "/dev/disk/by-uuid/89318a81-a434-4b0d-b04a-c1f369f8ba5d";
      fsType = "ext4";
    };
  };

  networking = {
    extraHosts = ''
      10.0.0.152 jellyfin.localnet ${config.networking.hostName}
      10.0.0.152 kvm.localnet ${config.networking.hostName}
      10.0.0.152 pihole.localnet ${config.networking.hostName}
    '';
    firewall = {
      allowedTCPPorts = [
        80 # HTTP
        443 # HTTPS
      ];
    };
    hostId = "eca03077";
    hostName = "r33-local";
    useDHCP = true;
    usePredictableInterfaceNames = false;
  };

  security.acme = {
    acceptTerms = true;
    defaults.email = "unisonsolos@gmail.com";
  };
}