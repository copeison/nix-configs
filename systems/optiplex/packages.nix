{ pkgs, ... }:

{
  nixpkgs.config.allowUnfree = true;

  # List packages installed in system profile.
  # You can use https://search.nixos.org/ to find more packages (and options).
  environment.systemPackages = with pkgs; [
    wget
    curl
    nano
    git
    blueman
    mpv
    vlc
    cider-2
    fastfetch
    xdg-user-dirs
    pwvucontrol
    libarchive
    unrar
    gedit
    btop
  ];

  fonts.packages = with pkgs; [nerd-fonts.roboto-mono];
}
