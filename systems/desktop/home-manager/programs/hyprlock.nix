{
  programs.hyprlock = {
    enable = true;

    settings = {
      "$font" = "Monospace";

      general.hide_cursor = false;

      animations = {
        enabled = true;
        bezier = "linear, 1, 1, 0, 0";
        animation = [
          "fadeIn, 1, 5, linear"
          "fadeOut, 1, 5, linear"
          "inputFieldDots, 1, 2, linear"
        ];
      };

      background = {
        monitor = "";
        path = "screenshot";
        blur_passes = 2;
      };

      input-field = {
        monitor = "";
        size = "20%, 5%";
        outline_thickness = 3;
        inner_color = "rgba(0, 0, 0, 0.0)";

        outer_color = "rgba(8caaeeee) rgba(8caaeeee) 45deg";
        check_color = "rgba(f2d5cdee) rgba(f2d5cdee) 120deg";
        fail_color = "rgba(cc241dee) rgba(cc241dee) 40deg";

        font_color = "rgb(143, 143, 143)";
        fade_on_empty = false;
        rounding = 15;

        font_family = "$font";
        placeholder_text = "Input password...";
        fail_text = "$PAMFAIL";

        dots_spacing = 0.3;

        position = "0, -20";
        halign = "center";
        valign = "center";
      };
    };
  };
}