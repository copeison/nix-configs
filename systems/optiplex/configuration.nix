{ config, lib, pkgs, modulesPath, ... }:
{
  imports = [
    "${modulesPath}/installer/scan/not-detected.nix"
    services/system/openssh.nix
    ./boot.nix
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
    minica
    ncdu
    ndisc6
    net-tools
    openssl
    screen
    tcpdump
    wget
  ];

  services.flatpak.enable = true;

  services.displayManager = {
    sddm = {
      enable = true;
      wayland.enable = true;
      theme = "breeze";
    };
    sessionPackages = [ pkgs.kdePackages.plasma-bigscreen ];
    defaultSession = "plasma-bigscreen-wayland";
  };

  xdg.portal = {
    enable = true;
    
    extraPortals = [ pkgs.kdePackages.xdg-desktop-portal-kde ]; 
    
    configPackages = [ pkgs.kdePackages.plasma-bigscreen ];
  };

  services.displayManager.sddm.settings = {
    Autologin = {
      Session = "plasma-bigscreen-wayland";
      User = "unison";
    };
  };

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
    hostName = "optiplex";
    useDHCP = true;
    usePredictableInterfaceNames = false;
  };

  hardware.graphics = {
    enable = true;
    extraPackages = with pkgs; [
      intel-vaapi-driver
      libvdpau-va-gl
    ];
  };

  environment.sessionVariables = {
    LIBVA_DRIVER_NAME = "i965";
  };

  security.acme = {
    acceptTerms = true;
    defaults.email = "unisonsolos@gmail.com";
  };
}