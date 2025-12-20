{ config, pkgs, lib, ... }:

let
  home-manager = builtins.fetchTarball https://github.com/nix-community/home-manager/archive/master.tar.gz;
in
{
  imports =
    [
      (import "${home-manager}/nixos")
    ];

  users.users.ethan.isNormalUser = true;
  home-manager.users.ethan = { pkgs, ... }: {
    imports = [
      programs/fastfetch.nix
      programs/git.nix
      programs/zsh.nix
    ];

    programs.bash.enable = true;

    # Add configs for programs
    home.file.".config/waybar".source = ./config/waybar;
    home.file.".config/waybar".recursive = true;
    home.file.".config/rofi".source = ./config/rofi;
    home.file.".config/rofi".recursive = true;
    home.file.".local/share/rofi".source = ./themes/rofi;
    home.file.".local/share/rofi".recursive = true;
    home.file.".config/mako".source = ./config/mako;
    home.file.".config/mako".recursive = true;
    home.file.".config/hypr".source = ./config/hypr;
    home.file.".config/hypr".recursive = true;
    home.file.".config/alacritty".source = ./config/alacritty;
    home.file.".config/alacritty".recursive = true;
    home.file.".config/zsh/omz-custom".source = ./config/omz-custom;
    home.file.".config/zsh/omz-custom".recursive = true;

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

  home.pointerCursor = {
    gtk.enable = true;
    x11.enable = true;
    package = pkgs.bibata-cursors;
    name = "Bibata-Modern-Classic";
    size = 12;
    };

    home.username = "ethan";
    home.homeDirectory = "/home/ethan";
    home.packages = [
      pkgs.jetbrains.rider
      pkgs.qimgv
      pkgs.spotify
      pkgs.prismlauncher
      pkgs.gedit
      pkgs.filezilla
      pkgs.vscode
      pkgs.discord
      pkgs.fallout-ce
      pkgs.fallout2-ce
    ];

    nixpkgs.config.allowUnfree = true;

    # The state version is required and should stay at the version you
    # originally installed.
    home.stateVersion = "25.11";
  };
}
