let
  keys = (import ./ssh_keys_personal.nix) ++ [
    "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQDJR3qDc8r2kbg6Q+A0dk7E6fC/wdlySBKb8X+8XgRGJg6huXaCTPZbAyvzt1IvxY69IdBymExjUie7YuFOLOKi5wisfw6d1yVjrhaoZWvXTz6eyF0ssAzM1BbqJsHU2dahQnNo7ThUguR365woBaw1UrZHEjlAiX16NxDVEyaXNImDjlQKBiAyDaa/pOCe1GUYwPgXHJMwF+6JbY+pGYAm6AvvsnjhLO0kyzwv1hSOd4qlzSobkDE9FQMbJD7uV+D1cXAv2ERdf/h9/L5dUcOEUscES+wg8ezLOhaBmq8TT9K3gmhMa47zNQU1WUAg39n+2+/Dwix0j7GNsNZdbp6B vali@nixos-amd"
  ];
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
  system.stateVersion = "26.05";
}