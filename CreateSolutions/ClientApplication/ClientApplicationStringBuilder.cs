﻿using System;
using System.Collections.Generic;

namespace CreateSolutions.ClientApplication
{
    public static class ClientApplicationStringBuilder
    {
        private static readonly
            Dictionary<string, (string conversion, string clientAppString, Func<string, string> parseString)>
            clientAppTypes = new Dictionary<string, (string, string, Func<string, string>)>
            {
                {"Browser-based", ("browser-based", ClientApplicationStrings.BrowserBasedComplete, ParseBrowserBased)},
                {
                    "Native mobile or tablet",
                    ("native-mobile", ClientApplicationStrings.NativeMobileComplete, ParseNativeMobile)
                },
                {
                    "Native desktop",
                    ("native-desktop", ClientApplicationStrings.NativeDesktopComplete, ParseNativeDesktop)
                }
            };

        public static string GetClientAppString(string ignoredSection = null,
            string clientApplicationTypes = "Browser-based")
        {
            var clientAppString = new List<string>();

            var finishedString = string.Empty;

            if (string.IsNullOrEmpty(ignoredSection))
            {
                foreach (var appType in clientAppTypes)
                    if (clientApplicationTypes.Contains(appType.Key))
                        clientAppString.Add(appType.Value.clientAppString);
                finishedString = string.Join(',', clientAppString);
            }
            else
            {
                foreach (var appType in clientAppTypes)
                    if (clientApplicationTypes.Contains(appType.Key))
                        finishedString += appType.Value.parseString(ignoredSection);
            }

            return BuildClientApplicationString(clientApplicationTypes, finishedString);
        }

        private static string BuildClientApplicationString(string clientApplicationType, string clientAppString)
        {
            var converted = new List<string>();

            foreach (var key in clientAppTypes.Keys)
                if (clientApplicationType.Contains(key))
                    converted.Add($"\"{clientAppTypes[key].conversion}\"");

            return string.Format("{{\"ClientApplicationTypes\":[{0}],{1}}}", string.Join(',', converted),
                clientAppString);
        }

        private static string ParseBrowserBased(string ignoredSection)
        {
            return ClientApplicationStrings.BrowserBasedComplete.Replace(
                ClientApplicationStrings.BrowserSections[ignoredSection], "");
        }

        private static string ParseNativeMobile(string ignoredSection)
        {
            return ClientApplicationStrings.NativeMobileComplete.Replace(
                ClientApplicationStrings.NativeMobileSections[ignoredSection], "");
        }

        private static string ParseNativeDesktop(string ignoredSection)
        {
            return ClientApplicationStrings.NativeDesktopComplete.Replace(
                ClientApplicationStrings.NativeDesktopSections[ignoredSection], "");
        }
    }
}
