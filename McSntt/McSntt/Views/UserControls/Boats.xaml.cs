using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using McSntt.DataAbstractionLayer;
using McSntt.Models;

namespace McSntt.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for Boats.xaml
    /// </summary>
    /// ry>
    public partial class Boats : UserControl
    {
        public Boat CurrentBoat = new Boat();
        public Boats()
        {
            InitializeComponent();
            /*var db = new BoatEfDal();
            db.GetAll(); */
            //Mock Boat
            var båd1 = new Boat() {NickName = "Bodil1", Operational = true};
            var båd2 = new Boat() {NickName = "båd"};
            var båd3 = new Boat() {NickName = "SS", ImagePath = "pack://application:,,,/Images/SundetLogo.PNG"};
            var boatList = new List<Boat>();
            boatList.Add(båd1);
            boatList.Add(båd2);
            boatList.Add(båd3);

            BoatComboBox.ItemsSource = boatList;
            BoatComboBox.DisplayMemberPath = "NickName";

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("pack://application:,,,/Images/gray.PNG");
            image.EndInit();
            BoatImage.Source = image; 
        }



        private void BoatComboBox_OnSelectionChanged(object sender, EventArgs e)
        {
            CurrentBoat = (Boat)BoatComboBox.SelectionBoxItem;
            NameLabel.Content = CurrentBoat.NickName;
            if (CurrentBoat.ImagePath != null)
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(CurrentBoat.ImagePath);
                image.EndInit();
                BoatImage.Source = image;
            }
            else
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri("pack://application:,,,/Images/gray.PNG");
                image.EndInit();
                BoatImage.Source = image; 
            }
            string operationel = "";
            if (CurrentBoat.Operational)
            {
                operationel = "Operationel";
            }
            else if (!CurrentBoat.Operational)
            {
                operationel = "Ikke operationel";
            }

            BoatTypeTextBox.Text = Enum.GetName(typeof(BoatType), CurrentBoat.Type);
            BoatStatusTextBox.Text = operationel;
        }
    }
}