using System;
using System.Text.RegularExpressions;
using Global.InputForms;
using Naxam.I18n;
using Naxam.I18n.Forms;
using Xamarin.Forms;

namespace SampleApp.Behaviors
{
    public class CheckGroupValidatorBehavior : Behavior<FrameInfo>
    {
        protected override void OnAttachedTo(FrameInfo bindable)
        {
            bindable.Validators += Validation;
            base.OnAttachedTo(bindable);
        }

        protected void Validation(object sender, EventArgs e)
        {
            var i18N = DependencyService.Get<IDependencyGetter>().Get<ILocalizedResourceProvider>();

            if (sender is CheckGroup checkGroup && checkGroup.Parent is FrameInfo frameInfo && !frameInfo.Info)
            {
                var isValid = false;
                foreach (var item in checkGroup.CheckList)
                    if (item.Checked)
                    {
                        isValid = true;
                        break;
                    }

                frameInfo.ShowInfo(!isValid, i18N.GetText("Error.Check"));
            }
        }

        protected override void OnDetachingFrom(FrameInfo bindable)
        {
            bindable.Validators -= Validation;
            base.OnDetachingFrom(bindable);
        }
    }

    public class RadioGroupValidatorBehavior : Behavior<FrameInfo>
    {
        protected override void OnAttachedTo(FrameInfo bindable)
        {
            if (bindable is FrameInfo frameInfo)
            {
                bindable.Validators += Validation;
                base.OnAttachedTo(bindable);
            }
        }

        protected void Validation(object sender, EventArgs e)
        {
            var i18N = DependencyService.Get<IDependencyGetter>().Get<ILocalizedResourceProvider>();

            if (sender is RadioGroup radioGroup && radioGroup.Parent is FrameInfo frameInfo && !frameInfo.Info)
            {
                var isValid = radioGroup.SelectedItem.Key != null;
                frameInfo.ShowInfo(!isValid, i18N.GetText("Error.Check"));
            }
        }

        protected override void OnDetachingFrom(FrameInfo bindable)
        {
            bindable.Validators -= Validation;
            base.OnDetachingFrom(bindable);
        }
    }

    public class RateGroupValidatorBehavior : Behavior<FrameInfo>
    {
        protected override void OnAttachedTo(FrameInfo bindable)
        {
            if (bindable is FrameInfo frameInfo)
            {
                bindable.Validators += Validation;
                base.OnAttachedTo(bindable);
            }
        }

        protected void Validation(object sender, EventArgs e)
        {
            var i18N = DependencyService.Get<IDependencyGetter>().Get<ILocalizedResourceProvider>();

            if (sender is RateGroup rateGroup && rateGroup.Parent is FrameInfo frameInfo && !frameInfo.Info)
            {
                var isValid = rateGroup.SelectedIndex != -1;
                frameInfo.ShowInfo(!isValid, i18N.GetText("Error.Check"));
            }
        }

        protected override void OnDetachingFrom(FrameInfo bindable)
        {
            bindable.Validators -= Validation;
            base.OnDetachingFrom(bindable);
        }
    }

    public class EmptyValidatorBehavior : Behavior<EntryView>
    {
        protected override void OnAttachedTo(EntryView bindable)
        {
            bindable.Validators += Validation;
            base.OnAttachedTo(bindable);
        }

        protected void Validation(object sender, EventArgs e)
        {
            var i18N = DependencyService.Get<IDependencyGetter>().Get<ILocalizedResourceProvider>();

            if (sender is EntryView entryView && !entryView.Info)
            {
                var isValid = !string.IsNullOrEmpty(entryView.EntryText);
                entryView.ShowInfo(!isValid, i18N.GetText("Error.Empty"));
            }
        }

        protected override void OnDetachingFrom(EntryView bindable)
        {
            bindable.Validators -= Validation;
            base.OnDetachingFrom(bindable);
        }
    }

    public class EmailCompareValidationBehavior : Behavior<EntryView>
    {
        public static readonly BindableProperty CompareToEntryProperty = BindableProperty.Create("CompareToEntry",
            typeof(EntryView), typeof(EmailCompareValidationBehavior));

        public EntryView CompareToEntry
        {
            get => (EntryView) GetValue(CompareToEntryProperty);
            set => SetValue(CompareToEntryProperty, value);
        }

        protected override void OnAttachedTo(EntryView bindable)
        {
            bindable.Validators += Validation;
            base.OnAttachedTo(bindable);
        }

        protected void Validation(object sender, EventArgs e)
        {
            var i18N = DependencyService.Get<IDependencyGetter>().Get<ILocalizedResourceProvider>();

            if (sender is EntryView entryView && !entryView.Info && !string.IsNullOrEmpty(CompareToEntry.EntryText))
            {
                var entryText = CompareToEntry.EntryText;
                var confirmEntryText = entryView.EntryText;
                var isSame = entryText.Equals(confirmEntryText);

                var errorText = entryView.IsPassword
                    ? i18N.GetText("Error.PasswordMatch")
                    : i18N.GetText("Error.EmailMatch");
                entryView.ShowInfo(!isSame, errorText);
                CompareToEntry.ShowInfo(!isSame, errorText);
            }
        }

        protected override void OnDetachingFrom(EntryView bindable)
        {
            bindable.Validators -= Validation;
            base.OnDetachingFrom(bindable);
        }
    }

    public class EmailValidatorBehavior : Behavior<EntryView>
    {
        private const string EmailRegex =
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))"
            + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        protected override void OnAttachedTo(EntryView bindable)
        {
            bindable.Validators += Validation;
            base.OnAttachedTo(bindable);
        }

        protected void Validation(object sender, EventArgs e)
        {
            var i18N = DependencyService.Get<IDependencyGetter>().Get<ILocalizedResourceProvider>();

            if (sender is EntryView entryView && !entryView.Info && !string.IsNullOrEmpty(entryView.EntryText))
            {
                var isValid = false;
                isValid = Regex.IsMatch(entryView.EntryText, EmailRegex, RegexOptions.IgnoreCase,
                    TimeSpan.FromMilliseconds(250));
                entryView.ShowInfo(!isValid, i18N.GetText("Error.InvalidEmail"));
            }
        }

        protected override void OnDetachingFrom(EntryView bindable)
        {
            bindable.Validators -= Validation;
            base.OnDetachingFrom(bindable);
        }
    }

    internal class DateMajorValidationBehavior : Behavior<DatePickerView>
    {
        protected override void OnAttachedTo(DatePickerView bindable)
        {
            bindable.Validators += Validation;
            base.OnAttachedTo(bindable);
        }

        private void Validation(object sender, EventArgs e)
        {
            var i18N = DependencyService.Get<IDependencyGetter>().Get<ILocalizedResourceProvider>();

            if (sender is DatePickerView datePickerView)
                if (datePickerView.Date is DateTime date)
                {
                    var majorAge = date.AddYears(18);
                    var isMajor = majorAge <= DateTime.Today;
                    datePickerView.ShowInfo(!isMajor, i18N.GetText("Error.MajorAge"));
                }
        }

        protected override void OnDetachingFrom(DatePickerView bindable)
        {
            bindable.Validators -= Validation;
            base.OnDetachingFrom(bindable);
        }
    }

    public class NumberValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += TextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= TextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void TextChanged(object sender, TextChangedEventArgs args)
        {
            var isValid = int.TryParse(args.NewTextValue, out var result);

            ((Entry) sender).TextColor = isValid ? Color.Default : Color.Red;
        }
    }

    public class MaxLengthValidatorBehavior : Behavior<Entry>
    {
        public static readonly BindableProperty MaxLengthProperty =
            BindableProperty.Create("MaxLength", typeof(int), typeof(MaxLengthValidatorBehavior), 0);

        public int MaxLength
        {
            get => (int) GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += TextChanged;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= MaxLength)
                ((Entry) sender).Text = e.NewTextValue.Substring(0, MaxLength);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= TextChanged;
        }
    }
}