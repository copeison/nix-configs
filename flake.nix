{
  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    agenix.url = "github:ryantm/agenix";
    nixos-mailserver.url = "gitlab:simple-nixos-mailserver/nixos-mailserver";
    pterodactyl-wings-nix.url = "github:BadCoder-Network/pterodactyl-wings-nix";
  };
  outputs = inputs@{ self, nixpkgs, agenix, nixos-mailserver, pterodactyl-wings-nix }:
  let
    system = "x86_64-linux";

    flakeOverlays = [
      (self: super: {
        agenix = agenix.outputs.packages.x86_64-linux.agenix;
        biolink = self.callPackage pkgs/BioLink {};
      })
    ];

    pkgs = import nixpkgs {
      inherit system;
      overlays = flakeOverlays;
    };
  in {
    colmena = {
      meta.nixpkgs = pkgs;
      meta.specialArgs = { inherit inputs system; };
      optionsplex = {
        deployment = {
          targetHost = "10.0.0.152";
          targetUser = "root";
        };
        imports = [
          agenix.nixosModules.age
          nixos-mailserver.nixosModule
          systems/optiplex/configuration.nix
          ./core.nix
        ];
      };
      shitbox = {
        deployment = {
          targetHost = "10.0.0.172";
          targetUser = "root";
        };
        imports = [
          agenix.nixosModules.age
          systems/shitbox/configuration.nix
          ./core.nix
        ];
      };
      mediacenter = {
        deployment = {
          targetHost = "10.0.0.75";
          targetUser = "root";
        };
        imports = [
          agenix.nixosModules.age
          systems/mediacenter/configuration.nix
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