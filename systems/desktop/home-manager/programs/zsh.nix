{
  programs.zsh = {
    enable = true;
      oh-my-zsh = {
      enable = true;
      plugins = ["git" "zsh-autosuggestions" "zsh-syntax-highlighting" "zsh-autocomplete"];
      theme = "sorin";
      custom = "/home/ethan/.config/omz-custom";
    };
      shellAliases = {
      rebuild = "sudo nixos-rebuild switch";
      lss = "ls -lha";
    };
  };
}