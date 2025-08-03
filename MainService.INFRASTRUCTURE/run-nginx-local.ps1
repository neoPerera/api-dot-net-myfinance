# PowerShell script to run nginx locally for MyFinance API development
# This script helps you start nginx with the local configuration

param(
    [string]$NginxPath = "nginx",
    [int]$Port = 8081,
    [switch]$Test,
    [switch]$Stop
)

$ConfigFile = "nginx.local.conf"
$PidFile = "nginx-local.pid"

Write-Host "MyFinance API - Local Nginx Setup" -ForegroundColor Green
Write-Host "=================================" -ForegroundColor Green

# Check if nginx is installed
try {
    $nginxVersion = & $NginxPath -v 2>&1
    Write-Host "Nginx found: $nginxVersion" -ForegroundColor Yellow
} catch {
    Write-Host "Error: Nginx not found. Please install nginx or update the NginxPath parameter." -ForegroundColor Red
    Write-Host "You can download nginx from: https://nginx.org/en/download.html" -ForegroundColor Yellow
    exit 1
}

# Check if config file exists
if (-not (Test-Path $ConfigFile)) {
    Write-Host "Error: Configuration file '$ConfigFile' not found!" -ForegroundColor Red
    exit 1
}

# Test configuration
if ($Test) {
    Write-Host "Testing nginx configuration..." -ForegroundColor Yellow
    & $NginxPath -t -c (Resolve-Path $ConfigFile)
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Configuration test passed!" -ForegroundColor Green
    } else {
        Write-Host "Configuration test failed!" -ForegroundColor Red
        exit 1
    }
    exit 0
}

# Stop nginx if requested
if ($Stop) {
    Write-Host "Stopping nginx..." -ForegroundColor Yellow
    if (Test-Path $PidFile) {
        $pid = Get-Content $PidFile
        try {
            Stop-Process -Id $pid -Force
            Remove-Item $PidFile -ErrorAction SilentlyContinue
            Write-Host "Nginx stopped successfully!" -ForegroundColor Green
        } catch {
            Write-Host "Error stopping nginx: $_" -ForegroundColor Red
        }
    } else {
        Write-Host "No nginx process found running." -ForegroundColor Yellow
    }
    exit 0
}

# Check if port is available
$portInUse = Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue
if ($portInUse) {
    Write-Host "Warning: Port $Port is already in use!" -ForegroundColor Yellow
    Write-Host "You may need to stop the existing service or use a different port." -ForegroundColor Yellow
    Write-Host "To use a different port, modify the nginx.local.conf file and run with -Port parameter." -ForegroundColor Yellow
}

# Start nginx
Write-Host "Starting nginx with local configuration..." -ForegroundColor Yellow
Write-Host "Config file: $ConfigFile" -ForegroundColor Cyan
Write-Host "Port: $Port" -ForegroundColor Cyan

try {
    # Create logs directory if it doesn't exist
    if (-not (Test-Path "logs")) {
        New-Item -ItemType Directory -Name "logs" -Force | Out-Null
        Write-Host "Created logs directory" -ForegroundColor Yellow
    }
    
    # Copy mime.types if it doesn't exist in current directory
    $mimeTypesPath = "mime.types"
    if (-not (Test-Path $mimeTypesPath)) {
        # Try to find mime.types in nginx installation
        $nginxDir = Split-Path -Parent (Get-Command $NginxPath).Source
        $sourceMimeTypes = Join-Path $nginxDir "conf\mime.types"
        if (Test-Path $sourceMimeTypes) {
            Copy-Item $sourceMimeTypes $mimeTypesPath
            Write-Host "Copied mime.types from nginx installation" -ForegroundColor Yellow
        } else {
            Write-Host "Warning: mime.types not found. Creating a basic one..." -ForegroundColor Yellow
            # Create a basic mime.types file
            @"
types {
    text/html                             html htm shtml;
    text/css                              css;
    text/xml                              xml;
    image/gif                             gif;
    image/jpeg                            jpeg jpg;
    application/javascript                js;
    application/atom+xml                  atom;
    application/rss+xml                   rss;
    text/mathml                           mml;
    text/plain                            txt;
    text/vnd.sun.j2me.app-descriptor      jad;
    text/vnd.wap.wml                      wml;
    text/x-component                      htc;
    image/png                             png;
    image/tiff                            tif tiff;
    image/vnd.wap.wbmp                    wbmp;
    image/x-icon                          ico;
    image/x-jng                           jng;
    image/x-ms-bmp                        bmp;
    image/svg+xml                         svg svgz;
    image/webp                            webp;
    application/font-woff                 woff;
    application/font-woff2                woff2;
    application/java-archive              jar war ear;
    application/json                      json;
    application/mac-binhex40              hqx;
    application/msword                    doc;
    application/pdf                       pdf;
    application/postscript                ps eps ai;
    application/rtf                       rtf;
    application/vnd.apple.mpegurl         m3u8;
    application/vnd.ms-excel              xls;
    application/vnd.ms-fontobject         eot;
    application/vnd.ms-powerpoint         ppt;
    application/vnd.wap.wmlc              wmlc;
    application/vnd.wap.xhtml+xml         xhtml;
    application/x-7z-compressed           7z;
    application/x-cocoa                   cco;
    application/x-java-archive-diff       jardiff;
    application/x-java-jnlp-file          jnlp;
    application/x-makeself                run;
    application/x-perl                    pl pm;
    application/x-pilot                   prc pdb;
    application/x-rar-compressed          rar;
    application/x-redhat-package-manager  rpm;
    application/x-sea                     sea;
    application/x-shockwave-flash         swf;
    application/x-stuffit                 sit;
    application/x-tcl                     tcl tk;
    application/x-x509-ca-cert            der pem crt;
    application/x-xpinstall               xpi;
    application/xhtml+xml                 xhtml;
    application/xspf+xml                  xspf;
    application/zip                       zip;
    application/octet-stream              bin exe dll;
    application/octet-stream              deb;
    application/octet-stream              dmg;
    application/octet-stream              iso img;
    application/octet-stream              msi msp msm;
    application/octet-stream              pkg;
    audio/midi                            mid midi kar;
    audio/mpeg                            mp3;
    audio/ogg                             ogg;
    audio/x-m4a                           m4a;
    audio/x-realaudio                     ra;
    video/3gpp                            3gpp 3gp;
    video/mp2t                            ts;
    video/mp4                             mp4;
    video/mpeg                            mpeg mpg;
    video/quicktime                       mov;
    video/webm                            webm;
    video/x-flv                           flv;
    video/x-m4v                           m4v;
    video/x-mng                           mng;
    video/x-ms-asf                        asx asf;
    video/x-ms-wmv                        wmv;
    video/x-msvideo                       avi;
}
"@ | Out-File -FilePath $mimeTypesPath -Encoding UTF8
        }
    }
    
    & $NginxPath -c (Resolve-Path $ConfigFile) -p (Get-Location)
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Nginx started successfully!" -ForegroundColor Green
        Write-Host "API Gateway is now running on: http://localhost:$Port" -ForegroundColor Green
        Write-Host "" -ForegroundColor White
        Write-Host "Available endpoints:" -ForegroundColor Cyan
        Write-Host "  - http://localhost:$Port/api/ - Main API" -ForegroundColor White
        Write-Host "  - http://localhost:$Port/swagger/ - API Documentation" -ForegroundColor White
        Write-Host "  - http://localhost:$Port/health - Health Check" -ForegroundColor White
        Write-Host "" -ForegroundColor White
        Write-Host "Make sure your .NET API is running on port 5057!" -ForegroundColor Yellow
        Write-Host "To stop nginx, run: .\run-nginx-local.ps1 -Stop" -ForegroundColor Yellow
    } else {
        Write-Host "Failed to start nginx!" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "Error starting nginx: $_" -ForegroundColor Red
    exit 1
} 