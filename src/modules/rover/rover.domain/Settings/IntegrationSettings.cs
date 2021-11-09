namespace rover.domain.Settings
{
    public  class IntegrationSettings
    {
        public string RabbitMQConnectionString { get; set; }
        public string RabbitMQQueue { get; set; }
        public string RabbitMQPublishExchange { get; set; }
        public string RabbitMQReadExchange { get; set; }
        public int TimeDistanceOfMessageInSeconds { get; set; }
        public int TimeDistanceOfVoyageInSeconds { get; set; }
    }
}
