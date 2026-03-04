{ ... }:

{
  # Enable the X11 windowing system.
  services.xserver.enable = true;

  # Enable lightdm
  services.xserver.displayManager.lightdm.enable = true;

  # Enable flatpak
  services.flatpak.enable = true;

  # Enable udisks2
  services.udisks2.enable = true;

  # Enable sound.
  # services.pulseaudio.enable = true;
  # OR
  services.pipewire = {
    enable = true;
    alsa.enable = true;
    alsa.support32Bit = true;
    pulse.enable = true;
  };

  # Enable CUPS to print documents.
  # services.printing.enable = true;

  # Enable touchpad support (enabled default in most desktopManager).
  services.libinput.enable = true;
}
