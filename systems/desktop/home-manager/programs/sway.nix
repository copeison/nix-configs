{ pkgs, ... }:
let
  mod = "Mod4";
in
{
  wayland.windowManager.sway = {
    enable = true;
    package = pkgs.swayfx;
    systemd.xdgAutostart = true;

    config = {
      modifier = mod;

      fonts = {
        names = [ "monospace" ];
        size = 8.0;
      };

      window = {
        border = 0;
        titlebar = false;
      };

      floating = {
        border = 0;
        titlebar = false;
      };

      focus.wrapping = "yes";

      gaps = {
        inner = 10;
        outer = 10;
        smartGaps = "inverse_outer";
      };

      colors.focused = {
        border = "#2C2C2C";
        background = "#2C2C2C";
        text = "#B5BFE2";
        indicator = "#2C2C2C";
        childBorder = "#2C2C2C";
      };

      output = {
        "DP-1" = {
          mode = "1920x1080@240Hz";
          position = "1920 0";
          scale = "1";
        };
        "DP-2" = {
          mode = "1920x1080@60Hz";
          position = "0 0";
          scale = "1";
        };
      };

      workspaceOutputAssign = [
        { workspace = "1"; output = "DP-1"; }
      ];

      bars = [ ];

      startup = [
        { command = "swaymsg focus output DP-1"; }
        #{ command = "swayidle -w before-sleep 'swaylock -f' lock 'swaylock -f'"; }
        { command = "waybar"; }
        { command = "mako"; }
        { command = "udiskie -t --appindicator"; }
        { command = "awww-daemon"; }
        { command = "waypaper --restore"; }
        { command = "${pkgs.autotiling}/bin/autotiling"; always = true; }
      ];

      keybindings = {
        "${mod}+Return" = "exec alacritty";
        "${mod}+Shift+q" = "kill";
        "${mod}+d" = "exec rofi -show drun";
        "${mod}+Shift+d" = ''exec "rofi -modi drun,run -show run"'';

        "${mod}+j" = "focus left";
        "${mod}+k" = "focus down";
        "${mod}+l" = "focus up";
        "${mod}+semicolon" = "focus right";

        "${mod}+Left" = "focus left";
        "${mod}+Down" = "focus down";
        "${mod}+Up" = "focus up";
        "${mod}+Right" = "focus right";

        "${mod}+Shift+j" = "move left";
        "${mod}+Shift+k" = "move down";
        "${mod}+Shift+l" = "move up";
        "${mod}+Shift+semicolon" = "move right";

        "${mod}+Shift+Left" = "move left";
        "${mod}+Shift+Down" = "move down";
        "${mod}+Shift+Up" = "move up";
        "${mod}+Shift+Right" = "move right";

        "${mod}+h" = "split h";
        "${mod}+v" = "split v";
        "${mod}+f" = "fullscreen toggle";

        "${mod}+s" = "layout stacking";
        "${mod}+w" = "layout tabbed";
        "${mod}+e" = "layout toggle split";

        "${mod}+Shift+space" = "floating toggle";
        "${mod}+space" = "focus mode_toggle";
        "${mod}+a" = "focus parent";

        "${mod}+1" = "workspace number 1";
        "${mod}+2" = "workspace number 2";
        "${mod}+3" = "workspace number 3";
        "${mod}+4" = "workspace number 4";
        "${mod}+5" = "workspace number 5";
        "${mod}+6" = "workspace number 6";
        "${mod}+7" = "workspace number 7";
        "${mod}+8" = "workspace number 8";
        "${mod}+9" = "workspace number 9";
        "${mod}+0" = "workspace number 10";

        "${mod}+Shift+1" = "move container to workspace number 1";
        "${mod}+Shift+2" = "move container to workspace number 2";
        "${mod}+Shift+3" = "move container to workspace number 3";
        "${mod}+Shift+4" = "move container to workspace number 4";
        "${mod}+Shift+5" = "move container to workspace number 5";
        "${mod}+Shift+6" = "move container to workspace number 6";
        "${mod}+Shift+7" = "move container to workspace number 7";
        "${mod}+Shift+8" = "move container to workspace number 8";
        "${mod}+Shift+9" = "move container to workspace number 9";
        "${mod}+Shift+0" = "move container to workspace number 10";

        "${mod}+Shift+c" = "reload";
        "${mod}+Shift+e" = "exec swaynag -t warning -m 'You pressed the exit shortcut. Do you really want to exit sway? This will end your Wayland session.' -B 'Yes, exit sway' 'swaymsg exit'";

        "${mod}+r" = "mode resize";

        "XF86AudioRaiseVolume" = "exec pactl set-sink-volume @DEFAULT_SINK@ +10%";
        "XF86AudioLowerVolume" = "exec pactl set-sink-volume @DEFAULT_SINK@ -10%";
        "XF86AudioMute" = "exec pactl set-sink-mute @DEFAULT_SINK@ toggle";
        "XF86AudioMicMute" = "exec pactl set-source-mute @DEFAULT_SOURCE@ toggle";

        "${mod}+Shift+z" = "exec dolphin";
        "${mod}+b" = "exec firefox";
      };

      modes.resize = {
        "j" = "resize shrink width 10 px or 10 ppt";
        "k" = "resize grow height 10 px or 10 ppt";
        "l" = "resize shrink height 10 px or 10 ppt";
        "semicolon" = "resize grow width 10 px or 10 ppt";

        "Left" = "resize shrink width 10 px or 10 ppt";
        "Down" = "resize grow height 10 px or 10 ppt";
        "Up" = "resize shrink height 10 px or 10 ppt";
        "Right" = "resize grow width 10 px or 10 ppt";

        "Return" = "mode default";
        "Escape" = "mode default";
        "${mod}+r" = "mode default";
      };
    };
  };

  services.polkit-gnome.enable = true;
}