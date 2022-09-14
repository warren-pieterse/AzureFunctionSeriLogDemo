using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.Elasticsearch;
using System;

[assembly: FunctionsStartup(typeof(AzureFunctionDemo.Startup))]


namespace AzureFunctionDemo;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        ElasticsearchSinkOptions options = new ElasticsearchSinkOptions(new Uri("http://elastic:yPlOLBzwB0AvLLVDh1Kt@picup-elknew-stack.eastus2.cloudapp.azure.com:9220/"))
        {
            IndexFormat = "routing-warren",
            AutoRegisterTemplate = true,
            //ModifyConnectionSettings = (c) => c.BasicAuthentication("yyy", "zzz"),
            //NumberOfShards = 2,
            //NumberOfReplicas = 0
        };

        builder.Services.AddSingleton<ILoggerProvider>((sp) =>
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(options)
                //.WriteTo.ApplicationInsights(sp.GetRequiredService<TelemetryClient>(), TelemetryConverter.Traces)
                .CreateLogger();

            return new SerilogLoggerProvider(Log.Logger, true);
        });
    }
}