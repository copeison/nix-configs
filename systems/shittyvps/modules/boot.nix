{
  boot.loader = {
    grub = {
      enable = true;
      configurationLimit = 3;
      copyKernels = true;
      device = "/dev/vda";
      efiSupport = false;
    };
    timeout = 10;
  };

  boot.kernel.sysctl = {
    "net.ipv4.ip_forward" = true;
    "net.ipv4.tcp_syncookies" = true;
    "net.ipv6.conf.all.forwarding" = true;
    "net.netfilter.nf_conntrack_max" = 25594;
    "kernel.hung_task_panic" = 1;
    "kernel.panic" = 30;
  };

  boot.blacklistedKernelModules = [
    "virtio_gpu"
    "qxl"
  ];

  boot = {
    consoleLogLevel = 0;
    initrd = {
      availableKernelModules = [
        "ahci" # SATA
        "ata_piix"
        "virtio_blk"
        "virtio_net"
        "virtio_pci"
        "virtio_pci_legacy_dev"
        "virtio_pci_modern_dev"
        "virtio_scsi"
        "uhci_hcd"
        "sd_mod"
        "sr_mod"
      ];
      kernelModules = [ ];
    };
    kernelParams = [
      # Boot a shell on failure
      "boot.shell_on_fail"
      # Show systemd
      "rd.systemd.show_status=auto"
      # Enable console on tty0
      "console=tty0"
    ];
  };
}