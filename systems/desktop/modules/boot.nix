{ config, ... }:

{
  boot.kernelModules = [ "uinput" ];
  boot.blacklistedKernelModules = [ "libwacom" ];
}