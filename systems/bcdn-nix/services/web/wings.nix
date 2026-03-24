{
    services.wings = {
      enable = true;
      tokenFile = config.age.secrets.wings-token.path;
      user = "root";
      group = "root";
      package = wings;
      config = {
        uuid = "613b2324-8498-4334-92f8-ac3248f11739";
        token_id = "5agckclRNbt9sJXP";
        remote = "https://client.r33.ca";
        ignore_panel_config_updates = true;
        docker = {
          container_pid_limit = 0;
        };
        api = {
          host = "0.0.0.0";
          port = "8080";
          ssl = {
            enabled = false;
          };
        };
      };
  };
}