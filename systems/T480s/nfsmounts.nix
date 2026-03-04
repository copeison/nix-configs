{
  fileSystems."/mnt/data" = {
      device = "10.0.0.152:/data/Share";
      fsType = "nfs";
      options = [ "x-systemd.automount" "noauto" "soft" ];
    };

    fileSystems."/mnt/Media" = {
      device = "10.0.0.152:/data/Media";
      fsType = "nfs";
      options = [ "x-systemd.automount" "noauto" "soft" ];
    };

    fileSystems."/mnt/downloads" = {
      device = "10.0.0.152:/data/downloads";
      fsType = "nfs";
      options = [ "x-systemd.automount" "noauto" "soft" ];
    };
}