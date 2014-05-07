using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
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
        private DateTime _dateAndTime;

        public DateTimePicker()
        {

            InitializeComponent();
        }

        public DateTime Value
        {
            get { return _dateAndTime; }
            set
            {
                DateTime date = new DateTime();
                    date = DatePicker.DisplayDate;

                DateTime time = (DateTime) TimePicker.Value;

                _dateAndTime = date + time.TimeOfDay;

            }
        }
    }
}
