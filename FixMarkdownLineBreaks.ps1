
param(
    [string]$Path
)

function Process-MarkdownFile {
    param([string]$FilePath)


    Write-Host "Processing file: $FilePath"
    if (!(Test-Path $FilePath)) {
        Write-Error "File not found: $FilePath"
        return
    }

    # Read all lines from the file
    $lines = Get-Content -Path $FilePath -Encoding UTF8

    $modified = $false
    $modifications = @()

    for ($i = 0; $i -lt $lines.Count - 1; $i++) {
        $currentLine = $lines[$i]
        $nextLine = $lines[$i + 1]
        $currentLineIsHeader = $currentLine.TrimStart() -match '^(#+)\s'
        $currentLineIsCodeFence = $currentLine.Trim() -eq '```'
        $currentLineEndsWithPipe = $currentLine.TrimEnd().EndsWith('|')
        $nextLineTrimmed = $nextLine.Trim()
        $nextLineIsEmpty = $nextLineTrimmed -eq ""
        $nextLineIsHeader = $nextLineTrimmed -match '^(#+)\s'
        if ($currentLine -ne "" -and ($currentLine -notmatch "  $") -and -not $nextLineIsEmpty -and -not $currentLineIsHeader -and -not $currentLineIsCodeFence -and -not $currentLineEndsWithPipe -and -not $nextLineIsHeader) {
            # Only check for code fence if the immediate next line is not empty
            $nextNonEmptyLine = $nextLineTrimmed
            $j = $i + 1
            while ($nextNonEmptyLine -eq "" -and $j -lt $lines.Count) {
                $j++
                if ($j -lt $lines.Count) {
                    $nextNonEmptyLine = $lines[$j].Trim()
                }
            }
            $nextLineIsCodeFence = $nextNonEmptyLine -match '^```'
            if (-not $nextLineIsCodeFence) {
                if ($currentLine -match " $" -and $currentLine -notmatch "  $") {
                    $lines[$i] = "$currentLine "
                } elseif ($currentLine -notmatch " $") {
                    $lines[$i] = "$currentLine  "
                }
                $modifications += ($i + 1) # 1-based line number
                $modified = $true
            }
        }
    }

    if ($modified) {
        Set-Content -Path $FilePath -Value $lines -Encoding UTF8
        foreach ($lineNum in $modifications) {
            Write-Host "  Added two spaces at end of line $lineNum"
        }
    } 
}


# Main logic
if ($Path) {
    Process-MarkdownFile -FilePath $Path
} else {
    $sep = [regex]::Escape([System.IO.Path]::DirectorySeparatorChar)
    $mdFiles = Get-ChildItem -Path (Get-Location) -Recurse -Filter *.md -File |
        Where-Object {
            $dir = $_.DirectoryName
            $parts = $dir -split $sep
            -not ($parts | Where-Object { $_ -like ".*" })
        }
    foreach ($file in $mdFiles) {
        Process-MarkdownFile -FilePath $file.FullName
    }
}
