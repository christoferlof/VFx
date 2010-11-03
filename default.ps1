properties {
  $basedir    = resolve-path .
  $builddir   = "$basedir\build"
  $toolsdir   = "$basedir\tools"
  $packagedir = "$toolsdir\Package"
  $xapdir     = "$toolsdir\Content\ClientBin"
  $slnfile    = "$basedir\VFx.sln"
  
}

task default -depends Test

task Test -depends Compile, Clean { 
  @(
    'TodoApp*.dll',
    'Victoria*.dll',
    'System.Xml.Linq.dll'
  ) | foreach { copy-item $builddir\$_ $packagedir }
  
  $env:path = "$toolsdir;$env:path"
  exec {Victoria.Test.Console.exe $testpath} "Testrun failed"
}

task Compile -depends Clean { 
  exec {msbuild "$slnfile" /p:Configuration=Debug  "/p:OutDir=$builddir\" /Verbosity:Quiet /nologo}
}

task Clean { 
  @($builddir, $packagedir, $xapdir) | foreach {
    get-childitem $_ -recurse | remove-item -recurse
  }
}

task ? -Description "Helper to display task info" {
	Write-Documentation
}