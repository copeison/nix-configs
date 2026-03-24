{ pkgs, ... }:

{
  environment.systemPackages = with pkgs; [
    agenix
  ];

  age.secrets = {
    nv-bcdn1-wings-token.file = ../../../secrets/nv-bcdn1-wings-token.age;
  };
}