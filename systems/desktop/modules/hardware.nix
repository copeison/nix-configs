{ pkgs, ... }:

{
  hardware = {
    bluetooth.enable = true;

    opentabletdriver = {
      enable = true;
      daemon.enable = true;
    };

    uinput.enable = true;

    enableRedistributableFirmware = true;
  };


  services.pipewire = {
    enable = true;
    alsa.enable = true;
    alsa.support32Bit = true;
    pulse.enable = true;
  };
}