{
  boot.loader.systemd-boot.enable = true;
  boot.loader.efi.canTouchEfiVariables = true;

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
      systemd.enable = true;
    };
    kernelModules = [
      "kvm-intel"
    ];
    kernelParams = [
      # Enable high-poll rate USB Keyboard devices
      "usbhid.kbpoll=1"
      # Boot a shell on failure
      "boot.shell_on_fail"
      # Show systemd
      "rd.systemd.show_status=auto"
    ];
    tmp.useTmpfs = false;
  };
}