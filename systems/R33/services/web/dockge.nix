{ lib, ... }:

let
  stacksPath = "/data/Config/stacks";
  dockgePath = "/data/Config/dockge/data";
in {
  virtualisation = {
    docker.enable = true;
    podman.enable = true;
    oci-containers.containers = {
      dockge = {
        autoStart = true;
        image = "louislam/dockge:1";
        ports = [ "5001:5001" ];
        volumes = [
          "/var/run/docker.sock:/var/run/docker.sock"
          "${dockgePath}:/app/data:Z"
          "${stacksPath}:${stacksPath}"
        ];
        environment.DOCKGE_STACKS_DIR = "${stacksPath}";
      };
    };
  };
}