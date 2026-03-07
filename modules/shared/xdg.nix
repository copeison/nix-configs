{ config, pkgs, ... }:

{
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
}