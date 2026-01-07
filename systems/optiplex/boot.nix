{
  boot.loader = {
    efi.canTouchEfiVariables = true;
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
        # USB2
        "xhci_pci"
        # USB1
        "ehci_pci"
        # SATA
        "ata_piix"
        # USB Human Input Device
        "usbhid"
        # USB Storage
        "usb_storage"
        # Storage Device
        "sd_mod"
        # Storage Reader (Disk Drive)
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