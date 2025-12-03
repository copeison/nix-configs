{ config, ... }:
{
  fileSystems."/mnt/data" = {
      device = "10.0.0.254:/data/Share";
      fsType = "nfs";
      options = [ "x-systemd.automount" "noauto" "soft" ];
    };
}