properties {
  $basedir    = resolve-path .
  $builddir   = "$basedir\build"
  $toolsdir   = "$basedir\tools"
  $packagedir = "$toolsdir\Package"
  $contentdir = "$toolsdir\Content"
  $xapdir     = "$contentdir\ClientBin"
  $slnfile    = "$basedir\VFx.sln"
  
}

task default -depends ?

task Test -depends Compile, Clean { 
  @(
    'TodoApp*.dll',
    'Victoria*.dll',
    'System.Xml.Linq.dll'
  ) | foreach { copy-item $builddir\$_ $packagedir }
  
  $env:path = "$toolsdir;$env:path"
  exec {Victoria.Test.Console.exe $testpath} "Testrun failed"
}

task Setup -depends Clean -description "Setups the VFx dev environment" {
  #create directories
  @($builddir, $toolsdir, $packagedir, $xapdir) | foreach {
    if((test-path -path $_) -ne $true){
      new-item $_ -itemtype directory
    }
  }
  
  #compile
  compile-solution
  
  #copy tools
  copy-item "$builddir\Victoria.Test.Console.exe" $toolsdir
  copy-item "$builddir\Content\RunnerPage.html" $contentdir
  copy-item "$builddir\Content\Silverlight.js" $contentdir
  
  write-host ""
  write-host "Setup complete. Run '.\psake.ps1 -task ""test""' to run all unit tests" -foregroundcolor yellow
}

task Compile -depends Clean { 
  compile-solution
}

task Clean { 
  @($builddir, $packagedir, $xapdir) | foreach {
    if((test-path -path $_) -eq $true) {
      get-childitem $_ -recurse | remove-item -recurse
    }
  }
}

task ? -Description "Helper to display task info" {
	Write-Documentation
}

function compile-solution {
  exec {msbuild "$slnfile" /p:Configuration=Debug  "/p:OutDir=$builddir\" /Verbosity:Quiet /nologo}
}