{ config, lib, pkgs, modulesPath, ... }:

let
  mkNamespace = { name ? "netns" }: {
    NetworkNamespacePath = "/run/${name}/container";
    InaccessiblePaths = [
      "/run/nscd"
      "/run/resolvconf"
    ];
    BindReadOnlyPaths = [ "/etc/netns-resolv.conf:/etc/resolv.conf" ];
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
    services/web/zipline.nix
    services/web/mail/mailserver.nix
    services/web/mail/roundcube.nix
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

    environment.etc."netns-resolv.conf".text = ''
    nameserver 1.1.1.1
    nameserver 8.8.8.8
    nameserver 2001:4860:4860::8888
    nameserver 2606:4700:4700::1111
    options edns0
  '';

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
    python315
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
    extraHosts = ''
      10.0.0.172 jellyfin.localnet ${config.networking.hostName}
      10.0.0.172 kvm.localnet ${config.networking.hostName}
      10.0.0.172 pihole.localnet ${config.networking.hostName}
    '';
    firewall = {
      allowedTCPPorts = [
        3700
        6969 # BioLink site
        8080
      ];
      allowedUDPPorts = [
        3700
        6990
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
    nginx.serviceConfig = mkNamespace {};
    dovecot.serviceConfig = mkNamespace {};
    postfix.serviceConfig = mkNamespace {};
    postfix-setup.serviceConfig = mkNamespace {};
    rspamd.serviceConfig = mkNamespace {};
  };

  security.acme = {
    acceptTerms = true;
    defaults.email = "unisonsolos@gmail.com";
  };
}