let
  keys = (import ./ssh_keys_personal.nix) ++ (import ./ssh_keys.nix);
in {
  console.keyMap = "us";

  i18n.defaultLocale = "en_CA.UTF-8";

  time.timeZone = "America/Edmonton";

  users = {
    users = {
      root.openssh.authorizedKeys = { inherit keys; };
      server = {
        isNormalUser = true;
        group = "server";
        extraGroups = [ "wheel" ];
        openssh.authorizedKeys = { inherit keys; };
      };
      unison = {
        isNormalUser = true;
        group = "unison";
        extraGroups = [ "wheel" ];
        openssh.authorizedKeys = { inherit keys; };
      };
    };
    groups.server = {};
    groups.unison = {};
  };
  system.stateVersion = "26.05";
}