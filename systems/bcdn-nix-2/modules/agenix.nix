{ pkgs, ... }:

{
  environment.systemPackages = with pkgs; [
    agenix
  ];

  age.secrets = {
    nv-bcdn2-wings-token.file = ../../../secrets/nv-bcdn2-wings-token.age;
  };
}