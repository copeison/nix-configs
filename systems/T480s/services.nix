{ ... }:

{
  #services.xserver.displayManager.lightdm.enable = true;

  services.desktopManager.cosmic.enable = true;

  services.displayManager.cosmic-greeter.enable = true;

  services.flatpak.enable = true;

  services.lact.enable = true;

  services.upower.enable = true;

  services.libinput.enable = true;
  
}
