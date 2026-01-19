{
  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    agenix.url = "github:ryantm/agenix";
  };
  outputs = inputs@{ self, nixpkgs, agenix }:
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
      optionsplex = {
        deployment = {
          targetHost = "10.0.0.152";
          targetUser = "root";
        };
        imports = [
          agenix.nixosModules.age
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
    };
  };
}