{ config, lib, pkgs, modulesPath, ... }:
{
  imports = [
    "${modulesPath}/installer/scan/not-detected.nix"
    ../../modules/networking/nfsmounts.nix
    ../../modules/networking/hosts.nix

    services/system/openssh.nix
    home-manager/home.nix
    ./boot.nix
  ];

  nixpkgs.config.allowUnfree = true;
  environment.systemPackages = with pkgs; [
    btop
    conntrack-tools
    dig
    fastfetch
    firefox
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

  services.pipewire = {
    enable = true;
    alsa.enable = true;
    alsa.support32Bit = true;
    pulse.enable = true;
  };

  services.desktopManager.plasma6.enable = true;

  services.displayManager = {
    sddm = {
      enable = true;
      wayland.enable = true;
      theme = "breeze";
    };
  };

  xdg.portal = {
    enable = true;
    
    extraPortals = [ pkgs.kdePackages.xdg-desktop-portal-kde ]; 
  };

  services.displayManager.sddm.settings = {
    Autologin = {
      Session = "plasma";
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