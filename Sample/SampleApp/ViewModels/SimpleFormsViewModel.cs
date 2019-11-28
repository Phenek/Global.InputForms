using System;
using System.Collections.Generic;
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

        private FormModel _form = new FormModel();

        private TimeSpan _time;
        public TimeSpan Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private string selectedItem;
        public string SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

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


            Languages = new ObservableDictionary<string, object>
            {
                {"English", _i18N.GetText("Form.English")},
                {"Spanish", _i18N.GetText("Form.Spanish")},
                {"Russian", _i18N.GetText("Form.Russian")}
            };

            LanguagesList = new List<string>
            {
                _i18N.GetText("Form.English"),
                _i18N.GetText("Form.Spanish"),
                _i18N.GetText("Form.Russian")
            };

            Time = new TimeSpan(DateTime.Now.Ticks);
            Date = DateTime.Today.AddYears(1);
            SelectedItem = _i18N.GetText("Form.Russian");
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
        public ObservableDictionary<string, object> Languages { get; set; }
        public List<string> LanguagesList { get; set; }

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