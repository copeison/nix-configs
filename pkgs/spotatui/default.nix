{ pkgs ? import <nixpkgs> {} }:

pkgs.rustPlatform.buildRustPackage rec {
  pname = "spotatui";
  version = "0.34.2";

  src = pkgs.fetchFromGitHub {
    owner = "LargeModGames";
    repo = "spotatui";
    rev = "v${version}";
    hash = "sha256-bSmtXRf5lnh/sPZsNg8GGDVnFOfGIpBzp3ZaF+rkXpc=";
  };

 cargoLock = {
    lockFile = "${src}/Cargo.lock";
  };

  nativeBuildInputs = with pkgs; [
    pkg-config
    patchelf
    llvmPackages.clang
    llvmPackages.libclang
  ];

  buildInputs = with pkgs; [
    openssl
    alsa-lib
    dbus
    pipewire
  ];

  postFixup = ''
    patchelf \
      --set-rpath "${pkgs.lib.makeLibraryPath [
        pkgs.openssl
        pkgs.alsa-lib
        pkgs.dbus
        pkgs.pipewire
      ]}" \
      $out/bin/spotatui
  '';

  meta = with pkgs.lib; {
    description = "Terminal UI Spotify client";
    homepage = "https://github.com/LargeModGames/spotatui";
    license = licenses.mit;
    mainProgram = "spotatui";
    platforms = platforms.linux;
  };
}
