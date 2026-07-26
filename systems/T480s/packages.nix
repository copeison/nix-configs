{ pkgs, ... }:

{
  nixpkgs.config.allowUnfree = true;

  programs.firefox.enable = true;

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
    alacritty
    mpv
    vlc
    lxsession
    fastfetch
    xdg-desktop-portal-gtk
    xdg-user-dirs
    jq
    grim
    slurp
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
    python315
    inetutils
    p4v
    #udiskie
    libreoffice-qt
    hunspell
    libdrm
    intel-media-driver
    intel-vaapi-driver
    libva-vdpau-driver
    libvdpau-va-gl
    dotnet-sdk_10
    kodi
    jellyfin-media-player
    agenix
    colmena
    kdePackages.dolphin
    kdePackages.ark
    #osu-stable
    #osu-gatari
    spotatui
    patreon-dl-gui
    fanbox-dl
  ];

  fonts.packages = with pkgs; [nerd-fonts.roboto-mono font-awesome];
}
