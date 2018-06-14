﻿using Acr.UserDialogs;
using Architecture.Core;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;

namespace Architecture
{
    public class PermissionHelper
    {
        public async Task CheckPermissionAsync(Permission permission, bool showSettings = true)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);

                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);

                    status = results[permission];
                }

                if (status != PermissionStatus.Granted)
                {
                    if (showSettings)
                    {
                        var res = await UserDialogs.Instance.ConfirmAsync(
                             message: TranslateHelper.Translate("Permission_Message") + " " + permission.ToString() + ". " + TranslateHelper.Translate("Permission_Settings"),
                             title: TranslateHelper.Translate("Permission_Title") + " " + permission.ToString(),
                             okText: TranslateHelper.Translate("Gen_Yes"),
                             cancelText: TranslateHelper.Translate("Gen_No"));

                        if (res)
                        {
                            try
                            {
                                CrossPermissions.Current.OpenAppSettings();
                            }
                            catch (Exception ex)
                            {
                                ex.Print();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ITranslateHelper translateHelper;
        public ITranslateHelper TranslateHelper => translateHelper ?? (translateHelper = ComponentContainer.Current.Resolve<ITranslateHelper>());
    }
}
