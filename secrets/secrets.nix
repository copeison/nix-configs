let
  ssh_keys = import ../ssh_keys_personal.nix;
  T430 = "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIKK+d1tHVMKqyed0hX3cctelxk7+vPknhuIC7pgm+UmQ";
  keys = ssh_keys ++ [
    T430
  ];

in {
  "nix-netrc.age".publicKeys = keys;
}