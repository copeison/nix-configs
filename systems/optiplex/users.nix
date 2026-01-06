{
  users.users = let
    keys = import ../../ssh_keys_personal.nix;
  in {
    root.openssh.authorizedKeys = { inherit keys; };
    server = {
      isNormalUser = true;
      extraGroups = [ "wheel" ];
      openssh.authorizedKeys = { inherit keys; };
    };
  };
}