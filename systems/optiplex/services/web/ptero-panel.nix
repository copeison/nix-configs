{
  config,
  pkgs,
  lib,
  ...
}: let
  app = "php-fpm-ptero";
  panel-root = "/data/Services/Pterodactyl";
  php-env = pkgs.php.buildEnv {
    extensions = {
      enabled,
      all,
    }:
      with all; enabled ++ [memcached redis mbstring bcmath mysqli curl zip gd];
    extraConfig = ''
      memory_limit = 256M
    '';
  };
in {
  services.mysql = {
    enable = true;
    package = pkgs.mariadb;
    settings.mysqld.bind-address = "0.0.0.0";
  };

  services.redis.servers."pterodactyl" = {
    enable = true;
    port = 3380;
  };

  services.phpfpm.pools.${app} = {
    user = "nginx";
    group = "nginx";
    settings = {
      "listen.owner" = config.services.nginx.user;
      "pm" = "dynamic";
      "pm.max_children" = 5;
      "pm.start_servers" = 2;
      "pm.min_spare_servers" = 1;
      "pm.max_spare_servers" = 3;
      "pm.max_requests" = 500;
    };
    phpEnv."PATH" = lib.makeBinPath [php-env];
    phpPackage = php-env;
  };

  systemd.services.pterodactyl-panel-scheduler = {
    description = "lagravel";

    after = ["redis-pterodactyl.service"];
    wantedBy = ["multi-user.target"];

    unitConfig = {
      ConditionPathExists = "${panel-root}/.env";
      ConditionDirectoryNotEmpty = "${panel-root}/vendor";

      StartLimitInterval = "180";
    };

    serviceConfig = {
      User = "nginx";
      Group = "nginx";
      SyslogIdentifier = "pterodactyl-panel-scheduler";
      WorkingDirectory = panel-root;
      ExecStart = "${config.services.phpfpm.phpPackage}/bin/php artisan queue:work --queue=high,standard,low --sleep=3 --tries=3";

      StartLimitBurst = "30";
      RestartSec = "5s";
      Restart = "always";
    };
  };

  services.nginx.virtualHosts."client.r33.ca" = {
    enableACME = true;
    forceSSL = true;

    root = "${panel-root}/public";

    extraConfig = ''
      index index.php;

      client_max_body_size 100m;
      client_body_timeout 120s;

      sendfile off;

      ssl_session_cache shared:SSL:10m;
      ssl_protocols TLSv1.2 TLSv1.3;
      ssl_ciphers "ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES128-GCM-SHA256:ECDHE-ECDSA-AES256-GCM-SHA384:ECDHE-RSA-AES256-GCM-SHA384:ECDHE-ECDSA-CHACHA20-POLY1305:ECDHE-RSA-CHACHA20-POLY1305:DHE-RSA-AES128-GCM-SHA256:DHE-RSA-AES256-GCM-SHA384";
      ssl_prefer_server_ciphers on;

      add_header X-Content-Type-Options nosniff;
      add_header X-XSS-Protection "1; mode=block";
      add_header X-Robots-Tag none;
      add_header Content-Security-Policy "frame-ancestors 'self'";
      add_header X-Frame-Options DENY;
      add_header Referrer-Policy same-origin;
    '';

    locations."/".extraConfig = ''
      try_files $uri $uri/ /index.php?$query_string;
    '';

    locations."~ \\.php$".extraConfig = ''
      fastcgi_split_path_info ^(.+\.php)(/.+)$;
      fastcgi_pass unix:${config.services.phpfpm.pools.php-fpm-ptero.socket};
      fastcgi_index index.php;
      fastcgi_param PHP_VALUE "upload_max_filesize = 100M \n post_max_size=100M";
      fastcgi_param SCRIPT_FILENAME $document_root$fastcgi_script_name;
      fastcgi_param HTTP_PROXY "";
      fastcgi_intercept_errors off;
      fastcgi_buffer_size 16k;
      fastcgi_buffers 4 16k;
      fastcgi_connect_timeout 300;
      fastcgi_send_timeout 300;
      fastcgi_read_timeout 300;
      include ${pkgs.nginx}/conf/fastcgi_params;
      include ${pkgs.nginx}/conf/fastcgi.conf;
    '';

    locations."~ /\\.ht".extraConfig = ''
      deny all;
    '';
  };
}