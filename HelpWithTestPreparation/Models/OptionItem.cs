using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpWithTestPreparation.Models
{
    public partial class OptionItem : ObservableObject
    {
        public string Text { get; set; }

        [ObservableProperty]
        private bool isSelected;
    }

}
