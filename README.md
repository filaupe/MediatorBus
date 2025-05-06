# MediatorBus

Um framework leve para .NET que implementa o padrão Mediator integrado a um Event Dispatcher, facilitando a separação de responsabilidades, o desacoplamento de componentes e a publicação/assinatura de eventos de forma simples e extensível.

> **Nota:** Este projeto foi baseado e inspirado no pacote [NetDevPack.SimpleMediator](https://www.nuget.org/packages/NetDevPack.SimpleMediator) desenvolvido por [desenvolvedor.io](https://www.nuget.org/profiles/desenvolvedor.io).

## Recursos principais

- **Mediator**: Encaminha comandos e eventos para os handlers registrados.
- **Event Dispatcher**: Permite listeners (sincronos e assíncronos) reagirem a eventos publicados.
- **Integração com DI**: Registro automático de handlers e dispatcher usando Microsoft.Extensions.DependencyInjection.
- **Extensível**: Crie seus próprios eventos, handlers, listeners e publishers facilmente.

---

## Instalação

Clone este repositório e adicione o projeto `MediatorBus` como referência no seu projeto .NET:

```bash
# Clone o repositório
 git clone <repo-url>

# Abra a solution no Visual Studio, Rider ou VS Code
```

Ou adicione como projeto local:

```bash
 dotnet add <seu-projeto>.csproj reference MediatorBus/MediatorBus.csproj
```

---

## Como funciona?

- **Requests** são enviados pelo Mediator para um único Handler.
- **Notifications** são publicadas e podem ser tratadas por múltiplos NotificationHandlers.
- **Handlers** podem repassar eventos para o EventDispatcher, que dispara listeners registrados via eventos.

### Diagrama Simplificado

```
[Publisher] --(Mediator.Publish)--> [NotificationHandler] --(EventDispatcher.PublishAsync)--> [Listeners]
```

---

## Exemplo de uso

Veja um exemplo de integração completa no projeto `MediatorBus.Sample`.

### 1. Defina um evento
```csharp
using MediatorBus.Interfaces;

public record SampleEvent(string Message) : INotification;
```

### 2. Implemente um Handler
```csharp
using MediatorBus.Interfaces;

public class SampleEventHandler(IEventDispatcher<SampleEvent> dispatcher) : INotificationHandler<SampleEvent>
{
    public async Task Handle(SampleEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handler recebeu o evento: {notification.Message}");
        await dispatcher.PublishAsync(notification);
    }
}
```

### 3. Crie um Listener
```csharp
using MediatorBus.Interfaces;

public class SampleListenerService
{
    public SampleListenerService(IEventDispatcher<SampleEvent> dispatcher)
    {
        dispatcher.OnPublished += OnEventReceived;
        dispatcher.OnPublishedAsync += OnEventReceivedAsync;
    }

    private static void OnEventReceived(object? sender, SampleEvent evt)
        => Console.WriteLine($"[ListenerService] Evento recebido (sync): {evt.Message}");

    private Task OnEventReceivedAsync(object sender, SampleEvent evt)
    {
        Console.WriteLine($"[ListenerService] Evento recebido (async): {evt.Message}");
        return Task.CompletedTask;
    }
}
```

### 4. Publisher via Mediator
```csharp
using MediatorBus.Interfaces;

public class SamplePublisherService(IMediator mediator)
{
    public async Task PublishMessageAsync(string message)
    {
        var evt = new SampleEvent(message);
        await mediator.Publish(evt);
    }
}
```

### 5. Configuração no Program.cs
```csharp
using MediatorBus;
using MediatorBus.Sample;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var services = new ServiceCollection();
services.AddMediator(Assembly.GetExecutingAssembly());
services.AddSingleton<SampleListenerService>();
services.AddSingleton<SamplePublisherService>();

var provider = services.BuildServiceProvider();
provider.GetRequiredService<SampleListenerService>();
var publisher = provider.GetRequiredService<SamplePublisherService>();

Console.WriteLine("Digite mensagens para publicar eventos (ou 'sair' para encerrar):");
string? input;
while ((input = Console.ReadLine()) != null && !input.Equals("sair", StringComparison.CurrentCultureIgnoreCase))
    await publisher.PublishMessageAsync(input);

Console.WriteLine("Aplicação encerrada.");
```

---

## Rodando o exemplo

1. Abra o terminal na raiz do projeto.
2. Execute:

```bash
 dotnet run --project MediatorBus.Sample
```

Digite mensagens no console para ver o fluxo Mediator → Handler → Dispatcher → Listener funcionando!

---

## Estrutura dos projetos

```
MediatorBus/             # Projeto do framework
MediatorBus.Sample/      # Projeto de exemplo
```

---

## Licença

MIT
