using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Architecture.Controls;
using System.Collections.Generic;
using System;
using SkiaSharp;

namespace Architecture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartsPage : ContentPage
    {
        public ChartsPage()
        {
            InitializeComponent();

            Random rand = new Random();

            LineChart.ChartEntries = new List<ChartEntry>();
            LineChart.SliderDetailOrientation = StackOrientation.Vertical;
            LineChart.VerticalUnit = "kWh";

            ChartEntry chartEntry1 = new ChartEntry
            {
                Color = SKColors.Red,
                Items = new List<ChartEntryItem>()
            };

            for (int i = 0; i < 11; i++)
            {
                var date = DateTime.Now.AddYears(i);
                var value = rand.Next(80, 400);

                chartEntry1.Items.Add(new ChartEntryItem
                {
                    Label = date.Year.ToString(),
                    Value = value
                });
            }

            ChartEntry chartEntry2 = new ChartEntry
            {
                Color = SKColors.Green,
                Items = new List<ChartEntryItem>()
            };

            for (int i = 0; i < 11; i++)
            {
                var date = DateTime.Now.AddYears(i);
                var value = rand.Next(80, 400);

                chartEntry2.Items.Add(new ChartEntryItem
                {
                    Label = date.Year.ToString(),
                    Value = value
                });
            }

            ChartEntry chartEntry3 = new ChartEntry
            {
                Color = SKColors.Blue,
                Items = new List<ChartEntryItem>()
            };

            for (int i = 0; i < 11; i++)
            {
                var date = DateTime.Now.AddYears(i);
                var value = rand.Next(80, 400);

                chartEntry3.Items.Add(new ChartEntryItem
                {
                    Label = date.Year.ToString(),
                    Value = value
                });
            }

            LineChart.ChartEntries.Add(chartEntry1);
            //LineChart.ChartEntries.Add(chartEntry2);
            LineChart.ChartEntries.Add(chartEntry3);
        }
    }
}