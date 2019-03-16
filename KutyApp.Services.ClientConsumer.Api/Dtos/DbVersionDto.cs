using System;

namespace KutyApp.Client.Services.ClientConsumer.Dtos
{
    public class DbVersionDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }
    }
}
