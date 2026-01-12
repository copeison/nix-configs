let
  keys = (import ./ssh_keys_personal.nix);
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
    };
    groups.server = {};
  };
}