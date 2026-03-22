{ config, ... }:

{
  nix.settings = {
    experimental-features = [
      "nix-command"
      "flakes"
    ];
    substituters = [
      "https://cache.nixos.org"
      "https://nix-community.cachix.org"
      "https://nix-gaming.cachix.org"
      "https://hydra.lab004.dev"
    ];
    trusted-users = [
      "ethan"
      "@wheel"
    ];
    trusted-public-keys = [
      "cache.nixos.org-1:6NCHdD59X431o0gWypbMrAURkbJ16ZPMQFGspcDShjY="
      "nix-community.cachix.org-1:mB9FSh9qf2dCimDSUo8Zy7bkq5CX+/rkCWyvRCYg3Fs="
      "nix-gaming.cachix.org-1:nbjlureqMbRAxR1gJ/f3hxemL9svXaZF/Ees8vCUUs4="
      "hydra.lab004.dev:6+mPv9GwAFx/9J+mIL0I41pU8k4HX0KiGi1LUHJf7LY="
    ];
  };
}