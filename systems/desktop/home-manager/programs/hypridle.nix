{
  services.hypridle = {
    enable = true;
    systemdTarget = "hyprland-session.target";

    settings = {
      general = {
        lock_cmd = "pidof hyprlock || hyprlock";
        before_sleep_cmd = "loginctl lock-session";
        after_sleep_cmd = "hyprctl dispatch 'hl.dsp.dpms({action = \"on\"})'";
      };

      listener = [
        {
          timeout = 300;
          on-timeout = "loginctl lock-session";
        }
        {
          timeout = 420;
          on-timeout = "hyprctl dispatch 'hl.dsp.dpms({action = \"off\"})'";
          on-resume = "hyprctl dispatch 'hl.dsp.dpms({action = \"on\"})'";
        }
        {
          timeout = 1800;
          on-timeout = "systemctl suspend";
        }
      ];
    };
  };
}