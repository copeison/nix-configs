{pkgs, ... }:
{
  services.flatpak.enable = true;

  services.flatpak.remotes = [{
  name = "flathub"; location = "https://dl.flathub.org/repo/flathub.flatpakrepo";
  }];

  services.flatpak.packages = [
  "app.zen_browser.zen"
  "com.github.tchx84.Flatseal"
  ];
}