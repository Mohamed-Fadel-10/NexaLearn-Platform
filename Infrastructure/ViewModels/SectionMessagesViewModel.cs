using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class SectionMessagesViewModel
    {
        public string Text { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PhotoPath { get; set; }
    }
}
