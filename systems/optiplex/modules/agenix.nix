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

    zipline = {
      file = ../../../secrets/zipline.age;
      owner = "root";
      group = "root";
    };

    yutsu-r33-ca = {
      file = ../../../secrets/yutsu-r33-ca.age;
      owner = "nginx";
      group = "nginx";
    };
  };
}