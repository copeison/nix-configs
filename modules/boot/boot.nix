{ config, lib, pkgs, ... }:
{
  boot.loader = {
    efi = {
      canTouchEfiVariables = true;
    };
    grub = {
      enable = true;
      configurationLimit = 100;
      copyKernels = true;
      device = "nodev";
      efiSupport = true;
      efiInstallAsRemovable = false;
      memtest86.enable = true;
      useOSProber = true;
    };
    timeout = 10;
  };
}