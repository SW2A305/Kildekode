/**
 * ============================================================================================
 * Class borrowed from http://www.codeproject.com/Tips/547627/Highlight-dates-on-a-WPF-Calendar
 * ============================================================================================
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace McSntt.Views.Helpers
{
    public class CalenderBackground
    {
        private readonly List<Dates> _datelist = new List<Dates>();
        private readonly List<Overlays> _overlaylist = new List<Overlays>();
        private Calendar _calendar;

        public CalenderBackground() {}

        public CalenderBackground(Calendar cal)
        {
            this._calendar = cal;
        }

        public string GrayoutWeekends { get; set; }

        public void SetCalenderBackground(Calendar cal)
        {
            this._calendar = cal;
        }

        public void ClearDates()
        {
            this._datelist.Clear();
        }

        public void AddOverlay(string id, string filename)
        {
            this._overlaylist.Add(new Overlays(id, filename));
        }

        public void AddDate(DateTime date, string overlay)
        {
            this._datelist.Add(new Dates(date, overlay));
        }

        public void RemoveDate(DateTime date, string overlay)
        {
            this._datelist.Remove(new Dates(date, overlay));
        }

        public ImageBrush GetBackground()
        {
            // Calculate the first shown date in the calendar
            DateTime displaydate = this._calendar.DisplayDate;
            var firstdayofmonth = new DateTime(displaydate.Year, displaydate.Month, 1);
            var dayofweek = (int) firstdayofmonth.DayOfWeek;

            if (dayofweek == 0)
            {
                dayofweek = 7; // set sunday to day 7.
            }

            if (dayofweek == (int) this._calendar.FirstDayOfWeek)
            {
                dayofweek = 8; // show a whole week ahead
            }

            if (this._calendar.FirstDayOfWeek == DayOfWeek.Sunday)
            {
                dayofweek += 1;
            }

            DateTime firstdate = firstdayofmonth.AddDays(-((Double) dayofweek) + 1);

            Debug.WriteLine("displayd date    {0} ", displaydate);
            Debug.WriteLine("firstdayofmonth  {0} ", firstdayofmonth);
            Debug.WriteLine("dayofweek        {0} ", dayofweek);
            Debug.WriteLine("firstdate        {0} ", firstdate);

            // Create default background image
            var rtBitmap = new RenderTargetBitmap(178 /* PixelWidth */, 160 /* PixelHeight */, 96 /* DpiX */, 96
                                                  /* DpiY */, PixelFormats.Default);
            var drawVisual = new DrawingVisual();

            using (DrawingContext dc = drawVisual.RenderOpen())
            {
                var backGroundBrush = new LinearGradientBrush
                                      {
                                          StartPoint = new Point(0.5, 0),
                                          EndPoint = new Point(0.5, 1)
                                      };
                backGroundBrush.GradientStops.Add(new GradientStop(
                                                      (Color) ColorConverter.ConvertFromString("#FFE4EAF0"), 0.0));
                backGroundBrush.GradientStops.Add(new GradientStop(
                                                      (Color) ColorConverter.ConvertFromString("#FFECF0F4"), 0.16));
                backGroundBrush.GradientStops.Add(new GradientStop(
                                                      (Color) ColorConverter.ConvertFromString("#FFFCFCFD"), 0.16));
                backGroundBrush.GradientStops.Add(new GradientStop(
                                                      (Color) ColorConverter.ConvertFromString("#FFFFFFFF"), 1));

                dc.DrawRectangle(backGroundBrush, null, new Rect(0, 0, rtBitmap.Width, rtBitmap.Height));
            }

            rtBitmap.Render(drawVisual);

            using (DrawingContext dc = drawVisual.RenderOpen())
            {
                for (int y = 0; y < 6; y++)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        int xpos = x*21 + 17;
                        int ypos = y*16 + 50;

                        if (y == 2)
                        {
                            ypos -= 1;
                        }

                        if (y == 3)
                        {
                            ypos -= 2;
                        }

                        if (y == 4)
                        {
                            ypos -= 2;
                        }

                        if (y == 5)
                        {
                            ypos -= 3;
                        }

                        foreach (
                            string overlayid in this._datelist.Where(c => c.date == firstdate).Select(c => c.overlay))
                        {
                            if (overlayid != null)
                            {
                                Overlays overlays = this._overlaylist.FirstOrDefault(c => c.id == overlayid);

                                try
                                {
                                    dc.DrawRectangle(overlays.Brush, null,
                                                     new Rect(xpos, ypos, overlays.BitMap.Width, overlays.BitMap.Height));
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }

                        if (this.GrayoutWeekends != ""
                            && (firstdate.DayOfWeek == DayOfWeek.Saturday || firstdate.DayOfWeek == DayOfWeek.Sunday))
                        {
                            Overlays overlays =
                                this._overlaylist.Where(c => c.id == this.GrayoutWeekends).FirstOrDefault();

                            try
                            {
                                dc.DrawRectangle(overlays.Brush, null /* no pen */,
                                                 new Rect(xpos, ypos, overlays.BitMap.Width, overlays.BitMap.Height));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }

                        firstdate = firstdate.AddDays(1);
                    }
                }
            }

            rtBitmap.Render(drawVisual);


            var brush = new ImageBrush(rtBitmap); // create a brush using the BitMap 
            return brush;
        }

        private static BitmapImage BitmapImage(string filename, out ImageBrush imageBrush)
        {
            var overlay = new BitmapImage(new Uri(filename, UriKind.Relative));
            imageBrush = new ImageBrush();
            imageBrush.ImageSource = overlay;
            imageBrush.Stretch = Stretch.Uniform;
            imageBrush.TileMode = TileMode.None;
            imageBrush.AlignmentX = AlignmentX.Center;
            imageBrush.AlignmentY = AlignmentY.Center;
            imageBrush.Opacity = 0.75;
            return overlay;
        }

        private class Dates
        {
            public Dates(DateTime _date, string _overlay)
            {
                this.date = _date;
                this.overlay = _overlay;
            }

            public DateTime date { get; set; }
            public string overlay { get; set; }
        }

        private class Overlays
        {
            public readonly BitmapImage BitMap;
            public readonly ImageBrush Brush;

            public Overlays(string _id, string _filename)
            {
                this.id = _id;

                this.BitMap = BitmapImage(_filename, out this.Brush);
            }

            public string id { get; set; }
        }
    }
}
