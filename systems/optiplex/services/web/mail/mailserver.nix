{ config, ... }:

{
  mailserver = {
    domains = [
      "r33.ca"
    ];
    enable = true;
    enableImap = true;
    enableImapSsl = true;
    enableSubmission = true;
    enableSubmissionSsl = true;
    enablePop3Ssl = true;
    fqdn = "mail.r33.ca";
    # nix-shell -p mkpasswd --run 'mkpasswd -sm bcrypt'
    loginAccounts = {
      "yutsu@r33.ca" = {
        hashedPasswordFile = config.age.secrets.yutsu-r33-ca.path;
        aliases = [
          "abuse@r33.ca"
          "admin@r33.ca"
          "postmaster@r33.ca"
        ];
      };
    };
    mailDirectory = "/var/vmail";
    openFirewall = true;
    stateVersion = 3;
    systemDomain = "r33.ca";
    systemName = "nixos-mailserver";
    x509.useACMEHost = config.mailserver.fqdn;
  };
}