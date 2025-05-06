using MediatorBus;
using MediatorBus.Sample;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var services = new ServiceCollection();

// Registra o Mediator e os handlers
services.AddMediator(Assembly.GetExecutingAssembly());

// Registra serviços Singleton
services.AddSingleton<SampleListenerService>();
services.AddSingleton<SamplePublisherService>();

var provider = services.BuildServiceProvider();

// Inicializa o listener (registra nos eventos do dispatcher)
provider.GetRequiredService<SampleListenerService>();

var publisher = provider.GetRequiredService<SamplePublisherService>();

Console.WriteLine("Digite mensagens para publicar eventos (ou 'sair' para encerrar):");
string? input;
while ((input = Console.ReadLine()) != null && !input.Equals("sair", StringComparison.CurrentCultureIgnoreCase))
    await publisher.PublishMessageAsync(input);

Console.WriteLine("Aplicação encerrada.");
