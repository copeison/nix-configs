{ pkgs, ... }:

{
  hardware = {
    graphics = {
      enable = true;
      enable32Bit = true;

      extraPackages = with pkgs; [
        intel-media-driver
      ];

      extraPackages32 = with pkgs; [
        driversi686Linux.intel-media-driver
      ];
    };

    bluetooth.enable = true;

    opentabletdriver = {
      enable = true;
      daemon.enable = true;
    };

    uinput.enable = true;

    enableRedistributableFirmware = true;
  };

  environment.sessionVariables = {
    LIBVA_DRIVER_NAME = "iHD";
  };

  services.pipewire = {
    enable = true;
    alsa.enable = true;
    alsa.support32Bit = true;
    pulse.enable = true;
  };
}