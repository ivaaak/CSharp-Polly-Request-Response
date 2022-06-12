using Polly;
using Polly.Retry;
using System.Net;

namespace RequestService.Policies
{
    public class ClientPolicy
    {
        public AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetry { get; }

        public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry { get; }

        public AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetry { get; }

        public AsyncRetryPolicy<HttpResponseMessage> InfiniteHttpRetry { get; }

        public ClientPolicy()
        {
            // Retries immediately up to 10 times after a failed response.
            ImmediateHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode)
                .RetryAsync(10);

            // Retries up to 5 times with a 3 second delay between attempts.
            LinearHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(3));

            // Retries up to 5 times and with each following attempt takes 2^n seconds to the next retry.
            ExponentialHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            // Wait and retry forever
            InfiniteHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode)
              .WaitAndRetryForeverAsync(retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public async Task TestPolicy()
        {
            // Different Policy which isnt implemented currently.
            // Handle primitive return values (implied use of .Equals())
            Policy
              .HandleResult<HttpStatusCode>(HttpStatusCode.InternalServerError)
              .OrResult(HttpStatusCode.BadGateway);

            // Handle both exceptions and return values in one policy
            HttpStatusCode[] httpStatusCodesWorthRetrying = {
               HttpStatusCode.RequestTimeout, // 408
               HttpStatusCode.InternalServerError, // 500
               HttpStatusCode.BadGateway, // 502
               HttpStatusCode.ServiceUnavailable, // 503
               HttpStatusCode.GatewayTimeout // 504
            };

            /*
            HttpResponseMessage result = await Policy
              .Handle<HttpRequestException>()
              .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
              .RetryAsync(10)
              .ExecuteAsync(Func<Task<HttpResponseMessage>>);
            */
        }
    }
}
