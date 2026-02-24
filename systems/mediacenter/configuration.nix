{ config, lib, pkgs, modulesPath, ... }:
{
  imports = [
    "${modulesPath}/installer/scan/not-detected.nix"
    services/system/openssh.nix
    ./boot.nix
  ];

  environment.systemPackages = with pkgs; [
    btop
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
    hostName = "3040";
    useDHCP = true;
    usePredictableInterfaceNames = false;
  };
}