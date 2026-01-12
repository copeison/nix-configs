{ config, lib, pkgs, modulesPath, ... }:
{
  imports = [
    "${modulesPath}/installer/scan/not-detected.nix"
    services/system/openssh.nix
    services/web/dockge.nix
    services/web/biolink.nix
    services/web/nginx.nix
    services/media/jellyfin.nix
    services/media/prowlarr.nix
    services/media/radarr.nix
    services/media/sonarr.nix
    services/fileshare/nfs.nix
    services/fileshare/samba.nix
    services/local/pihole.nix
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
      device = "/dev/disk/by-label/NIXOS_ROOTFS";
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
        6969 # BioLink site
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