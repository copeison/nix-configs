{ pkgs, ... }:

{
  nixpkgs.config.allowUnfree = true;

  programs.firefox.enable = true;
  programs.hyprland.enable = true;
  programs.adb.enable = true;

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
    kitty
    alacritty
    bluetui
    blueman
    rofi
    discord
    kdePackages.dolphin
    kdePackages.ark
    mpv
    vlc
    cider-2
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
    vscode
    filezilla
    deno
    gedit
    btop
    fontconfig
    prismlauncher
    jdk21
    spotify
    nfs-utils
    dotnetCorePackages.sdk_9_0-bin
    qimgv
    agenix
    colmena
    ajax-deploy
    osu-stable
    osu-gatari
    patreon-dl-gui
  ];

  fonts.packages = with pkgs; [nerd-fonts.roboto-mono font-awesome];
}
