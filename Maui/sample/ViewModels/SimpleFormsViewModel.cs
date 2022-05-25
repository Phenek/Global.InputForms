using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Global.InputForms.Models;
using Sample.Models;
using Microsoft.Maui.Controls;
using Sample.Services;

namespace Sample.ViewModels
{
    public class SimpleFormsViewModel : BaseViewModel
    {
        private string _entryText = "Global.InputForms";

        private DateTime _date;

        private FormModel _form = new FormModel();

        private TimeSpan _time;

        private string selectedItem;

        public SimpleFormsViewModel()
        {
            Rates = new ObservableDictionary<string, object>
            {
                {"0", "none"},
                {"1", "Less than 1 year"},
                {"2", "Between 1 and 2 years"},
                {"3", "More than 2 years"}
            };

            Genders = new ObservableDictionary<string, object>
            {
                {"1", Translate.GetText("Form.Man")},
                {"2", Translate.GetText("Form.Woman")}
            };

            Languages = new ObservableCollection<string>
            {
                Translate.GetText("Form.English"),
                Translate.GetText("Form.Spanish"),
                Translate.GetText("Form.Russian")
            };

            Time = new TimeSpan(DateTime.Now.Ticks);
            Date = DateTime.Today.AddYears(1);
            SelectedItem = Translate.GetText("Form.Russian");
        }

        public TimeSpan Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public string SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        public string EntryText
        {
            get => _entryText;
            set => SetProperty(ref _entryText, value);
        }

        public FormModel Form
        {
            get => _form;
            set
            {
                _form = value;
                SetProperty(ref _form, value);
            }
        }

        public ObservableDictionary<string, object> Rates { get; set; }
        public ObservableDictionary<string, object> Genders { get; set; }
        public ObservableCollection<string> Languages { get; set; }

        public async void OnSubmit()
        {
            IsBusy = true;
            try
            {
                await Task.Run(() => Task.Delay(3000));
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}