using Autodesk.Forge;

namespace DA_DynamoRevit;

public static class Authentication
{

    /// <summary>
    /// Retrieves the Autodesk Forge Design Automation client ID from the environment variables.
    /// Throws an exception if the environment variable is missing or empty.
    /// </summary>
    /// <returns>The Autodesk Forge Design Automation client ID.</returns>
    public static string GetClientId()
    {
        var ClientID = Environment.GetEnvironmentVariable("APS_CLIENT_ID");
        if (string.IsNullOrEmpty(ClientID))
        {
            throw new Exception("Missing APS_CLIENT_ID environment variable.");
        }

        return ClientID;
    }

    /// <summary>
    /// Retrieves the Autodesk Forge Design Automation client secret from the environment variables.
    /// Throws an exception if the environment variable is missing or empty.
    /// </summary>
    /// <returns>The Autodesk Forge Design Automation client secret.</returns>
    public static string GetClientSecret()
    {
        var ClientSecret = Environment.GetEnvironmentVariable("APS_CLIENT_SECRET");
        if (string.IsNullOrEmpty(ClientSecret))
        {
            throw new Exception("Missing APS_CLIENT_SECRET environment variable.");
        }

        return ClientSecret;
    }

    /// <summary>
    /// Retrieves the Autodesk Forge Design Automation callback URL from the environment variables.
    /// Throws an exception if the environment variable is missing or empty.
    /// </summary>
    /// <returns>The Autodesk Forge Design Automation callback URL.</returns>
    public static string GetCallbackUrl()
    {
        var CallbackUrl = Environment.GetEnvironmentVariable("APS_CALLBACK_URL");
        if (string.IsNullOrEmpty(CallbackUrl))
        {
            throw new Exception("Missing APS_CALLBACK_URL environment variable.");
        }

        return CallbackUrl;
    }

    /// <summary>
    /// Retrieves a 2-legged access token from the Autodesk Forge API using client credentials.
    /// </summary>
    /// <returns>The 2-legged access token.</returns>
    /// <exception cref="Exception">Thrown when APS_CLIENT_ID or APS_CLIENT_SECRET environment variables are missing.</exception>
    public static async Task<string> Get2LeggedToken()
    {
        Autodesk.Forge.TwoLeggedApi twoLeggedApi = new Autodesk.Forge.TwoLeggedApi();
        var ClientID = Environment.GetEnvironmentVariable("APS_CLIENT_ID");
        var ClientSecret = Environment.GetEnvironmentVariable("APS_CLIENT_SECRET");
        if (string.IsNullOrEmpty(ClientID) || string.IsNullOrEmpty(ClientSecret))
        {
            throw new Exception("Missing APS_CLIENT_ID or APS_CLIENT_SECRET environment variables.");
        }

        dynamic token = await twoLeggedApi.AuthenticateAsync(ClientID, ClientSecret, "client_credentials",
            new Scope[]
            {
                Scope.DataRead, Scope.DataWrite, Scope.DataCreate, Scope.DataSearch, Scope.BucketCreate,
                Scope.BucketRead,Scope.CodeAll,
                Scope.BucketUpdate, Scope.BucketDelete
            }).ConfigureAwait(false);
        var access_token = token.access_token;
        if (string.IsNullOrEmpty(access_token))
        {
            throw new Exception("can't get access_token, please check again value APS_CLIENT_ID and APS_CLIENT_SECRET");
        }

        return access_token;
    }

    /// <summary>
    /// Retrieves a 2-legged access token from the Autodesk Forge API using client credentials.
    /// </summary>
    /// <param name="clientId">The client ID for authentication.</param>
    /// <param name="clientSecret">The client secret for authentication.</param>
    /// <returns>The 2-legged access token.</returns>
    /// <exception cref="Exception">Thrown when clientId or clientSecret is null or empty.</exception>
    public static async Task<string> Get2LeggedToken(string clientId, string clientSecret)
    {
        Autodesk.Forge.TwoLeggedApi twoLeggedApi = new Autodesk.Forge.TwoLeggedApi();
        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            throw new Exception("Missing APS_CLIENT_ID or APS_CLIENT_SECRET environment variables.");
        }

        dynamic token = await twoLeggedApi.AuthenticateAsync(clientId, clientSecret, "client_credentials",
            new Scope[]
            {
                Scope.DataRead, Scope.DataWrite, Scope.DataCreate, Scope.DataSearch, Scope.BucketCreate,
                Scope.BucketRead,
                Scope.BucketUpdate, Scope.BucketDelete
            }).ConfigureAwait(false);
        var access_token = token.access_token;
        return access_token;
    }

}