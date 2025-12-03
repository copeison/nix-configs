let
  nixGaming = builtins.getFlake "github:fufexan/nix-gaming";
  nixGamingPkgs = nixGaming.outputs.packages.x86_64-linux;
in {
  nixpkgs.overlays = [
    # Custom pkgs
    (self: super: {
      osu-base = self.callPackage ./osu {
        osu-mime = nixGamingPkgs.osu-mime;
        wine-discord-ipc-bridge = nixGamingPkgs.wine-discord-ipc-bridge;
        proton-osu-bin = nixGamingPkgs.proton-osu-bin;
      };
      osu-stable = self.osu-base;
      osu-gatari = self.osu-base.override {
        desktopName = "osu!gatari";
        pname = "osu-gatari";
        launchArgs = "-devserver gatari.pw";
      };
      patreon-dl-gui = self.callPackage ./patreon-dl-gui {};
    })
  ];
}