using System;
using System.Collections.Generic;

namespace Architecture
{
    public class TaskOverviewViewModel : BaseViewModel
    {
        public TaskOverviewViewModel()
        {
            PickerValues = new List<string>();

            for (int i = 0; i < 12; i++)
            {
                PickerValues.Add(DateTime.Today.AddMonths(i).ToString("MMMM").ToUpper());
            }
        }

        public string SelectedItem { get; set; }
        public List<string> PickerValues { get; private set; }
    }
}
