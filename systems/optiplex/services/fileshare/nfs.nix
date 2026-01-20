let
  statd-port = 4000;
  lockd-port = 4001;
  mountd-port = 4002;
in {
  networking.firewall.allowedTCPPorts = [
    111 # NFS Portmapper
    2049 # NFS Traffic
    statd-port
    lockd-port
    mountd-port
  ];

  services.nfs.server = {
    enable = true;
    statdPort = statd-port;
    lockdPort = lockd-port;
    mountdPort = mountd-port;
    exports = ''
      /data/Share 10.0.0.0/24(rw,async,no_subtree_check)
      /data/Media 10.0.0.0/24(rw,async,no_subtree_check)
      /data/downloads 10.0.0.0/24(rw,async,no_subtree_check)
    '';
  };
}