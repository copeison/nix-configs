{ pkgs, ... }:

{
  environment.systemPackages = with pkgs; [
    agenix
  ];

  age.secrets = {
  };
}