{ config, ... }:
{
  fileSystems."/mnt/data" = {
      device = "10.0.0.254:/data/Share";
      fsType = "nfs";
      options = [ "x-systemd.automount" "noauto" "soft" ];
    };

    fileSystems."/mnt/Media" = {
      device = "10.0.0.254:/data/Media";
      fsType = "nfs";
      options = [ "x-systemd.automount" "noauto" "soft" ];
    };

    fileSystems."/mnt/downloads" = {
      device = "10.0.0.254:/data/downloads";
      fsType = "nfs";
      options = [ "x-systemd.automount" "noauto" "soft" ];
    };
}