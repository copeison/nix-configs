{ config, pkgs, ... }:

{
  # Keep your existing configuration here.

  programs.nix-ld = {
    enable = true;

    libraries = with pkgs; [
      stdenv.cc.cc
      zlib
      openssl
      curl
      icu
      glib
      glibc
      gcc.cc.lib
      libgcc
      libstdcxx5

      # Native launcher GUI dependencies
      gtk3
      webkitgtk_4_1
      libsoup_3
      glib-networking
      libxkbcommon
    ];
  };
}