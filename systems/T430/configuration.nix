# Edit this configuration file to define what should be installed on
# your system. Help is available in the configuration.nix(5) man page, on
# https://search.nixos.org/options and in the NixOS manual (`nixos-help`).

{ config, lib, pkgs, ... }:

{
  imports =
    [ # Include the results of the hardware scan.
      ./hardware-configuration.nix
      home-manager/home.nix
      pkgs/overlays.nix
      ./nix-settings.nix
      ./services.nix
      ./networking.nix
      ./packages.nix
      ./users.nix
      ./env.nix
      ./filesystem.nix
    ];

  # Use the systemd-boot EFI boot loader.
  boot.loader.systemd-boot.enable = true;
  boot.loader.efi.canTouchEfiVariables = true;

  hardware.graphics = {
    enable = true;
    enable32Bit = true;
  };

  hardware = {
    bluetooth.enable = true;
    opentabletdriver = {
      enable = true;
      daemon.enable = true;
    };
  };

  # Required by OpenTabletDriver
  hardware.uinput.enable = true;
  boot.kernelModules = [ "uinput" ];

  # You dont need to change this. it dosen't mean your system is out of date.
  system.stateVersion = "25.11";

}

