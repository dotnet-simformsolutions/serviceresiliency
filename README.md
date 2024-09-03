# Service Resiliency in Microservice
  Service resiliency in microservices involves designing systems that can withstand and recover from various failures, ensuring continuous operation without significant impact on the user experience.
By implementing resiliency patterns, monitoring mechanisms, and self-healing capabilities, microservices can gracefully handle issues and maintain high availability. This approach not only enhances user satisfaction but also improves overall system stability and robustness.

# Features
* Fault Isolation
* Retry Mechanisms
* Circuit Breaker Pattern
* Fallback Strategies
* Load Balancing
* Health Monitoring
* Auto-Scaling
* Redundancy and Replication
* Self-Healing
* Graceful Degradation
* Chaos Engineering
* Data Consistency and Synchronization
* Distributed Tracing
* Throttling and Rate Limiting
* Service Mesh

# Prerequisites
- [.NET Core SDK](https://dotnet.microsoft.com/download) (version 8.0)
- The following NuGet packages installed:
  - `Microsoft.Extensions.Http.Resilience`
  - `Microsoft.AspNetCore.Diagnostics.HealthChecks`
  - `Microsoft.Extensions.Diagnostics.HealthChecks`
  - `Polly`
  - `Polly.RateLimiting`
  - `Polly.Retry`
 
- We would need OpenWeatherMap API , To get a free API key for OpenWeatherMap.org, follow these steps:
   1. Visit OpenWeatherMap.org:
   2. Go to OpenWeatherMap.org.
   3. Sign Up:
   4. If you don’t have an account, click on the "Sign Up" button and create a new account by filling out the required information.
   5. If you already have an account, click on the "Sign In" button and log in with your credentials.
   6. Access API Keys:
        * Once logged in, navigate to the "API keys" section.
        * This can usually be found under your account profile or dashboard.
        * You should see an option to generate a new API key.
        * Click on the "Generate" button to create a new key.
        * Copy Your API Key:
        * After generating, you will see your new API key listed. Copy this key to use in your application.
        * Start Using the API:
        * Use this API key in your requests to OpenWeatherMap's APIs. 
        * Ensure you include the key in your API request parameters, usually like this: &appid=YOUR_API_KEY.
        * Here is an example of how you can use the API key in a request to get the current weather data for a city:https://api.openweathermap.org/data/2.5/weather?q=London&appid=YOUR_API_KEY Replace YOUR_API_KEY with the key you obtained from the OpenWeatherMap site.
 
# Service Resiliency Pattern

Microservices resiliency patterns are essential mechanisms that enhance an application’s ability to handle failures and maintain performance in distributed environments. Here are several key resiliency patterns commonly used in microservices architecture:

* Circuit Breaker Pattern
* Retry Pattern
* Bulkhead Pattern
* Timeout Pattern
* Fallback Pattern
* Rate Limiter Pattern
* Health Check API Pattern

# Getting Started
* Clone the repo
* Replace YOUR_API_KEY with the key you obtained from the OpenWeatherMap site as shown below.
  - `var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={YOUR_API_KEY}&units=metric";`
* Run the application by pressing CTRL + F5. It will show the Swagger UI.
  
  ![image](https://github.com/user-attachments/assets/f1afedd6-b1d8-47da-bba7-61cc2c20cedd)

  # Conclusion
  * In essence, resilient microservices design is like building a strong, flexible backbone for modern software systems.
  * By separating tasks into small, independent services and applying clever strategies like circuit breakers and graceful degradation, we ensure that our systems can handle problems without collapsing entirely.
  * Real-world examples from big companies like Netflix and Amazon show us the power of these approaches in action.

  
