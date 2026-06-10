$files = Get-ChildItem -Path . -Filter *.mp4 -File

Write-Output $files
$confirmation = Read-Host "Do you want to process these files? (Y/N)"
if ($confirmation -ne 'Y') {
  Write-Host "Operation cancelled by user."
  exit
}

foreach ($file in $files) {
  Write-Host "Processing file: $($file.FullName)"
  ffmpeg -i $file.FullName -c copy -movflags +faststart "$($file)_faststart.mp4"
  Write-Host "Finished processing: $($file.FullName). Now replacing original file."
  Remove-Item -Path $file.FullName
  Rename-Item -Path "$($file)_faststart.mp4" -NewName $file.Name
  Write-Host "Replaced original file with faststart version: $($file.FullName)"
}