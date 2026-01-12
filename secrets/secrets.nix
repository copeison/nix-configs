let
  ssh_keys = import ../ssh_keys_personal.nix;
  T430 = "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIKK+d1tHVMKqyed0hX3cctelxk7+vPknhuIC7pgm+UmQ";
  r33-local = "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAICnauTgmSibFaafYvDNr5pZF9daFLVrl7cfsxZA5D+sQ";
  keys = ssh_keys ++ [
    T430
  ];

in {
  "nix-netrc.age".publicKeys = keys;
  "wg0.age".publicKeys = ssh_keys ++ [ r33-local ];
}