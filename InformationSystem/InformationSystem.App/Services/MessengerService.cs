using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Services.Interfaces;

namespace InformationSystem.App.Services;

public class MessengerService : IMessengerService
{
    public IMessenger Messenger { get; }
    
    public MessengerService(IMessenger messenger)
    {
        Messenger = messenger;
    }
    
    public void Send<TMessage>(TMessage message) where TMessage : class
    {
        Messenger.Send(message);
    }
}