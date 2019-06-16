using Android;
using Android.App;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using System.Collections.Generic;

namespace OpenExtensions.Droid.Services
{
    /// <summary>
    /// A service to check and request permissions.
    /// </summary>
    public class PermissionService
    {
        private readonly Activity ThisActivity;

        /// <summary>
        /// Initialize a new PermissionService for the current Activity.
        /// </summary>
        public PermissionService(Activity thisActivity)
        {
            ThisActivity = thisActivity;
        }

        /// <summary>
        /// Check if a permission was granted.
        /// </summary>
        /// <param name="permission">Permission string provided from the <see cref="Manifest.Permission"/> properties.</param>
        /// <returns>True if permission was granted.</returns>
        public bool HasPermission(string permission)
        {
            if (ContextCompat.CheckSelfPermission(ThisActivity, permission) == Permission.Granted)
                return true;
            return false;
        }

        /// <summary>
        /// Check if multiple permissions were granted.
        /// </summary>
        /// <param name="permissions">Permission strings provided from the <see cref="Manifest.Permission"/> properties.</param>
        /// <returns>Dictionary with Permission name as key and a boolean as value indicating if permission was granted.</returns>
        public Dictionary<string, bool> HasPermissions(params string[] permissions)
        {
            var result = new Dictionary<string, bool>();
            foreach (var permission in permissions)
            {
                result[permission] = HasPermission(permission);
            }
            return result;
        }

        /// <summary>
        /// Check if you have arleady shown the reason why you need this permission.
        /// </summary>
        /// <param name="permission">Permission strings provided from the <see cref="Manifest.Permission"/> properties.</param>
        public bool ShouldShowRequestPermissionRationale(string permission)
        {
            return ActivityCompat.ShouldShowRequestPermissionRationale(ThisActivity, permission);
        }

        /// <summary>
        /// Request a specific permission, no check is done to see if permission is already granted.
        /// </summary>
        /// <param name="permission">Permission string provided from the <see cref="Manifest.Permission"/> properties.</param>
        /// <param name="requestCode"></param>
        public void RequestPermission(string permission, int requestCode = 0)
        {
            ActivityCompat.RequestPermissions(ThisActivity, new string[] { permission }, requestCode);
        }

        /// <summary>
        /// Request multiple permissions, no check is done to see if any permissions are already granted.
        /// request code is set to 0.
        /// </summary>
        /// <param name="permissions">Permission strings provided from the <see cref="Manifest.Permission"/> properties.</param>
        public void RequestPermissions(params string[] permissions)
        {
            ActivityCompat.RequestPermissions(ThisActivity, permissions, 0);
        }
    }
}
