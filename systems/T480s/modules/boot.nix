{ config, ... }:

{
  boot.kernelModules = [ "uinput" ];
  boot.loader.grub.extraEntries = ''
  menuentry 'MacOS' $menuentry_id_option 'macOS-efi' {
	insmod chain
	insmod part_gpt
	insmod fat
	set root=(hd0,gpt1)
	chainloader /efi/OC/OpenCore.efi
	set root=(hd0,gpt1)/efi/OC
}
'';
}