{ config, lib, pkgs, ... }:

{
  imports =
    [
      modules/hardware.nix
      modules/agenix.nix
      modules/boot.nix
      #modules/dnsmasq.nix
      #modules/wireguard.nix
      home-manager/home.nix
      ./services.nix
      ./packages.nix
      ./flatpak.nix
      ./hardware-configuration.nix
    ];

  networking.hostName = "T480s";

  system.stateVersion = "25.11";

}
