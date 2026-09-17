{ config, ... }:

{
  networking.extraHosts = ''
    10.0.0.7 pihole
    10.0.0.141 r33-local
    10.0.0.153 r33-proxmox
    10.0.0.165 shitbox
    10.0.0.211 r33-debian
    23.143.108.23 bcdn-nix-2
    23.143.108.37 bcdn-nix
    74.208.73.245 shittyvps
  '';
}