{ config, lib, pkgs, modulesPath, ... }:
{
  imports = [
    "${modulesPath}/installer/scan/not-detected.nix"
    services/system/openssh.nix
    modules/agenix.nix
    ./boot.nix
  ];

  environment.etc."netns-resolv.conf".text = ''
    nameserver 1.1.1.1
    nameserver 8.8.8.8
    nameserver 2001:4860:4860::8888
    nameserver 2606:4700:4700::1111
    options edns0
  '';

  environment.systemPackages = with pkgs; [
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
  };

  networking = {
    firewall = {
      allowedTCPPorts = [
      ];
    };
    hostId = "eca03077";
    hostName = "bcdn-nix-2";
    useDHCP = true;
    usePredictableInterfaceNames = false;
  };

  security.acme = {
    acceptTerms = true;
    defaults.email = "unisonsolos@gmail.com";
  };
}