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
      ../../modules/shared/gnupg.nix
      ../../modules/shared/services.nix

      ./hardware-configuration.nix
      modules/hardware.nix
      modules/boot.nix
      home-manager/home.nix
      ./services.nix
      ./packages.nix
    ];

  networking.hostName = "T430";

  system.stateVersion = "25.11";

}