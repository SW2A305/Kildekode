using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using McSntt.DataAbstractionLayer;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for NewLecture.xaml
    /// </summary>
    public partial class NewLecture : Window
    {
        public int Year;
        public int Month;
        public int Day;
        public NewLecture()
        {
            InitializeComponent();

        }

        private void txtName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CheckIsNumeric(e);
        }

        private void CheckIsNumeric(TextCompositionEventArgs e)
        {
            int result;

            if(!(int.TryParse(e.Text, out result)))
            {
                e.Handled = true;
            }
        }
        private void NewLectureYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((NewLectureYear.Text == string.Empty))
            {
                NewLectureYear.Text = "2014";
            }
            Year = int.Parse(NewLectureYear.Text);
        }

        private void NewLectureMonth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((NewLectureMonth.Text == string.Empty))
            {
                NewLectureMonth.Text = "01";
            }
            Year = int.Parse(NewLectureMonth.Text);
        }

        private void NewLectureDay_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((NewLectureDay.Text == string.Empty))
            {
                NewLectureDay.Text = "01";
            }
            Year = int.Parse(NewLectureDay.Text);
        }

        private void CompleteLectureCreate_Click(object sender, RoutedEventArgs e)
        {
             var lecture = new Lecture{DateOfLecture = new DateTime(Year, Month, Day)};
             //Should add this to current team list of lectures in DB
        }
    }
}
