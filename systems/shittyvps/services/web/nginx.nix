{
  services.nginx = {
    enable = true;
    enableReload = true;

    recommendedTlsSettings = true;

    appendConfig = ''
      stream {
        map $ssl_preread_server_name $backend {
          status.pawjob.online 127.0.0.1:443;  # handled by HTTP block
          default          10.127.0.3:443; # catch-all
        }

        server {
          listen 74.208.73.245:443;
          proxy_pass $backend;
          ssl_preread on;
        }
      }
    '';
  };
}