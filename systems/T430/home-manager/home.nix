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
    ];

    programs.bash.enable = true;


    home.username = "ethan";
    home.homeDirectory = "/home/ethan";
    home.packages = [
      pkgs.vscode
    ];

    nixpkgs.config.allowUnfree = true;

    # The state version is required and should stay at the version you
    # originally installed.
    home.stateVersion = "25.11";
  };
}
