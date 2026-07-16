{ config, ... }:

{
  networking.extraHosts = ''
    10.0.0.141 r33-local
    10.0.0.154 shitbox
    10.0.0.210 osuserver
    23.143.108.23 bcdn-nix-2
    23.143.108.37 bcdn-nix
    74.208.73.245 shittyvps
  '';
}