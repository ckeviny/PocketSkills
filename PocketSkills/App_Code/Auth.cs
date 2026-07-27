using System;
using System.Web;
using System.Web.Configuration;
using Microsoft.Identity.Client;

public static class Auth
{
    public static readonly string[] Scopes = { "User.Read", "Files.Read" };

    /// <summary>
    /// Builds the exact redirect URI registered in the Azure AD app registration for this host.
    /// Must be identical between the AuthLogin request and the AuthCallback token exchange.
    /// </summary>
    public static string GetRedirectUri(HttpRequestBase request)
    {
        var scheme = request.Url.Host == "localhost" ? "http" : "https";
        return scheme + "://" + request.Url.Authority + "/AuthCallback.cshtml";
    }

    /// <summary>
    /// Builds the confidential client used for the server-side OAuth code exchange and
    /// silent token refresh. Reuses the same app registration/secret as the Key Vault
    /// service principal in Azure.cs.
    /// </summary>
    public static IConfidentialClientApplication BuildApp(HttpRequestBase request)
    {
        return ConfidentialClientApplicationBuilder
            .Create(Environment.GetEnvironmentVariable("Client_ID"))
            .WithClientSecret(WebConfigurationManager.ConnectionStrings["ClientSecret"].ConnectionString)
            .WithAuthority("https://login.microsoftonline.com/common")
            .WithRedirectUri(GetRedirectUri(request))
            .Build();
    }
}
