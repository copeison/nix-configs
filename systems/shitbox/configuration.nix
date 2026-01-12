{ config, lib, pkgs, modulesPath, ... }:
{
  imports = [
    "${modulesPath}/installer/scan/not-detected.nix"
    services/system/openssh.nix
    services/web/nginx.nix
    services/local/nginx.nix
    ./boot.nix
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
    hostName = "shitbox";
    useDHCP = true;
    usePredictableInterfaceNames = false;
  };

  security.acme = {
    acceptTerms = true;
    defaults.email = "unisonsolos@gmail.com";
  };
}