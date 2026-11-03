{ config, pkgs, lib, ... }:
{

  users.users.ethan.isNormalUser = true;
  home-manager.users.ethan = { pkgs, ... }: {
    imports = [
      programs/fastfetch.nix
      programs/git.nix
      programs/zsh.nix
      programs/sway.nix
    ];

    programs.bash.enable = true;

    home.file.".config/alacritty".source = ./config/alacritty;
    home.file.".config/alacritty".recursive = true;
    home.file.".config/omz-custom".source = ./config/omz-custom;
    home.file.".config/omz-custom".recursive = true;
    home.file.".config/waybar".source = ./config/waybar;
    home.file.".config/waybar".recursive = true;
    home.file.".config/rofi".source = ./config/rofi;
    home.file.".config/rofi".recursive = true;
    home.file.".local/share/rofi".source = ./themes/rofi;
    home.file.".local/share/rofi".recursive = true;
    home.file.".config/mako".source = ./config/mako;
    home.file.".config/mako".recursive = true;

    home.username = "ethan";
    home.homeDirectory = "/home/ethan";
    home.packages = [
      pkgs.qimgv
      pkgs.spotify
      pkgs.prismlauncher
      pkgs.gedit
      pkgs.filezilla
      pkgs.vscode
      pkgs.fallout-ce
      pkgs.fallout2-ce
      pkgs.makemkv
      #pkgs.audacity
      pkgs.qbittorrent
      pkgs.kodi
      pkgs.jellyfin-media-player
      pkgs.parsec-bin
      pkgs.p4v
      pkgs.libreoffice-qt
      pkgs.mpv
      pkgs.vlc
      pkgs.heroic
      pkgs.lutris
      pkgs.obs-studio
    ];

    gtk = {
      enable = true;
      gtk3.extraConfig.gtk-application-prefer-dark-theme = 1;
      iconTheme = {
        name = "Papirus-Dark";
        package = pkgs.papirus-icon-theme;
      };
      theme = {
        name = "Adwaita-dark";
        package = pkgs.gnome-themes-extra;
      };
    };

  dconf = {
  enable = true;
  settings = {
      "org/cinnamon/desktop/applications/terminal" = {
        exec = "alacritty";
      };
    };
  };

  home.pointerCursor = {
    enable = true;
    gtk.enable = true;
    x11.enable = true;
    package = pkgs.bibata-cursors;
    name = "Bibata-Modern-Classic";
    size = 12;
    };

    nixpkgs.config.allowUnfree = true;
    wayland.windowManager.sway.checkConfig = false;

    # The state version is required and should stay at the version you
    # originally installed.
    home.stateVersion = "25.11";
  };
}
