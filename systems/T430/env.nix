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

  xdg.portal = {
      enable = true;
      extraPortals = [
        pkgs.xdg-desktop-portal
      ];
      xdgOpenUsePortal = false;
    };

  xdg.mime = {
      addedAssociations = {
        "x-scheme-handler/ftp" = "app.zen_browser.zen.desktop";
        "x-scheme-handler/http" = "app.zen_browser.zen.desktop";
        "x-scheme-handler/https" = "app.zen_browser.zen.desktop";
      };
      defaultApplications = {
        "application/xhtml+xml" = "app.zen_browser.zen.desktop";
        "text/html" = "app.zen_browser.zen.desktop";
        "text/xml" = "app.zen_browser.zen.desktop";
      };
    };

  qt = {
    enable = true;
    platformTheme = "lxqt";
    style = "adwaita-dark";
  };
}
