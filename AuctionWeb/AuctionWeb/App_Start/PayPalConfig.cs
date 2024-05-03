﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace AuctionWeb.App_Start
{
    public class PayPalConfig
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        static PayPalConfig() {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
            
        }

        public static Dictionary<string, string> GetConfig() {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken() {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        public static APIContext GetAPIContext() {
            var apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;

        }

    }
}