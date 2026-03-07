{ config, ... }:

{
  networking.extraHosts = ''
    10.0.0.152 r33-local
    10.0.0.172 shitbox
    23.143.108.23 r33-bcdn
    74.208.73.245 shittyvps
  '';
}