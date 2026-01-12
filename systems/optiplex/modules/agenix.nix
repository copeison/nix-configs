{ pkgs, ... }:

{
  environment.systemPackages = with pkgs; [
    agenix
  ];

  age.secrets = {
    wireguard = {
      file = ../../../secrets/wireguard.age;
      owner = "root";
      group = "root";
    };
  };
}