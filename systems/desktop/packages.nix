{ pkgs, ... }:

{
  nixpkgs.config.allowUnfree = true;

  programs.firefox.enable = true;

  programs.steam = {
  enable = true;
  remotePlay.openFirewall = true;
  dedicatedServer.openFirewall = true;
  };

  environment.systemPackages = with pkgs; [
    wget
    curl
    nano
    git
    (discord.override { withVencord = true; withOpenASAR = true; })
    alacritty
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
    hunspell
    dotnet-sdk_10
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
