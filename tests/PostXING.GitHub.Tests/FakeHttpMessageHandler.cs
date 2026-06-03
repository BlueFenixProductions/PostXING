using System.Net;
using System.Text;

namespace PostXING.GitHub.Tests;

/// <summary>One captured outbound request: method, resolved absolute URI, request headers
/// (case-insensitive), and the request body (null for GET).</summary>
internal sealed record CapturedRequest(HttpMethod Method, Uri Uri, IReadOnlyDictionary<string, string> Headers, string? Body)
{
    public string Path => Uri.AbsolutePath;
    public string Query => Uri.Query;
    public string? Authorization => Header("Authorization");
    public string? Header(string name) => Headers.TryGetValue(name, out var v) ? v : null;
}

/// <summary>Test double <see cref="HttpMessageHandler"/>: records every request and returns a
/// scripted response. Lets the gateway tests assert on verb/URL/headers/body without a network.</summary>
internal sealed class FakeHttpMessageHandler(Func<CapturedRequest, HttpResponseMessage> responder) : HttpMessageHandler
{
    public List<CapturedRequest> Requests { get; } = [];

    /// <summary>Always answer with the given status + JSON body.</summary>
    public static FakeHttpMessageHandler Respond(HttpStatusCode status, string json = "{}") =>
        new(_ => new HttpResponseMessage(status) { Content = new StringContent(json, Encoding.UTF8, "application/json") });

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var h in request.Headers)
            headers[h.Key] = string.Join(",", h.Value);
        Requests.Add(new CapturedRequest(request.Method, request.RequestUri!, headers, body));
        return responder(Requests[^1]);
    }
}
