#define OAuth_Eran

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

#if OAuth_Eran
using OAuth; 
#else
using DevDefined.OAuth;
#endif


public class OAuthMessageHandler : DelegatingHandler
{
    
	//keys/tokens for authentication
    private static string _consumerKey = "6KkjWGGTZ996fRp7l94MDL71m";
    private static string _consumerSecret = "N3JEixfVzuF9JhNg6QejEXsgF3EYxPXG8KAPEXcnB0XLp9AdXK";
    private static string _token = "1888246147-nbrIWbC0FccfSotqqjmWH3CUNk5HsSpslg9RqqX";
    private static string _tokenSecret = "ayX0EawTUphs1A9i2AnVcqp6oEMbDiWoPHKZFFP8gJkD4";

#if OAuth_Eran
    private OAuthBase _oAuthBase = new OAuthBase(); 
#endif

    public OAuthMessageHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {
        //empty
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //compute OAuth header
#if OAuth_Eran
        string normalizedUri, normalizedParameters, authHeader;

        string _timeStamp = _oAuthBase.GenerateTimeStamp();
        string _nonce = _oAuthBase.GenerateNonce();

        string signature = _oAuthBase.GenerateSignature(
            request.RequestUri,
            _consumerKey,
            _consumerSecret,
            _token,
            _tokenSecret,
            request.Method.Method,
            _timeStamp,
            _nonce,
            out normalizedUri,
            out normalizedParameters);

        Console.WriteLine(signature);
        //request.Headers.Authorization = new AuthenticationHeaderValue("OAuth", authHeader); 
        authHeader = GenerateAuthHeader(_consumerKey, _token, _timeStamp, _nonce, signature, request.Method.Method);
#endif
        Console.WriteLine(authHeader);
        request.Headers.Authorization = new AuthenticationHeaderValue("OAuth", authHeader);
        return base.SendAsync(request, cancellationToken);
    }

    private static string GenerateAuthHeader(string consumerKey, string token, string timeStamp, string nonce, string signature, string signatureMethod)
    {
        string authHeader;
        
        authHeader = string.Format(
            "oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", oauth_signature=\"{2}\", oauth_signature_method=\"{3}\", oauth_timestamp=\"{4}\", oauth_version=\"{5}\"",
            consumerKey,
            nonce,
            Uri.EscapeDataString(signature),
            signatureMethod,
            timeStamp,
            OAuthBase.GetVersion());

        if (string.IsNullOrEmpty(token))
        {
            return authHeader;
        }

        return authHeader + string.Format(", oauth_token=\"{0}\"", token);
    }
}