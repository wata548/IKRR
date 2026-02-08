namespace Data {
    public class EventData {
        public int Chapter { get; private set; }
        public string Name { get; private set; }
        public string EventString { get; private set; }
        public Event.Event Event => _event ??= new(EventString);
        
        private Event.Event _event = null;
    }
}