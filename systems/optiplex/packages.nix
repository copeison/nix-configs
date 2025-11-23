{ pkgs, ... }:

{
  nixpkgs.config.allowUnfree = true;

  programs.firefox.enable = true;

  # List packages installed in system profile.
  # You can use https://search.nixos.org/ to find more packages (and options).
  environment.systemPackages = with pkgs; [
    wget
    curl
    nano
    git
    blueman
    rofi
    discord
    mpv
    vlc
    cider2
    fastfetch
    xdg-user-dirs
    wl-clipboard
    pwvucontrol
    libarchive
    unrar
    gedit
    btop
  ];

  fonts.packages = with pkgs; [nerd-fonts.roboto-mono];
}
