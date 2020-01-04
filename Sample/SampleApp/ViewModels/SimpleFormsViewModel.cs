using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Global.InputForms.Models;
using Naxam.I18n;
using Naxam.I18n.Forms;
using SampleApp.Models;
using Xamarin.Forms;

namespace SampleApp.ViewModels
{
    public class SimpleFormsViewModel : BaseViewModel
    {
        private readonly ILocalizedResourceProvider _i18N = DependencyService.Get<IDependencyGetter>()
            .Get<ILocalizedResourceProvider>();

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
                {"1", _i18N.GetText("Form.Man")},
                {"2", _i18N.GetText("Form.Woman")}
            };

            Languages = new ObservableCollection<string>
            {
                _i18N.GetText("Form.English"),
                _i18N.GetText("Form.Spanish"),
                _i18N.GetText("Form.Russian")
            };

            Time = new TimeSpan(DateTime.Now.Ticks);
            Date = DateTime.Today.AddYears(1);
            SelectedItem = _i18N.GetText("Form.Russian");
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