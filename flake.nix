
{
  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    nixpkgs-latest.url = "github:NixOS/nixpkgs/265d204da2c6616afd5356c935779004b5625d7b";
    colmena = {
      url = "github:nix-community/colmena";
      inputs.nixpkgs.follows = "nixpkgs";
    };
    agenix.url = "github:ryantm/agenix";
    nixos-mailserver.url = "gitlab:simple-nixos-mailserver/nixos-mailserver";
    pterodactyl-wings-nix.url = "github:BadCoder-Network/pterodactyl-wings-nix";
    home-manager = {
      url = "github:nix-community/home-manager/master";
      inputs.nixpkgs.follows = "nixpkgs";
    };
    nix-gaming.url = "github:fufexan/nix-gaming";
    nix-flatpak.url = "github:gmodena/nix-flatpak/?ref=latest";
  };

  outputs = inputs@{
    self,
    nixpkgs,
    nixpkgs-latest,
    colmena,
    agenix,
    nixos-mailserver,
    pterodactyl-wings-nix,
    home-manager,
    nix-gaming,
    nix-flatpak,
    ...
  }:
  let
    system = "x86_64-linux";

    latestPkgs = import nixpkgs-latest {
      inherit system;
    };

    flakeOverlays = [
      (final: prev: {
        agenix = agenix.packages.${system}.agenix;
        nixGaming = nix-gaming.packages.${system};

        biolink = final.callPackage ./pkgs/BioLink {};
        patreon-dl-server = final.callPackage ./pkgs/patreon-dl-server {};

        sonarr = latestPkgs.sonarr;
        zipline = latestPkgs.zipline;
        jellyfin = latestPkgs.jellyfin;
        jellyfin-ffmpeg = latestPkgs.jellyfin-ffmpeg;

        rtorrent = prev.rtorrent.overrideAttrs (old: {
          version = "0.15.6";
          src = final.fetchFromGitHub {
            owner = "rakshasa";
            repo = "rtorrent";
            rev = "v0.15.6";
            hash = "sha256-B/5m1JXdUpczUMNN4cy5p6YurjmRFxMQHG3cQFSmZSs=";
          };
        });

        libtorrent-rakshasa =
          prev.libtorrent-rakshasa.overrideAttrs (old: {
            version = "0.15.6";
            src = final.fetchFromGitHub {
              owner = "rakshasa";
              repo = "libtorrent";
              rev = "v0.15.6";
              hash = "sha256-udEe9VyUzPXuCTrB3U3+XCbVWvfTT7xNvJJkLSQrRt4=";
            };
          });
      })
    ];

    specialArgs = {
      inherit inputs system;
    };

    pkgs = import nixpkgs {
      inherit system;
      overlays = flakeOverlays;
    };

    sharedDesktopModules = [
      agenix.nixosModules.age
      home-manager.nixosModules.home-manager
      nix-flatpak.nixosModules.nix-flatpak

      { nixpkgs.overlays = flakeOverlays; }

      ./modules/boot/boot.nix
      ./modules/networking/nfsmounts.nix
      ./modules/networking/hosts.nix
      ./modules/networking/defaults.nix

      ./modules/shared/locale.nix
      ./modules/shared/users.nix
      ./modules/shared/nix-settings.nix
      ./modules/shared/dconf.nix
      ./modules/shared/shell.nix
      ./modules/shared/services.nix
      ./modules/shared/kvm.nix
      ./modules/shared/git.nix

      ./pkgs/overlays.nix
    ];
  in {
    colmenaHive = colmena.lib.makeHive {
      meta = {
        nixpkgs = pkgs;
        specialArgs = specialArgs;
      };
      defaults = {
        deployment.targetUser = "root";
        imports = [ ./core.nix ];
      };
      r33 = {
        deployment.targetHost = "10.0.0.141";
        imports = [
          agenix.nixosModules.age
          ./systems/R33/configuration.nix
        ];
      };
      shitbox = {
        deployment.targetHost = "10.0.0.165";
        imports = [
          agenix.nixosModules.age
          ./systems/shitbox/configuration.nix
        ];
      };
      optiplex = {
        deployment.targetHost = "10.0.0.229";
        imports = [
          home-manager.nixosModules.home-manager
          ./systems/optiplex/configuration.nix
          ./pkgs/overlays.nix
        ];
      };
      shittyvps = {
        deployment.targetHost = "74.208.73.245";
        imports = [
          agenix.nixosModules.age
          ./systems/shittyvps/configuration.nix
        ];
      };
      bcdn-nix = {
        deployment.targetHost = "23.143.108.37";
        imports = [
          agenix.nixosModules.age
          pterodactyl-wings-nix.nixosModules.default
          ./systems/bcdn-nix/configuration.nix
        ];
      };
      bcdn-nix-2 = {
        deployment.targetHost = "23.143.108.23";
        imports = [
          agenix.nixosModules.age
          pterodactyl-wings-nix.nixosModules.default
          ./systems/bcdn-nix-2/configuration.nix
        ];
      };
    };
    nixosConfigurations = {
      T480s = nixpkgs.lib.nixosSystem {
        inherit system specialArgs;
        modules = sharedDesktopModules ++ [
          ./systems/T480s/configuration.nix
        ];
      };
      jik = nixpkgs.lib.nixosSystem {
        inherit system specialArgs;
        modules = sharedDesktopModules ++ [
          ./systems/desktop/configuration.nix
          ./modules/hardware/amdgpu.nix
        ];
      };
    };
  };
}