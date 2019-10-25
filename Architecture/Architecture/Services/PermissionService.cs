using Architecture.Core;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Architecture
{
    public class PermissionService
    {
        /// <summary>
        /// Checking if user have permission for different services (example: Camera or Location)
        /// </summary>
        /// <param name="permission">Type of permission to check</param>
        /// <param name="showSettings">Boolean if prompting dialog to go into settings</param>
        /// <returns>Returns true or false depend on if permission is granted</returns>
        public async Task<bool> CheckPermissionAsync(Permission permission, bool showSettings = true)
        {
            try
            {
                // Check if permission is already done
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);

                // If permission is not granted we prompt a dialog to ask for permission
                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);

                    status = results[permission];
                }

                // If permission is not granted here we could prompt a "Open settings?" dialog
                if (status != PermissionStatus.Granted)
                {
                    if (showSettings)
                    {
                        var res = await Application.Current.MainPage.DisplayAlert(
                             title: TranslateHelper.Translate("Permission_Title") + " " + permission.ToString(),
                             message: TranslateHelper.Translate("Permission_Message") + " " + permission.ToString() + ". " + TranslateHelper.Translate("Permission_Settings"),
                             accept: TranslateHelper.Translate("Gen_Yes"),
                             cancel: TranslateHelper.Translate("Gen_No"));

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

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ITranslateService translateHelper;
        public ITranslateService TranslateHelper => translateHelper ?? (translateHelper = ComponentContainer.Current.Resolve<ITranslateService>());
    }
}
