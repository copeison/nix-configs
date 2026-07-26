{ config, pkgs, lib, ... }:
{

  users.users.ethan.isNormalUser = true;
  home-manager.users.unison = { pkgs, ... }: {
    imports = [
      programs/fastfetch.nix
      programs/git.nix
      programs/zsh.nix
    ];

    programs.bash.enable = true;

    home.file.".config/alacritty".source = ./config/alacritty;
    home.file.".config/alacritty".recursive = true;
    home.file.".config/omz-custom".source = ./config/omz-custom;
    home.file.".config/omz-custom".recursive = true;

    home.username = "unison";
    home.homeDirectory = "/home/unison";
    home.packages = [
      pkgs.qimgv
      pkgs.spotify
      pkgs.makemkv
      pkgs.audacity
      pkgs.kodi
      pkgs.kodiPackages.inputstream-adaptive
      pkgs.kodiPackages.jellyfin
    ];

    nixpkgs.config.allowUnfree = true;

    # The state version is required and should stay at the version you
    # originally installed.
    home.stateVersion = "25.11";
  };
}
