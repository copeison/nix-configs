{
  services = {
    samba = {
      enable = true;
      openFirewall = true;
      settings = {
        global = {
          "follow symlinks" = "yes";
          "guest account" = "nobody";
          "inherit permissions" = "yes";
          "log level" = "3";
          "map to guest" = "bad user";
          "netbios name" = "r33-local";
          "security" = "user";
          "server role" = "standalone server";
          "server signing" = "disabled";
          "server string" = "r33-local";
          "workgroup" = "WORKGROUP";
        };
        data = {
          browseable = "yes";
          "create mask" = "0644";
          "create mode" = "0660";
          "directory mask" = "0770";
          "directory mode" = "0775";
          "force create mode" = "0660";
          "force directory mode" = "0775";
          "guest ok" = "no";
          locking = "no";
          path = "/data/Share";
          "read only" = "no";
          "valid users" = "@users";
          writeable = "yes";
        };
      };
    };
    samba-wsdd = {
      enable = true;
      openFirewall = true;
    };
  };
}