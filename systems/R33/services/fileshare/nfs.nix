let
  statdport = 4000;
  lockdport = 4001;
  mountdport = 4002;
in {
  networking.firewall = {
  allowedTCPPorts = [
    111
    2049
    statdport
    lockdport
    mountdport
  ];
  allowedUDPPorts = [
    111
    2049
    statdport
    lockdport
    mountdport
  ];
};

  services.nfs.server = {
    enable = true;
    statdPort = statdport;
    lockdPort = lockdport;
    mountdPort = mountdport;
    exports = ''
      /data/Share 10.0.0.0/24(rw,async,no_subtree_check)
      /data/Media 10.0.0.0/24(rw,async,no_subtree_check)
      /data/downloads 10.0.0.0/24(rw,async,no_subtree_check)
    '';
  };
}