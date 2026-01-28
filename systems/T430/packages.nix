{ pkgs, ... }:

{
  nixpkgs.config.allowUnfree = true;

  programs.firefox.enable = true;
  programs.hyprland.enable = true;

  programs.steam = {
  enable = true;
  remotePlay.openFirewall = true;
  dedicatedServer.openFirewall = true;
  };

  # List packages installed in system profile.
  # You can use https://search.nixos.org/ to find more packages (and options).
  environment.systemPackages = with pkgs; [
    wget
    curl
    nano
    git
    (discord.override { withVencord = true; withOpenASAR = true; })
    kitty
    alacritty
    bluetui
    blueman
    rofi
    kdePackages.dolphin
    kdePackages.ark
    mpv
    vlc
    swww
    waypaper
    waybar
    hypridle
    hyprlock
    hyprpolkitagent
    mako
    lxsession
    fastfetch
    xdg-desktop-portal-gtk
    xdg-desktop-portal-hyprland
    xdg-user-dirs
    jq
    grim
    slurp
    wl-clipboard
    pwvucontrol
    libarchive
    unrar
    deno
    btop
    fontconfig
    jdk21
    nfs-utils
    dig
    nodejs
    smartmontools
    android-tools
    tmate
    agenix
    colmena
    ajax-deploy
    osu-stable
    osu-gatari
    spotatui
    patreon-dl-gui
    fanbox-dl
  ];

  fonts.packages = with pkgs; [nerd-fonts.roboto-mono font-awesome];
}
