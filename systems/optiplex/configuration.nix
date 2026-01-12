{ config, lib, pkgs, modulesPath, ... }:

let
mkNamespace = { name ? "netns" }: {
      NetworkNamespacePath = "/run/${name}/container";
      InaccessiblePaths = [
        "/run/nscd"
        "/run/resolvconf"
      ];
    };
in {
  imports = [
    "${modulesPath}/installer/scan/not-detected.nix"
    modules/agenix.nix
    modules/boot.nix
    services/system/openssh.nix
    services/web/dockge.nix
    services/web/biolink.nix
    services/web/nginx.nix
    #services/vpn/confinement.nix
    services/vpn/wireguard.nix
    services/torrenting/flood.nix
    services/torrenting/rtorrent.nix
    services/media/jellyfin.nix
    services/media/prowlarr.nix
    services/media/radarr.nix
    services/media/sonarr.nix
    services/fileshare/nfs.nix
    services/fileshare/samba.nix
    services/local/pihole.nix
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
    wireguard-tools
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
    firewall = {
      allowedTCPPorts = [
        6969 # BioLink site
      ];
      allowedUDPPorts = [
        51820 # WireGuard
      ];
    };
    hostId = "eca03077";
    hostName = "r33-local";
    useDHCP = true;
    usePredictableInterfaceNames = false;
  };

  systemd.services = {
    rtorrent.serviceConfig = mkNamespace {};
  };

  security.acme = {
    acceptTerms = true;
    defaults.email = "unisonsolos@gmail.com";
  };
}