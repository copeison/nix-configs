{ pkgs, ... }:

{
  nixpkgs.config.allowUnfree = true;

  programs.firefox.enable = true;
  programs.windowmaker.enable = true;

  # List packages installed in system profile.
  # You can use https://search.nixos.org/ to find more packages (and options).
  environment.systemPackages = with pkgs; [
    wget
    curl
    nano
    git
    xterm
    mpv
    vlc
    fastfetch
    xdg-desktop-portal-gtk
    xdg-user-dirs
    libarchive
    unrar
    btop
    fontconfig
    nfs-utils
    nemo
    nemo-with-extensions
    nemo-fileroller
    file-roller
  ];
}
