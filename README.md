# Chaso.Push.Server
It is a Windows Service run a SignalR Service and ASP.Net Web Api, ideal to centralize your push notifications, you can connect the many listeners  as broadcast to receive notifications in a specific channel.

## Dependecies
- Microsoft.AspNet.WebApi.Core
- Microsoft.AspNet.SignalR
- Microsoft.Owin
- NLog
- Serilog
- SimpleInjector
- Swashbuckle.Core

You Can use [Chaso.Push.Client](https://github.com/chasoliveira/Chaso.Push.Client) to connect to your Push Server to send and receive notifications.