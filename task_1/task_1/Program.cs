using System;

class CustomEventArgs : EventArgs
{
    public String Dispatch 
    { 
        get; set;
    }
}

class EventBus
{
    private static readonly EventBus _EventBusInstance = new EventBus();
    private readonly Dictionary<object, List<Action<CustomEventArgs>>> _actions = new Dictionary<object, List<Action<CustomEventArgs>>>();

    public static EventBus Instance => _EventBusInstance;

    private EventBus() { }

    public void Subscribe(object publisher, Action<CustomEventArgs> eventHandler)
    {
        if (!_actions.ContainsKey(publisher))
        {
            _actions.Add(publisher, new List<Action<CustomEventArgs>>());
        }
        _actions[publisher].Add(eventHandler);
    }

    public void Unsubscribe(object publisher, Action<CustomEventArgs> eventHandler)
    {
        if (_actions.ContainsKey(publisher))
        {
            _actions[publisher].Remove(eventHandler);
        }
    }

    public void Publish(object publisher, CustomEventArgs args)
    {
        if (_actions.ContainsKey(publisher))
        {
            foreach (var action in _actions[publisher])
            {
                action(args);
            }
        }
    }
}
class Publisher
{
    public void Publish(string message)
    {
        EventBus.Instance.Publish(this, new CustomEventArgs { Dispatch = message });
    }
}

class SubscriberA
{
    public void EventHandler(CustomEventArgs arg)
    {
        System.Console.WriteLine($"SubscriberA message::{ arg.Dispatch}");
    }
}

class SubscriberB
{
    public void EventHandler(CustomEventArgs arg)
    {
        System.Console.WriteLine($"SubscriberB message::{arg.Dispatch}");
    }
}

class Task1
{
    static void Main()
    {
        Publisher pubA = new Publisher();
        SubscriberA subA = new SubscriberA();
        EventBus.Instance.Subscribe(pubA, subA.EventHandler);
        pubA.Publish("pubA test");

        Publisher pubB = new Publisher();
        SubscriberB subB = new SubscriberB();
        EventBus.Instance.Subscribe(pubB, subB.EventHandler);
        pubB.Publish("pubB test");
        EventBus.Instance.Subscribe(pubA, subB.EventHandler);
        pubB.Publish("pubA 2nd test");

        EventBus.Instance.Unsubscribe(pubA, subA.EventHandler);
        EventBus.Instance.Unsubscribe(pubA, subB.EventHandler);
        EventBus.Instance.Unsubscribe(pubB, subB.EventHandler);
        pubA.Publish("this is error message");
        pubB.Publish("this is 2nd error message");
    }
}