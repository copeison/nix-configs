let
  ssh_keys = import ../ssh_keys_personal.nix;
  T480s = "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIEmKDFKOOuos5aDsCElX22qwMvWg/Rxxg/CBad8LpX6r root@T480s";
  wsl-debian = "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIO+/e4IxjfnJCaXKczlndKqpfHIw0ry0a+ieZCf90wxR";
  r33-local = "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIP7Gua0Rx5Ay1IiXR6RFStsPwtH5KNOeML/XHhhFLwfK";
  shittyvps = "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIG0fCV7QcBNz5iLh2rMtlyGoAAr8nrYC8P68TkhZtTyq";
  bcdn-nix =  "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIEq35IHi/KPh9ykPUyAHhyTGr/oIR9+T4oyWOkGX/tZ6";
  bcdn-nix-2 = "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIDpr24zjWZweZGW4+AwQWoA+yuMatc76K5BVaGSb7Q8b";
  rwf93 = "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIGcWcrTasX029yZ9zxhlZSvnQ0cnF75kEIdZPijFkRO3";
  keys = ssh_keys ++ [
    wsl-debian
    T480s
    rwf93
  ];

in {
  "nix-netrc.age".publicKeys = keys;
  "zipline.age".publicKeys = keys ++ [ r33-local ];
  "yutsu-r33-ca.age".publicKeys = keys ++ [ r33-local ];
  "noreply-r33-ca.age".publicKeys = keys ++ [ r33-local ];
  "zukameku-r33-ca.age".publicKeys = keys ++ [ r33-local ];
  "wireguard.age".publicKeys = ssh_keys ++ [ r33-local ];
  "wireguard-ps3.age".publicKeys = ssh_keys;
  "wireguard-server.age".publicKeys = ssh_keys ++ [ shittyvps ];
  "nv-bcdn1-wings-token.age".publicKeys = keys ++ [ bcdn-nix ];
  "nv-bcdn2-wings-token.age".publicKeys = keys ++ [ bcdn-nix-2 ];
}