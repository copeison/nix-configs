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
      programs/zsh.nix
    ];

    programs.bash.enable = true;

    # Add configs for programs
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

    home.username = "ethan";
    home.homeDirectory = "/home/ethan";

    home.packages = [
      pkgs.filezilla
    ];

    # The state version is required and should stay at the version you
    # originally installed.
    home.stateVersion = "25.11";
  };
}
