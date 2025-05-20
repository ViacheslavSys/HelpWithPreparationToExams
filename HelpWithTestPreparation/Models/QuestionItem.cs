using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpWithTestPreparation.Models
{
    public class QuestionItem
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public List<string> Answers { get; set; }
    }

}
