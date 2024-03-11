using Npgsql;
using Polly.Retry;
using Polly;

namespace Crebito.Api;

public static class CrebitoResilienceFactory
{
    public static ResiliencePipeline Create()
    {
        return new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<NpgsqlException>().Handle<InvalidOperationException>(),
                BackoffType = DelayBackoffType.Linear,
                UseJitter = true,
                DelayGenerator = (args) =>
                {
                    var delay = args.AttemptNumber switch
                    {
                        0 => TimeSpan.Zero,
                        _ => TimeSpan.FromMilliseconds(Random.Shared.Next(15, 85))
                    };

                    return new ValueTask<TimeSpan?>(delay);
                },
                MaxRetryAttempts = 50
            })
            .Build();
    }
}
