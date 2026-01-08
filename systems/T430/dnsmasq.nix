{ pkgs, ... }:

{
  services.dnsmasq = {
    enable = true;
    resolveLocalQueries = false;
    settings = {
      bind-interfaces = true;
      dhcp-range = [
        "192.168.100.2,192.168.100.254"
        "2001:db8:1::1000,2001:db8:1::2000,64,12h"
        "::,constructor:enp0s25,ra-stateless,12h"
      ];
      enable-ra = true;
      interface = [
        "enp0s25"
      ];
      no-resolv = true;
      server = [
        "2001:4860:4860::8888"
        "2606:4700:4700::1111"
        "1.1.1.1"
        "8.8.8.8"
      ];
    };
  };

  networking = {
    interfaces.enp0s25 = {
      ipv4.addresses = [{
        address = "192.168.100.1";
        prefixLength = 24;
      }];
      ipv6.addresses = [{
        address = "2001:db8:1::1";
        prefixLength = 64;
      }];
      useDHCP = false;
    };
    firewall = {
      allowedUDPPorts = [
        # DHCP
        67
        68
      ];
      checkReversePath = false;
      enable = true;
      extraCommands = ''
        ${pkgs.iptables}/bin/iptables -t nat -A POSTROUTING -o wlp3s0 -j MASQUERADE
        ${pkgs.iptables}/bin/ip6tables -t nat -A POSTROUTING -o wlp3s0 -j MASQUERADE
      '';
      trustedInterfaces = [ "enp0s25" ];
    };
  };
}