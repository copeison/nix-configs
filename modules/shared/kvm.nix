{ config, pkgs, ... }:

{
  virtualisation.libvirtd = {
  enable = true;
  qemu.vhostUserPackages = with pkgs; [ virtiofsd ];
    };
  programs.virt-manager.enable = true;
  environment.systemPackages = with pkgs; [
  dnsmasq
    ];
  virtualisation.spiceUSBRedirection.enable = true;
  networking.firewall.trustedInterfaces = [ "virbr0" ];

  users.users.ethan.extraGroups = [ "libvirtd" ];
}