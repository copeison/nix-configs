{
    services.nginx = {
    enable = true;
    enableReload = true;
    commonHttpConfig = ''
      # Proxy settings
      proxy_headers_hash_max_size 512;
      proxy_headers_hash_bucket_size 64;

      # Setup Real-IP
      real_ip_header CF-Connecting-IP;
      set_real_ip_from 173.245.48.0/20;
      set_real_ip_from 103.21.244.0/22;
      set_real_ip_from 103.22.200.0/22;
      set_real_ip_from 103.31.4.0/22;
      set_real_ip_from 141.101.64.0/18;
      set_real_ip_from 108.162.192.0/18;
      set_real_ip_from 190.93.240.0/20;
      set_real_ip_from 188.114.96.0/20;
      set_real_ip_from 197.234.240.0/22;
      set_real_ip_from 198.41.128.0/17;
      set_real_ip_from 162.158.0.0/15;
      set_real_ip_from 104.16.0.0/13;
      set_real_ip_from 104.24.0.0/14;
      set_real_ip_from 172.64.0.0/13;
      set_real_ip_from 131.0.72.0/22;
    '';

    recommendedBrotliSettings = true;
    recommendedGzipSettings = true;
    recommendedOptimisation = true;
    recommendedProxySettings = true;
    recommendedTlsSettings = true;
    experimentalZstdSettings = true;
  };

    services.nginx.virtualHosts."pawjob.online" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
      proxyPass = "http://10.0.0.152:6969";
      proxyWebsockets = true;
      extraConfig = ''
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $remote_addr;
        proxy_set_header CF-Connecting-IP $remote_addr;
      '';
    };
  };

  services.nginx.virtualHosts."geta.pawjob.online" = {
      enableACME = true;
      forceSSL = true;
      locations."/" = {
        proxyPass = "http://10.0.0.28";
        proxyWebsockets = true;
      };
    };

    services.nginx.virtualHosts."debrid.pawjob.online" = {
        enableACME = true;
        forceSSL = true;
        locations."/" = {
          proxyPass = "http://10.0.0.152:6500";
          proxyWebsockets = true;
        };
    };

    services.nginx.virtualHosts."iwanta.pawjob.online" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://10.0.0.152:8096";
        proxyWebsockets = true;
    };
  };

  services.nginx.virtualHosts."prowlarr.pawjob.online" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://10.0.0.152:9696";
        proxyWebsockets = true;
    };
  };

  services.nginx.virtualHosts."radarr.pawjob.online" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://10.0.0.152:7878";
        proxyWebsockets = true;
    };
  };

  services.nginx.virtualHosts."sonarr.pawjob.online" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://10.0.0.152:8989";
        proxyWebsockets = true;
    };
  };

  services.nginx.virtualHosts."dockge.pawjob.online" = {
    enableACME = true;
    forceSSL = true;
    locations."/" = {
        proxyPass = "http://10.0.0.152:5001";
        proxyWebsockets = true;
    };
  };
}