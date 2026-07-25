{
  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    nixpkgs-latest.url = "github:NixOS/nixpkgs/265d204da2c6616afd5356c935779004b5625d7b";
    agenix.url = "github:ryantm/agenix";
    nixos-mailserver.url = "gitlab:simple-nixos-mailserver/nixos-mailserver";
    pterodactyl-wings-nix.url = "github:BadCoder-Network/pterodactyl-wings-nix";
  };
  outputs = inputs@{ self, nixpkgs, nixpkgs-latest, agenix, nixos-mailserver, pterodactyl-wings-nix }:
  let
    system = "x86_64-linux";

    flakeOverlays = [
      (self: super: {
        agenix = agenix.outputs.packages.x86_64-linux.agenix;
        biolink = self.callPackage pkgs/BioLink {};
        patreon-dl-server = self.callPackage pkgs/patreon-dl-server {};
        sonarr = latestPkgs.sonarr;
        zipline = latestPkgs.zipline;
        jellyfin = latestPkgs.jellyfin;
        jellyfin-ffmpeg = latestPkgs.jellyfin-ffmpeg;
      })
    ];

    latestPkgs = import nixpkgs-latest { inherit system; };

    pkgs = import nixpkgs {
      inherit system;
      overlays = flakeOverlays;
    };
  in {
    colmena = {
      meta.nixpkgs = pkgs;
      meta.specialArgs = { inherit inputs system; };
      r33 = {
        deployment = {
          targetHost = "10.0.0.141";
          targetUser = "root";
        };
        imports = [
          agenix.nixosModules.age
          systems/R33/configuration.nix
          ./core.nix
        ];
      };
      shitbox = {
        deployment = {
          targetHost = "10.0.0.165";
          targetUser = "root";
        };
        imports = [
          agenix.nixosModules.age
          systems/shitbox/configuration.nix
          ./core.nix
        ];
      };
      optiplex = {
        deployment = {
          targetHost = "10.0.0.229";
          targetUser = "root";
        };
        imports = [
          agenix.nixosModules.age
          systems/optiplex/configuration.nix
          ./core.nix
        ];
      };
      shittyvps = {
        deployment = {
          targetHost = "74.208.73.245";
          targetUser = "root";
        };
        imports = [
          agenix.nixosModules.age
          systems/shittyvps/configuration.nix
          ./core.nix
        ];
      };
      bcdn-nix = {
        deployment = {
          targetHost = "23.143.108.37";
          targetUser = "root";
        };
        imports = [
          agenix.nixosModules.age
          pterodactyl-wings-nix.nixosModules.default
          systems/bcdn-nix/configuration.nix
          ./core.nix
        ];
      };
      bcdn-nix-2 = {
        deployment = {
          targetHost = "23.143.108.23";
          targetUser = "root";
        };
        imports = [
          agenix.nixosModules.age
          pterodactyl-wings-nix.nixosModules.default
          systems/bcdn-nix-2/configuration.nix
          ./core.nix
        ];
      };
    };
  };
}