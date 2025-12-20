{ pkgs ? import <nixpkgs> {} }:

  pkgs.rustPlatform.buildRustPackage rec {
    pname = "spotatui";
    version = "0.34.5";

  src = pkgs.lib.cleanSource ./.;

  cargoLock = {
    lockFile = ./Cargo.lock;
  };

  nativeBuildInputs = with pkgs; [
    pkg-config
    llvmPackages.clang
    llvmPackages.libclang
  ];

  buildInputs = with pkgs; [
    openssl
    alsa-lib
    dbus
    pipewire
  ];

  meta = with pkgs.lib; {
    description = "Terminal UI Spotify client";
    homepage = "https://github.com/LargeModGames/spotatui";
    license = licenses.mit;
    mainProgram = "spotatui";
    platforms = platforms.linux;
  };
}
