using System;
using System.Windows.Controls;

namespace McSntt.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        private bool _isReadOnly = false;

        public DateTimePicker() { this.InitializeComponent(); }

        public DateTime Value
        {
            get
            {
                return (this.DatePicker.SelectedDate.GetValueOrDefault()
                        + this.TimePicker.Value.GetValueOrDefault().TimeOfDay);
            }
            set
            {
                this.DatePicker.SelectedDate = value;
                this.TimePicker.Value = value;
            }
        }

        public bool IsReadOnly
        {
            get { return this._isReadOnly; }
            set
            {
                if (value)
                {
                    this.DatePicker.IsEnabled = false;
                    this.TimePicker.IsEnabled = false;
                }
                else
                {
                    this.DatePicker.IsEnabled = true;
                    this.TimePicker.IsEnabled = true;
                }
            }
        }
    }
}
