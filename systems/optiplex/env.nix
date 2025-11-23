{ config, pkgs, ... }:

{
  # Select internationalisation properties.
  i18n.defaultLocale = "en_CA.UTF-8";
  # console = {
    # font = "Lat2-Terminus16";
    # keyMap = "us";
    # useXkbConfig = true; # use xkb.options in tty.
  # };

  environment.shells = [ pkgs.zsh ];
  programs.zsh = {
    enable = true;
    shellInit = ''
      export XDG_DATA_DIRS="/var/lib/flatpak/exports/share:/home/ethan/.local/share/flatpak/exports/share:$XDG_DATA_DIRS"
      export PATH=/home/ethan/.local/bin:$PATH
    '';
  };
  users.defaultUserShell = pkgs.zsh;
}
