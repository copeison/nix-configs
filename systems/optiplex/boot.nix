{
  boot.loader = {
    efi.canTouchEfiVariables = false;
    grub = {
      enable = true;
      configurationLimit = 100;
      copyKernels = true;
      device = "nodev";
      efiSupport = true;
      efiInstallAsRemovable = false;
      memtest86.enable = true;
    };
    timeout = 10;
  };

  boot = {
    consoleLogLevel = 0;
    initrd = {
      availableKernelModules = [
        "xhci_pci"
        "ehci_pci"
        "ata_piix"
        "uas"
        "usbhid"
        "sd_mod"
        "sr_mod"
      ];
      kernelModules = [ ];
    };
    kernelModules = [
      "kvm-intel"
    ];
    kernelParams = [
      # Boot a shell on failure
      "boot.shell_on_fail"
      # Show systemd
      "rd.systemd.show_status=auto"
    ];
  };
}