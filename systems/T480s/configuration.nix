{ config, lib, pkgs, ... }:

{
  imports =
    [
      ../../modules/shared/qt.nix
      ../../modules/shared/dconf.nix
      ../../modules/shared/shell.nix
      ../../modules/shared/xdg.nix
      ../../modules/shared/gnupg.nix
      ../../modules/shared/services.nix
      ../../modules/shared/kvm.nix
      ../../modules/shared/git.nix

      ../../pkgs/overlays.nix

      modules/hardware.nix
      modules/agenix.nix
      modules/boot.nix
      #modules/dnsmasq.nix
      #modules/wireguard.nix
      home-manager/home.nix
      ./services.nix
      ./packages.nix
      ./hardware-configuration.nix
    ];

  networking.hostName = "T480s";

  system.stateVersion = "25.11";

}
