{
  programs.zsh = {
    enable = true;
      oh-my-zsh = {
      enable = true;
      plugins = ["git" "zsh-autosuggestions" "zsh-syntax-highlighting" "zsh-autocomplete"];
      theme = "sorin";
      custom = "/home/unison/.config/omz-custom";
    };
      shellAliases = {
      lss = "ls -lha";
    };
  };
}