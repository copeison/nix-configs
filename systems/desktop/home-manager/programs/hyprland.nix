{
  wayland.windowManager.hyprland = {
    enable = true;
    configType = "lua";

    settings = {
  #https://wiki.hypr.land/configuring/core/monitors/
      #monitor = {
      #  output = "eDP-1";
      #  mode = "1920x1080@60";
      #  position = "0x0";
      #  scale = 1;
      #};

      env = [
      ];

      config = {
        general = {
          gaps_in = 5;
          gaps_out = 10;
          border_size = 1;
          col = {
            active_border = {
              colors = [ "rgba(444444ee)" "rgba(444444ee)" ];
              angle = 45;
            };
            inactive_border = "rgba(595959aa)";
          };
          resize_on_border = false;
          allow_tearing = false;
          layout = "dwindle";
        };

        decoration = {
          rounding = 10;
          rounding_power = 2;
          active_opacity = 1.0;
          inactive_opacity = 1.0;

          shadow = {
            enabled = true;
            range = 4;
            render_power = 3;
            color = "rgba(1a1a1aee)";
          };

          blur = {
            enabled = true;
            size = 1;
            passes = 1;
            vibrancy = 0.1696;
          };
        };

        animations.enabled = true;

        dwindle.preserve_split = true;

        master.new_status = "master";

        misc = {
          force_default_wallpaper = 1;
          disable_hyprland_logo = false;
        };

        input = {
          kb_layout = "us";
          follow_mouse = 1;
          sensitivity = 0;
          touchpad.natural_scroll = true;
        };

        xwayland.force_zero_scaling = true;
      };

      curve = [
        { _args = [ "easeOutQuint" { type = "bezier"; points = [ [ 0.23 1 ] [ 0.32 1 ] ]; } ]; }
        { _args = [ "easeInOutCubic" { type = "bezier"; points = [ [ 0.65 0.05 ] [ 0.36 1 ] ]; } ]; }
        { _args = [ "linear" { type = "bezier"; points = [ [ 0 0 ] [ 1 1 ] ]; } ]; }
        { _args = [ "almostLinear" { type = "bezier"; points = [ [ 0.5 0.5 ] [ 0.75 1.0 ] ]; } ]; }
        { _args = [ "quick" { type = "bezier"; points = [ [ 0.15 0 ] [ 0.1 1 ] ]; } ]; }
      ];

      animation = [
        { leaf = "global"; enabled = true; speed = 10; bezier = "default"; }
        { leaf = "border"; enabled = true; speed = 5.39; bezier = "easeOutQuint"; }
        { leaf = "windows"; enabled = true; speed = 4.79; bezier = "easeOutQuint"; }
        { leaf = "windowsIn"; enabled = true; speed = 4.1; bezier = "easeOutQuint"; style = "popin 87%"; }
        { leaf = "windowsOut"; enabled = true; speed = 1.49; bezier = "linear"; style = "popin 87%"; }
        { leaf = "fadeIn"; enabled = true; speed = 1.73; bezier = "almostLinear"; }
        { leaf = "fadeOut"; enabled = true; speed = 1.46; bezier = "almostLinear"; }
        { leaf = "fade"; enabled = true; speed = 3.03; bezier = "quick"; }
        { leaf = "layers"; enabled = true; speed = 3.81; bezier = "easeOutQuint"; }
        { leaf = "layersIn"; enabled = true; speed = 4; bezier = "easeOutQuint"; style = "fade"; }
        { leaf = "layersOut"; enabled = true; speed = 1.5; bezier = "linear"; style = "fade"; }
        { leaf = "fadeLayersIn"; enabled = true; speed = 1.79; bezier = "almostLinear"; }
        { leaf = "fadeLayersOut"; enabled = true; speed = 1.39; bezier = "almostLinear"; }
        { leaf = "workspaces"; enabled = true; speed = 1.94; bezier = "almostLinear"; style = "fade"; }
        { leaf = "workspacesIn"; enabled = true; speed = 1.21; bezier = "almostLinear"; style = "fade"; }
        { leaf = "workspacesOut"; enabled = true; speed = 1.94; bezier = "almostLinear"; style = "fade"; }
      ];

      layer_rule = {
        name = "blur-waybar";
        match.namespace = "waybar";
        blur = true;
      };

      device = {
        name = "epic-mouse-v1";
        sensitivity = -0.5;
      };
    };

    extraConfig = ''
      local terminal = "alacritty"
      local fileManager = "dolphin"
      local menu = "rofi -show drun"
      local mainMod = "SUPER"

      hl.on("hyprland.start", function()
        hl.exec_cmd("waybar")
        hl.exec_cmd("awww-daemon")
        hl.exec_cmd("mako")
        hl.exec_cmd("udiskie -t")
        hl.exec_cmd("systemctl --user start hyprpolkitagent")
      end)

      hl.bind(mainMod .. " + Return", hl.dsp.exec_cmd(terminal))
      hl.bind(mainMod .. " + SHIFT + Q", hl.dsp.window.close())
      hl.bind(mainMod .. " + SHIFT + M", hl.dsp.exit())
      hl.bind(mainMod .. " + SHIFT + z", hl.dsp.exec_cmd(fileManager))
      hl.bind(mainMod .. " + SHIFT + V", hl.dsp.window.float({ action = "toggle" }))
      hl.bind(mainMod .. " + D", hl.dsp.exec_cmd(menu))
      hl.bind(mainMod .. " + J", hl.dsp.layout("togglesplit"))

      hl.bind(mainMod .. " + left", hl.dsp.focus({ direction = "left" }))
      hl.bind(mainMod .. " + right", hl.dsp.focus({ direction = "right" }))
      hl.bind(mainMod .. " + up", hl.dsp.focus({ direction = "up" }))
      hl.bind(mainMod .. " + down", hl.dsp.focus({ direction = "down" }))

      for i = 1, 10 do
        local key = i % 10
        hl.bind(mainMod .. " + " .. key, hl.dsp.focus({ workspace = i }))
        hl.bind(mainMod .. " + SHIFT + " .. key, hl.dsp.window.move({ workspace = i }))
      end

      hl.bind(mainMod .. " + mouse_down", hl.dsp.focus({ workspace = "e+1" }))
      hl.bind(mainMod .. " + mouse_up", hl.dsp.focus({ workspace = "e-1" }))

      hl.bind(mainMod .. " + mouse:272", hl.dsp.window.drag(), { mouse = true })
      hl.bind(mainMod .. " + mouse:273", hl.dsp.window.resize(), { mouse = true })

      hl.bind("XF86AudioRaiseVolume", hl.dsp.exec_cmd("wpctl set-volume -l 1 @DEFAULT_AUDIO_SINK@ 5%+"), { locked = true, repeating = true })
      hl.bind("XF86AudioLowerVolume", hl.dsp.exec_cmd("wpctl set-volume @DEFAULT_AUDIO_SINK@ 5%-"), { locked = true, repeating = true })
      hl.bind("XF86AudioMute", hl.dsp.exec_cmd("wpctl set-mute @DEFAULT_AUDIO_SINK@ toggle"), { locked = true, repeating = true })
      hl.bind("XF86AudioMicMute", hl.dsp.exec_cmd("wpctl set-mute @DEFAULT_AUDIO_SOURCE@ toggle"), { locked = true, repeating = true })
      hl.bind("XF86MonBrightnessUp", hl.dsp.exec_cmd("brightnessctl s 10%+"), { locked = true, repeating = true })
      hl.bind("XF86MonBrightnessDown", hl.dsp.exec_cmd("brightnessctl s 10%-"), { locked = true, repeating = true })

      hl.bind("XF86AudioNext", hl.dsp.exec_cmd("playerctl next"), { locked = true })
      hl.bind("XF86AudioPause", hl.dsp.exec_cmd("playerctl play-pause"), { locked = true })
      hl.bind("XF86AudioPlay", hl.dsp.exec_cmd("playerctl play-pause"), { locked = true })
      hl.bind("XF86AudioPrev", hl.dsp.exec_cmd("playerctl previous"), { locked = true })

      --hl.bind(mainMod .. " + P", hl.dsp.exec_cmd("hyprshot -m region"))
      hl.bind(mainMod .. " + P", hl.dsp.exec_cmd("~/scripts/Zipline.sh"))
      hl.bind(mainMod .. " + L", hl.dsp.exec_cmd("pactl set-sink-mute @DEFAULT_SINK@ 1 && hyprlock"))
    '';
  };
}