using TcpServerService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<TcpServer>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
