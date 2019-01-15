using System;

namespace KutyApp.Services.Environment.Bll.Entities.Model
{
    public class DbVersion
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }
    }
}
