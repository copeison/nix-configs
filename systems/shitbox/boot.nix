{
  boot.loader = {
    efi.canTouchEfiVariables = false;
    grub = {
      enable = true;
      configurationLimit = 100;
      copyKernels = true;
      device = "nodev";
      efiSupport = true;
      efiInstallAsRemovable = true;
      memtest86.enable = true;
    };
    timeout = 10;
  };

  boot = {
    consoleLogLevel = 0;
    initrd = {
      availableKernelModules = [
        "ahci" # SATA
        "xhci_pci" # USB
        "usbhid"
        "sdhci_pci" # Storage device host controller PCI
        "sdhci_acpi" # Storage device module
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