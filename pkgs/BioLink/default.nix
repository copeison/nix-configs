{ lib
, buildNpmPackage
, fetchFromGitHub }:

buildNpmPackage {
  dontNpmBuild = true;
  name = "biolink";
  npmDepsHash = "sha256-fgGpIZVGVL7rKPWDRZvSiHCdKC9URyv7utiQoYjbmwg=";
  src = ./.;
}
