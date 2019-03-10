using System;
using System.Collections.Generic;
using System.Text;

namespace KutyApp.Services.Environment.Bll.Configuration
{
    public class FileSettings
    {
        public string RootDirectory { get; set; }
        public string ImagesPath { get; set; }
        public List<string> AllowedFileExtensions { get; set; }
    }
}
