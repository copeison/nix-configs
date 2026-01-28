{
  services.hytale-servers = {
    enable = true;

    servers = {
      jake = {
        enable = true;
        listenAddress = "0.0.0.0";
        port = 5520;
        openFirewall = true;
        autoStart = true;
        patchline = "release";
        files = {
        };
      };
    };
  };
}