{
    services.wings = {
    enable = true;
    tokenFile = config.age.secrets.wings-token.path;
    user = "root";
    group = "root";
      package = wings;
      config = {
        uuid = "";
      token_id = "";
      remote = "https://portal.r33.ca";
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