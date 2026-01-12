{ pkgs, ... }:

{
  environment.systemPackages = with pkgs; [
    agenix
  ];

  age.secrets = {
    wg0 = {
      file = ../../../secrets/wg0.age;
      owner = "root";
      group = "root";
    };
  };
}