{ config, pkgs, ... }:

{
  xdg.portal = {
      enable = true;
      extraPortals = [
        pkgs.xdg-desktop-portal
      ];
      xdgOpenUsePortal = false;
    };
}