# CSharp Polly Request Response

Polly is a . NET library that provides resilience and transient-fault handling capabilities.

This demo implements Polly policies such as Retry and Timeout. It consists of 2 API apps which create requests and responses.


# Request API:

Uses an HTTP Client to call the ResponseAPI and if the request isnt successful it executes the implemented Polly retry policies.

Examples of the Polly retry/result policies used:
```
ImmediateHttpRetry = Policy
    .HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
    .RetryAsync(10);

LinearHttpRetry = Policy
    .HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(3));

ExponentialHttpRetry = Policy
    .HandleResult<HttpResponseMessage>( res => !res.IsSuccessStatusCode)
    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
```

# Response API: 
 
Calling the API ( /api/response/successPercent ) with a number - successPercent between 0 and 100 (over 100 is always successful) the higher the number is the more likely the response is to be successful.

For example: 

- /api/response/90  --- returns ~ 90% Ok responses
- /api/response/10  --- returns ~ 10% Ok responses
