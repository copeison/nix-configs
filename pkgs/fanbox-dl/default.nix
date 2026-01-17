{ lib
, buildGoModule
, fetchFromGitHub
}:

buildGoModule rec {
  pname = "fanbox-dl";
  version = "0.28.1";
  rev = "v${version}";

  src = fetchFromGitHub {
    inherit rev;
    owner = "hareku";
    repo = "fanbox-dl";
    hash = "sha256-vXKiShP8RdIT8pRhDkO5K3fBVHQZ9nXv5GAhZaEXj8E=";
  };

  vendorHash = "sha256-uhNitrJeFuFG2XyQrc1JBbExoU6Ln6AFRO2Bgb1+N5M=";

  ldflags = [
    "-s"
    "-w"
    "-X github.com/hareku/fanbox-dl/cmd.Version=${version}"
    "-X github.com/hareku/fanbox-dl/cmd.Revision=${rev}"
    "-X github.com/hareku/fanbox-dl/cmd.Branch=unknown"
    "-X github.com/hareku/fanbox-dl/cmd.BuildUser=nix@nixpkgs"
    "-X github.com/hareku/fanbox-dl/cmd.BuildDate=unknown"
  ];

  meta = {
    description = "Pixiv Fanbox Downloader";
    mainProgram = "fanbox-dl";
    homepage = "https://github.com/hareku/fanbox-dl";
    license = lib.licenses.mit;
  };
}