# Local Nginx Configuration for MyFinance API

This directory contains the local nginx configuration for running the MyFinance API on localhost for development purposes.

## Files

- `nginx.local.conf` - Nginx configuration optimized for localhost development
- `run-nginx-local.ps1` - PowerShell script to easily start/stop nginx locally
- `NGINX-LOCAL-README.md` - This documentation file

## Prerequisites

1. **Nginx installed** on your Windows machine
   - Download from: https://nginx.org/en/download.html
   - Or install via Chocolatey: `choco install nginx`
   - Or install via winget: `winget install nginx`

2. **.NET API running** on port 5057
   - Make sure your MainService.API is running locally
   - The API should be accessible at `http://localhost:5057`

## Quick Start

### 1. Start your .NET API
First, make sure your .NET API is running:
```bash
# From the project root directory
dotnet run --project MainService.API
```

### 2. Start Nginx with local configuration
```powershell
# Test the configuration first
.\run-nginx-local.ps1 -Test

# Start nginx
.\run-nginx-local.ps1
```

### 3. Access your API
Once both services are running, you can access:

- **API Gateway**: http://localhost:8081
- **Main API**: http://localhost:8081/api/
- **API Documentation**: http://localhost:8081/swagger/
- **Health Check**: http://localhost:8081/health

## Configuration Details

### Key Differences from Production

The `nginx.local.conf` is simplified for local development:

1. **Removed production-specific settings**:
   - Cloudflare proxy headers
   - Rate limiting
   - Strict security headers
   - SSL/HTTPS forcing

2. **Added development-friendly features**:
   - CORS headers for local development
   - Simplified logging
   - Localhost-specific upstream configuration
   - Helpful root endpoint with available routes

3. **Port Configuration**:
   - Nginx listens on port 8081
   - Proxies to localhost:5057 (your .NET API)
   - No SSL/HTTPS (for simplicity)

### Upstream Configuration
```nginx
upstream main_service {
    server localhost:5057;  # Your .NET API port
}
```

### CORS Support
The configuration includes CORS headers for local development:
```nginx
add_header Access-Control-Allow-Origin "*" always;
add_header Access-Control-Allow-Methods "GET, POST, PUT, DELETE, OPTIONS" always;
add_header Access-Control-Allow-Headers "DNT,User-Agent,X-Requested-With,If-Modified-Since,Cache-Control,Content-Type,Range,Authorization" always;
```

## Script Usage

### PowerShell Script Options

```powershell
# Test configuration
.\run-nginx-local.ps1 -Test

# Start nginx (default)
.\run-nginx-local.ps1

# Start nginx on custom port (requires config modification)
.\run-nginx-local.ps1 -Port 8082

# Stop nginx
.\run-nginx-local.ps1 -Stop

# Use custom nginx path
.\run-nginx-local.ps1 -NginxPath "C:\nginx\nginx.exe"
```

### Manual Nginx Commands

If you prefer to run nginx manually:

```bash
# Test configuration
nginx -t -c nginx.local.conf

# Start nginx
nginx -c nginx.local.conf

# Stop nginx
nginx -s stop

# Reload configuration
nginx -s reload
```

## Troubleshooting

### Port 8081 Already in Use
If port 8081 is already in use, you have a few options:

1. **Stop the conflicting service**:
   ```powershell
   # Check what's using port 8081
   netstat -ano | findstr :8081
   
   # Stop the service (replace PID with actual process ID)
   taskkill /PID <PID> /F
   ```

2. **Use a different port**:
   - Edit `nginx.local.conf` and change `listen 8081;` to `listen 8082;`
   - Run: `.\run-nginx-local.ps1 -Port 8082`

### Nginx Not Found
If nginx is not in your PATH:

1. **Add nginx to PATH**, or
2. **Use the full path**:
   ```powershell
   .\run-nginx-local.ps1 -NginxPath "C:\path\to\nginx.exe"
   ```

### API Not Accessible
If you get 502 Bad Gateway errors:

1. **Check if your .NET API is running** on port 5057
2. **Verify the API is accessible** directly: http://localhost:5057
3. **Check nginx error logs** for more details

### Permission Issues
If you get permission errors:

1. **Run PowerShell as Administrator**
2. **Check Windows Defender Firewall** settings
3. **Ensure nginx has permission** to bind to the port

## Development Workflow

1. **Start your .NET API** (port 5057)
2. **Start nginx** with local config (port 8081)
3. **Access your API** through the gateway
4. **Make changes** to your API
5. **Restart API** as needed (nginx will continue proxying)
6. **Stop nginx** when done: `.\run-nginx-local.ps1 -Stop`

## Comparison with Production

| Feature | Local Config | Production Config |
|---------|-------------|-------------------|
| Port | 8081 | 80 (with SSL termination) |
| Upstream | localhost:5057 | main-service:8080 |
| CORS | Open (*) | Restricted |
| Rate Limiting | Disabled | Enabled |
| Security Headers | Basic | Comprehensive |
| Cloudflare | Disabled | Enabled |
| SSL/HTTPS | Disabled | Enabled |

## Next Steps

Once you're comfortable with the local setup, you can:

1. **Customize the configuration** for your specific needs
2. **Add more upstream services** for microservices development
3. **Configure SSL certificates** for HTTPS testing
4. **Set up load balancing** for multiple API instances

## Support

If you encounter issues:

1. Check the nginx error logs
2. Verify your .NET API is running correctly
3. Test the configuration with `.\run-nginx-local.ps1 -Test`
4. Check Windows Event Viewer for system-level errors 