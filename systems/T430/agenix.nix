{config, ...}:

{
  age.identityPaths = [
    "/etc/ssh/ssh_host_rsa_key"
    "/etc/ssh/ssh_host_ed25519_key"
    "/home/ethan/.ssh/id_ed25519"
  ];

  age.secrets = {
    nix-netrc = {
      file = ../../secrets/nix-netrc.age;
      owner = "root";
      group = "root";
    };
  };
}