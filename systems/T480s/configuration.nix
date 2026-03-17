{ config, lib, pkgs, ... }:

{
  imports =
    [
      ../../modules/boot/boot.nix
      ../../modules/networking/nfsmounts.nix
      ../../modules/networking/hosts.nix
      ../../modules/networking/defaults.nix
      ../../modules/shared/locale.nix
      ../../modules/shared/users.nix
      ../../modules/shared/nix-settings.nix
      ../../modules/shared/qt.nix
      ../../modules/shared/dconf.nix
      ../../modules/shared/shell.nix
      ../../modules/shared/xdg.nix
      ../../modules/shared/gnupg.nix
      ../../modules/shared/services.nix
      ../../modules/shared/kvm.nix
      ../../modules/shared/git.nix

      ../../pkgs/overlays.nix

      ./hardware-configuration.nix
      modules/hardware.nix
      modules/agenix.nix
      modules/boot.nix
      #modules/dnsmasq.nix
      #modules/wireguard.nix
      home-manager/home.nix
      ./services.nix
      ./packages.nix
    ];

  networking.hostName = "T480s";

  system.stateVersion = "25.11";

}
