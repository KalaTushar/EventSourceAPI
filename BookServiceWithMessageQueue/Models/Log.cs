namespace LoggingService.Models
{
    public class Log
    {
        public int LogId { get; set; }
        public string LogName { get; set; }
        public string LogType { get; set; }
        public DateTime LogCreated { get; set; }
    }
    public class RabbitMQConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
    }
    public class Message
    {
        public Guid Id { get; set; }
        public Types MessageType { get; set; }
        public TypeOfMethod Method { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public enum Types
    {
        retail,
        payment
    }
    public enum TypeOfMethod
    {
        post,
        get
    }
}
