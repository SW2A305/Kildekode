using System;
using System.Data.Entity;
using System.Windows.Controls;

namespace McSntt.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for Boats.xaml
    /// </summary>
    /// ry>
    public partial class Boats : UserControl
    {
        public Boats()
        {
            InitializeComponent();


            using (var db = new McSntttContext())
            {
                #region BoatUI

                db.Boats.Load();
                BoatComboBox.ItemsSource = db.Boats.Local;
                BoatComboBox.DisplayMemberPath = "NickName";
                BoatComboBox.SelectedValuePath = "Id";

                #endregion
            }
        }

        private void BoatComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            //TODO: Change all UI data to the current boats' info

            // Write the name to the NameBlock text
            string text = (sender as ComboBox).Text;
            if (text != null) NameBlock.Text = text;
        }
    }
}