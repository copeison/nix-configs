{ ... }:

{
  # Enable the X11 windowing system.
  services.xserver.enable = true;

  # Enable KDE, sddm, and Wayland.
  services.displayManager.sddm.enable = true;
  services.displayManager.sddm.wayland.enable = true;
  services.desktopManager.plasma6.enable = true;

  # Enable flatpak
  services.flatpak.enable = true;

  # Enable udisks2
  services.udisks2.enable = true;

  # Enable sound.
  # services.pulseaudio.enable = true;
  # OR
  services.pipewire = {
    enable = true;
    pulse.enable = true;
  };

  # Enable CUPS to print documents.
  # services.printing.enable = true;

  # Enable touchpad support (enabled default in most desktopManager).
  services.libinput.enable = true;
}
