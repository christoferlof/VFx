$testpath = ""
if($args[0] -ne $null) {
  $testpath = $args[0]
}
.\psake.ps1 -framework WP7 -task Test -parameters @{"testpath"=$testpath}
