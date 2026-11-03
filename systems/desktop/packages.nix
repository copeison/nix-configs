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
    dotnet-sdk_8
    dotnet-sdk_9
    dotnet-sdk_10
    python315
    inetutils
    hunspell
    agenix
    colmena
    kdePackages.dolphin
    kdePackages.ark
    piper
    osu-stable
    osu-gatari
    osu-lazer-bin
    spotatui
    patreon-dl-gui
    fanbox-dl
    usbutils
    #(llama-cpp.override { vulkanSupport = true; rocmSupport = true; })
  ];

  fonts.packages = with pkgs; [nerd-fonts.roboto-mono font-awesome];
}