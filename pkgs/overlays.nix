let
  nixGaming = builtins.getFlake "github:fufexan/nix-gaming";
  nixGamingPkgs = nixGaming.outputs.packages.x86_64-linux;
  agenixFlake = builtins.getFlake "github:ryantm/agenix";
  waybar-module-music = builtins.getFlake "github:Andeskjerf/waybar-module-music";
in {
  imports = [
    agenixFlake.nixosModules.age
  ];

  nixpkgs.overlays = [
    # Custom pkgs
    (self: super: {
      osu-base = self.callPackage ./osu {
        osu-mime = nixGamingPkgs.osu-mime;
        wine-discord-ipc-bridge = nixGamingPkgs.wine-discord-ipc-bridge;
        proton-osu-bin = nixGamingPkgs.proton-osu-bin;
      };
      agenix = agenixFlake.outputs.packages.x86_64-linux.agenix;
      waybar-module-music = waybar-module-music.outputs.packages.x86_64-linux.waybar-module-music;
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