
{ config, lib, pkgs, ... }:

let
  netnsName = "container";
  vethName = "veth9";
  hostIF = "eth0";

  vethHostIP4 = "192.168.100.1";
  vethNSIP4 = "192.168.100.2";
  vethHostIP6 = "fd00:100::1";
  vethNSIP6 = "fd00:100::2";

  vpsIP = "74.208.73.245";
  vpsIPv6 = "2607:f1c0:f01e:fa00::1";

  ip = "${pkgs.iproute2}/bin/ip";
  iptables = "${pkgs.iptables}/bin/iptables";
  ip6tables = "${pkgs.iptables}/bin/ip6tables";

  ns = "${ip} -n ${netnsName}";
  nsExec = "${ip} netns exec ${netnsName}";

  vethService = "veth@${netnsName}.service";

in {
  environment.systemPackages = [
    pkgs.wireguard-tools
  ];

  networking = {
    networkmanager.unmanaged = [
      "interface-name:wg0"
    ];

    firewall = {
      allowPing = true;
      allowedUDPPorts = [ 51820 ];
      checkReversePath = false;

      interfaces.${vethName} = {
        allowedTCPPortRanges = [
          { from = 0; to = 65535; }
        ];
        allowedUDPPortRanges = [
          { from = 0; to = 65535; }
        ];
      };
    };
  };

  boot.kernel.sysctl = {
    "net.ipv4.ip_forward" = 1;
    "net.ipv6.conf.all.forwarding" = 1;
  };

  systemd.services."veth@${netnsName}" = {
    description = "Persistent network namespace ${netnsName}";

    wantedBy = [ "multi-user.target" ];

    before = [
      "wireguard-wg0.service"
    ];

    # Critical: never tear down the namespace
    # automatically during nixos-rebuild switch.
    restartIfChanged = false;
    stopIfChanged = false;

    path = [
      pkgs.iproute2
      pkgs.iptables
      pkgs.coreutils
    ];

    serviceConfig = {
      Type = "oneshot";
      RemainAfterExit = true;
      TimeoutStartSec = "30s";
    };

    script = ''
      set -euo pipefail

      # Create namespace only if missing.
      if ! ${ip} netns list | grep -qE '^${netnsName}( |$)'; then
        ${ip} netns add ${netnsName}
      fi

      # Create veth pair only if missing.
      if ! ${ip} link show ${vethName} >/dev/null 2>&1; then
        # Clean up an orphaned namespace-side veth.
        if ${ns} link show veth1 >/dev/null 2>&1; then
          ${ns} link delete veth1
        fi

        ${ip} link add ${vethName} \
          type veth peer name veth1 \
          netns ${netnsName}
      fi

      # Configure host side.
      ${ip} addr replace ${vethHostIP4}/24 dev ${vethName}
      ${ip} -6 addr replace ${vethHostIP6}/64 dev ${vethName}
      ${ip} link set ${vethName} up

      # Configure namespace side.
      ${ns} addr replace ${vethNSIP4}/24 dev veth1
      ${ns} -6 addr replace ${vethNSIP6}/64 dev veth1

      ${ns} link set veth1 up
      ${ns} link set lo up

      # IPv6 default route.
      ${ns} -6 route replace default \
        via ${vethHostIP6} dev veth1

      # Explicit routes to the VPS through the host.
      ${ns} route replace ${vpsIP}/32 \
        via ${vethHostIP4} dev veth1

      ${ns} -6 route replace ${vpsIPv6}/128 \
        via ${vethHostIP6} dev veth1

      # Idempotent IPv4 NAT.
      ${iptables} -t nat -C POSTROUTING \
        -s ${vethNSIP4}/24 -o ${hostIF} \
        -j MASQUERADE 2>/dev/null || \
      ${iptables} -t nat -A POSTROUTING \
        -s ${vethNSIP4}/24 -o ${hostIF} \
        -j MASQUERADE

      # Idempotent IPv6 NAT.
      ${ip6tables} -t nat -C POSTROUTING \
        -s ${vethNSIP6}/64 -o ${hostIF} \
        -j MASQUERADE 2>/dev/null || \
      ${ip6tables} -t nat -A POSTROUTING \
        -s ${vethNSIP6}/64 -o ${hostIF} \
        -j MASQUERADE

      # Verify setup before reporting success.
      ${ip} link show ${vethName} >/dev/null
      ${ns} link show veth1 >/dev/null

      echo "Namespace ${netnsName} ready"
    '';

    # Deliberately no ExecStop.
    # The namespace survives service stops/rebuilds.
  };

  systemd.services.wireguard-wg0 = {
    requires = [ vethService ];
    after = [ vethService ];
  };

  networking.wireguard.interfaces.wg0 = {
    interfaceNamespace = netnsName;
    ips = [
      "fd00:127::2/128"
      "10.127.0.2/24"
    ];
    listenPort = 51820;
    mtu = 1420;
    privateKeyFile = config.age.secrets.wireguard.path;
    peers = [{
      allowedIPs = [ "0.0.0.0/0" "::/0" ];
      endpoint = "${vpsIP}:51820";
      persistentKeepalive = 25;
      publicKey = "1qLS2X6/Y6ldHHpG0fnvaD4qexzpNWdSKgZsXqfQ7gA=";
    }];
  };
}