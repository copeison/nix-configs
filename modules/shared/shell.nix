{ config, pkgs, ... }:

{
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