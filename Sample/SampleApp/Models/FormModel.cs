using System;

namespace SampleApp.Models
{
    public class FormModel : BaseModel
    {
        private string _firstname  = string.Empty;
        private string _lastname = string.Empty;
        private string _gender = string.Empty;
        private string _email = string.Empty;
        private string _emailConfirm = string.Empty;
        private string _password = string.Empty;
        private string _zipCode = string.Empty;
        private string _city = string.Empty;
        private DateTime _birthDate = DateTime.Today;
        private bool _english;
        private bool _spanish;
        private bool _russian;
        private int _rateSpanish;
        private int _rateRussian;

        public FormModel()
        {
        }

        public string Firstname
        {
            get => _firstname;
            set => SetProperty(ref _firstname, value);
        }

        public string Lastname
        {
            get => _lastname;
            set => SetProperty(ref _lastname, value);
        }

        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string EmailConfirm
        {
            get => _emailConfirm;
            set => SetProperty(ref _emailConfirm, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ZipCode
        {
            get => _zipCode;
            set => SetProperty(ref _zipCode, value);
        }

        public string City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set => SetProperty(ref _birthDate, value);
        }

        public bool English
        {
            get => _english;
            set => SetProperty(ref _english, value);
        }

        public bool Spanish
        {
            get => _spanish;
            set => SetProperty(ref _spanish, value);
        }

        public bool Russian
        {
            get => _russian;
            set => SetProperty(ref _russian, value);
        }

        private int _rateEnglish;
        public int RateEnglish
        {
            get => _rateEnglish;
            set => SetProperty(ref _rateEnglish, value);
        }

        public int RateSpanish
        {
            get => _rateSpanish;
            set => SetProperty(ref _rateSpanish, value);
        }

        public int RateRussian
        {
            get => _rateRussian;
            set => SetProperty(ref _rateRussian, value);
        }
    }
}