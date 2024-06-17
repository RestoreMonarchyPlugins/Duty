using System;

namespace RestoreMonarchy.Duty.Helpers;

internal static class ValidationHelper
{
    internal static bool IsUrlInvalid(string url, bool required = false)
    {
        if (string.IsNullOrEmpty(url))
        {
            return required;
        }

        return !Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}