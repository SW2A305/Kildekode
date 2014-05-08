using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace McSntt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl
    {


        private bool _isReadOnly = false;

        public DateTimePicker()
        {

            InitializeComponent();

        }

        public DateTime Value
        {
            get
            {
                return (DatePicker.SelectedDate.GetValueOrDefault() + TimePicker.Value.GetValueOrDefault().TimeOfDay);
            }
            set
            {
                DatePicker.SelectedDate = value;
                TimePicker.Value = value;
            }
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                if (value == true)
                {
                    DatePicker.IsEnabled = false;
                    TimePicker.IsEnabled = false;
                }
                else
                {
                    DatePicker.IsEnabled = true;
                    TimePicker.IsEnabled = true;
                }
            }
        }
    }
}
