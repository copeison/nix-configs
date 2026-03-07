{ config, ... }:

{
  users.users.ethan = {
    isNormalUser = true;
    extraGroups = [ "wheel" ];
  };
}
