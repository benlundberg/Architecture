﻿using Architecture.Controls;
using Architecture.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            Username = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>("You need to provide a username")
            });

            Password = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>("You need to provied a password")
            });
        }

        private ICommand forgotPasswordCommand;
        public ICommand ForgotPasswordCommand => forgotPasswordCommand ?? (forgotPasswordCommand = new Command(async () =>
        {
            if (Device.Idiom == TargetIdiom.Desktop)
            {
                await Navigation.PushModalAsync(new Views.Desktop.RecoverPasswordPage { BindingContext = new RecoverPasswordViewModel { Navigation = this.Navigation } });
            }
            else
            {
                await Navigation.PushModalAsync(new Views.Phone.RecoverPasswordPage { BindingContext = new RecoverPasswordViewModel { Navigation = this.Navigation } });
            }
        }));

        private ICommand loginCommand;
        public ICommand LoginCommand => loginCommand ?? (loginCommand = new Command(async () =>
        {
            if (IsBusy)
            {
                return;
            }

            if (!Username.Validate() || !Password.Validate())
            {
                return;
            }

            var loading = new LoadingPopup("Signing you in");

            try
            {
                IsBusy = true;

                await loading.ShowAsync();

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, GetType().ToString(), sendToService: false);
                ShowAlert(ex.Message, "Exception");
            }
            finally
            {
                IsBusy = false;
                await loading.HideAsync();
            }
        }));

        public ValidatableObject<string> Username { get; set; }
        public ValidatableObject<string> Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
