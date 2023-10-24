## Add OpenMeteo

## Change Controller Get

## Add Health Endpoint
```
 app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // Add management endpoints into pipeline like this
                endpoints.Map<HealthEndpoint>();
            });
```

## Helm Chart Sequence
### Health Endpoints
```
          livenessProbe:
            httpGet:
              path: /actuator/health/liveness
              port: http
          readinessProbe:
            httpGet:
              path: /actuator/health/readiness
              port: http
```
### .helmignore
```
deploy.sh
*.tgz
```
### Chart.yaml
```
appVersion: "latest"
```
### Values.yaml
```
image:
  repository: localhost:5001/weather-server
  name: weather-server
  pullPolicy: Always
  # Overrides the image tag whose default is the chart appVersion.
  tag: "latest"
```

```
service:
  enabled: true
  type: LoadBalancer
```

```
ingress:
  enabled: false
  annotations:
      kubernetes.io/ingress.class: nginx
      kubernetes.io/tls-acme: "true"
  hosts:
    - host: weather-server.local
      paths:
        - path: /
          pathType: Prefix
          backend:
            serviceName: weather-server-svc
            servicePort: 80
```

## Code
```
  Host.CreateDefaultBuilder(args)
                .AddStreamServices<WeatherProcessor>()
```
### Startup.cs
```
 services.AddHttpClient();

            // Add Rabbit template
            services.AddRabbitTemplate();
            services.AddHostedService<WeatherService>();
```