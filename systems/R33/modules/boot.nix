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
        "uhci_hcd"
        "ehci_pci"
        "ahci"
        "virtio_pci"
        "virtio_scsi"
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