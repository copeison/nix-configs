{ config, ... }:

{
  #
  # Used for Git
  #
  # This allows flakes to pull in private repos, otherwise
  # some hacky stuff is needed.
  environment.etc."nix/netrc" = {
    user = "ethan";
    group = "root";
    source = config.age.secrets.nix-netrc.path;
  };
}