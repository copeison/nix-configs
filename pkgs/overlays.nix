 { inputs, ...}:
 {
  nixpkgs.overlays = [
    # Custom pkgs
    (self: super: {
      osu-base = self.callPackage ./osu {
        osu-mime = self.nixGaming.osu-mime;
        wine-discord-ipc-bridge = self.nixGaming.wine-discord-ipc-bridge;
        proton-osu-bin = self.nixGaming.proton-osu-bin;
      };
      osu-stable = self.osu-base;
      osu-gatari = self.osu-base.override {
        desktopName = "osu!gatari";
        pname = "osu-gatari";
        launchArgs = "-devserver gatari.pw";
      };
      patreon-dl-gui = self.callPackage ./patreon-dl-gui {};
      fanbox-dl = self.callPackage ./fanbox-dl {};
    })
  ];
}