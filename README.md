# dotnet-core-3x-hangfire
dotnet new api --name DotnetCore.Hangfire.Api

dotnet add package HangFire.Core

dotnet add package HangFire.AspNetCore

dotnet add package HangFire.SqlServer

CREATE DATABASE [HangfireTest]
GO

#appsettings.json
"ConnectionStrings": {
    "HangfireConnection": "Server=.;Database=HangfireTest;Integrated Security=SSPI;"
  }
  
   "Logging": {
    "LogLevel": {
      "Hangfire": "Information"
    }
  }
  
 #Run project and Hangfire 
 dotnet watch run
 
 http://localhost:5000/hangfire
  
  
  # Job Examples
  //Fire and Forgot Job
  
  BackgroundJob.Enqueue(() => Console.WriteLine("Fire and Forgot Job"));

  //Schedule Job, run after xx time
  
  BackgroundJob.Schedule(() => Console.WriteLine("Scheduled after 5 seconds"), TimeSpan.FromSeconds(5));

  //Recurring Daily 15:10
  
  RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring Daily 15:10" + DateTime.Now), Cron.Daily(15, 10));

  //Recurring every minute
  
  RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring every minute" + DateTime.Now), Cron.Minutely());

  //Recurring every 5 minute
  
  RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring every minute" + DateTime.Now), "5 * * * *");
  
