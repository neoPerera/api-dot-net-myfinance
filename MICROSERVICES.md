# MyFinance Microservices Architecture

## Overview
This project has been restructured to support a microservices architecture with the current API serving as the **MainService**.

## Current Services

### 1. MainService (Current API)
- **Container**: `myfinance-main-service`
- **Port**: 5057 (internal: 8080)
- **Purpose**: Core financial operations (accounts, transactions, expenses, income)
- **Endpoints**: 
  - `/api/main/` - Main service routes
  - `/api/` - Legacy routes (backward compatibility)
  - `/health` - Health checks
  - `/swagger/` - API documentation

### 2. API Gateway (Nginx)
- **Container**: `myfinance-api-gateway`
- **Port**: 5000
- **Purpose**: Reverse proxy, routing, rate limiting, security headers
- **Features**: Cloudflare integration, CORS handling, load balancing

## Future Microservices (Planned)

### 3. UserService
- **Purpose**: User management, authentication, profiles
- **Endpoints**: `/api/users/`
- **Database**: Separate user database
- **Features**: User registration, profile management, preferences

### 4. NotificationService
- **Purpose**: Email, SMS, push notifications
- **Endpoints**: `/api/notifications/`
- **Features**: Email templates, SMS integration, push notifications

### 5. AnalyticsService
- **Purpose**: Financial analytics, reporting, insights
- **Endpoints**: `/api/analytics/`
- **Features**: Charts, reports, data aggregation

### 6. AuditService
- **Purpose**: Audit logging, compliance
- **Endpoints**: `/api/audit/`
- **Features**: Activity logging, compliance reports

## Architecture Benefits

### ✅ **Scalability**
- Each service can scale independently
- Load balancing across services
- Resource optimization

### ✅ **Maintainability**
- Isolated codebases
- Independent deployments
- Technology flexibility

### ✅ **Reliability**
- Service isolation
- Fault tolerance
- Health monitoring

### ✅ **Development**
- Team autonomy
- Parallel development
- Technology diversity

## Service Communication

### Current: Direct HTTP Calls
```
Frontend → API Gateway → MainService
```

### Future: Event-Driven Architecture
```
Frontend → API Gateway → Services
Services ↔ Message Queue (RabbitMQ)
Services ↔ Service Discovery (Consul)
```

## Deployment Strategy

### Development
```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f main-service
docker-compose logs -f api-gateway
```

### Production
```bash
# Deploy with specific image tag
IMAGE_TAG=v1.0.0 docker-compose up -d

# Health checks
curl http://localhost:5000/health
curl http://localhost:5000/api/main/
```

## Adding New Microservices

### 1. Create Service
```bash
# Create new service project
dotnet new webapi -n UserService
```

### 2. Update Docker Compose
```yaml
user-service:
  container_name: myfinance-user-service
  image: chanuth/myfinance-user-service:${IMAGE_TAG}
  ports:
    - "5058:8080"
  environment:
    - SERVICE_NAME=UserService
    - SERVICE_VERSION=1.0.0
  networks:
    - myfinance-network
```

### 3. Update Nginx Configuration
```nginx
upstream user_service {
    server user-service:8080;
}

location /api/users/ {
    proxy_pass http://user_service/;
    # ... proxy settings
}
```

### 4. Update CORS in Program.cs
```csharp
policy.WithOrigins("https://stage-myfinance.onrender.com", 
                   "https://myfinance.chanuthperera.com", 
                   "http://localhost:3000")
```

## Monitoring & Observability

### Health Checks
- **Liveness**: `/health/live` - Is service running?
- **Readiness**: `/health/ready` - Is service ready to serve?
- **Health**: `/health` - Detailed health information

### Logging
- Structured logging with service identification
- Centralized log aggregation (future)
- Request tracing across services

### Metrics
- Service performance metrics
- Database connection health
- Response times and error rates

## Security Considerations

### API Gateway Security
- Rate limiting per service
- CORS configuration
- Security headers
- Cloudflare integration

### Service-to-Service Security
- JWT token validation
- Service mesh (future consideration)
- Network isolation

### Data Security
- Database per service
- Encrypted communication
- Audit logging

## Migration Strategy

### Phase 1: Current (MainService)
- ✅ API Gateway setup
- ✅ Health monitoring
- ✅ Cloudflare integration

### Phase 2: UserService
- User management extraction
- Authentication service
- Profile management

### Phase 3: NotificationService
- Email/SMS integration
- Notification templates
- Delivery tracking

### Phase 4: AnalyticsService
- Financial analytics
- Reporting engine
- Data aggregation

### Phase 5: Event-Driven Architecture
- Message queue integration
- Service discovery
- Event sourcing

## Best Practices

### Service Design
- Single responsibility principle
- Bounded contexts
- API versioning
- Backward compatibility

### Data Management
- Database per service
- Eventual consistency
- Data synchronization
- Backup strategies

### Deployment
- Blue-green deployments
- Rolling updates
- Health checks
- Rollback procedures

### Monitoring
- Centralized logging
- Distributed tracing
- Performance metrics
- Alert systems 