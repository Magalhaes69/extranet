using Application.Shared.Behavior;
using Application.Shared.Helps;
using Application.UseCases.IncomingPayments.GetAllIncomingPayments;
using Domain.Helps;
using Domain.Helps.Enum;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Application.UseCases.Dashboard
{
    public sealed class GetDashboardMetricHandler : IRequestHandler<GetDashboardMetricRequest, DashboardMetricResponse>
    {
        //private readonly DashboardMetricCache _cache;


        private readonly ISapClient _sapClient;
        private readonly IMemoryCache _cache;

        public GetDashboardMetricHandler(ISapClient sapClient, IMemoryCache cache)
        {

            _sapClient = sapClient;
            _cache = cache;
        }

        //public async Task<DashboardMetricResponse> Handle(GetDashboardMetricRequest request, CancellationToken ct)
        //{
        //    var def = DashboardMetrics.All[request.Key];

        //    return (await _cache.GetOrCreateAsync($"dash:{request.Key}:{request.CardCode}", async entry =>
        //    {
        //        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);

        //        var filter = def.BuildFilter(request.CardCode);

        //        var countTask = _sapClient.CountAsync(def.Entity, filter, ct);


        //        var sumTask = def.Amount.Count > 0
        //            ? _sapClient.SumAsync(def.Entity, def.Amount.Select(a => a.Field).ToList(), filter, ct)
        //            : Task.FromResult(Array.Empty<decimal>());


        //        var currencyTask = _sapClient.GetLocalCurrencyAsync(ct);

        //        await Task.WhenAll(countTask, sumTask, currencyTask);

        //        decimal? amount = def.Amount.Count == 0
        //            ? null
        //            : sumTask.Result.Select((value, i) => value * def.Amount[i].Sign).Sum();

        //        if (def.IncludeChecks)
        //            amount = (amount ?? 0) + await GetCheckTotalAsync(request.CardCode, ct);

        //        return new DashboardMetricResponse
        //        {
        //            Key = request.Key,
        //            Count = countTask.Result,
        //            Amount = amount,
        //            Currency = currencyTask.Result
        //        };
        //    }))!;
        //}
        //public async Task<DashboardMetricResponse> Handle(GetDashboardMetricRequest request, CancellationToken ct)
        //{
        //    var def = DashboardMetrics.All[request.Key];
        //    var cacheKey = $"dash:{request.Key}:{request.CardCode}";

        //    if (_cache.TryGetValue(cacheKey, out DashboardMetricResponse? cached) && cached is not null)
        //    {
        //       // _logger.LogInformation("Dashboard {Key}: cache hit", request.Key);
        //        return cached;
        //    }

        //    var filter = def.BuildFilter(request.CardCode);

        //    var countWatch = Stopwatch.StartNew();
        //    var count = await _sapClient.CountAsync(def.Entity, filter, ct);
        //    countWatch.Stop();

        //    var currencyWatch = Stopwatch.StartNew();
        //    var currency = await _sapClient.GetLocalCurrencyAsync(ct);
        //    currencyWatch.Stop();

        //    //_logger.LogInformation(
        //    //    "Dashboard {Key}: count {CountMs} ms, currency {CurrencyMs} ms, filter: {Filter}",
        //    //    request.Key, countWatch.ElapsedMilliseconds, currencyWatch.ElapsedMilliseconds, filter);

        //    var response = new DashboardMetricResponse
        //    {
        //        Key = request.Key,
        //        Count = count,
        //        Amount = 0,
        //        Currency = currency
        //    };

        //    _cache.Set(cacheKey, response, TimeSpan.FromSeconds(60));
        //    return response;
        //}

        //public Task<DashboardMetricResponse> Handle(GetDashboardMetricRequest request, CancellationToken ct)
        //{
        //    var def = DashboardMetrics.All[request.Key];
        //    var cacheKey = $"dash:{request.Key}:{request.Part}:{request.CardCode}";

        //    return _cache.GetAsync(
        //        cacheKey,
        //        (sap, token) => request.Part == MetricPart.Count
        //            ? LoadCountAsync(sap, def, request, token)
        //            : LoadAmountAsync(sap, def, request, token),
        //        ct);
        //}

        //private static async Task<DashboardMetricResponse> LoadCountAsync(
        //    ISapClient sap, DashboardMetricDefinition def, GetDashboardMetricRequest request, CancellationToken ct)
        //{
        //    var count = await sap.CountAsync(def.Entity, def.BuildFilter(request.CardCode), ct);
        //    return new DashboardMetricResponse { Key = request.Key, Count = count };
        //}

        //private static async Task<DashboardMetricResponse> LoadAmountAsync(
        //    ISapClient sap, DashboardMetricDefinition def, GetDashboardMetricRequest request, CancellationToken ct)
        //{
        //    if (def.Amount.Count == 0 && !def.IncludeChecks)
        //        return new DashboardMetricResponse { Key = request.Key };

        //    var filter = def.BuildFilter(request.CardCode);
        //    decimal amount = 0;

        //    if (def.Amount.Count > 0)
        //    {
        //        var sums = await sap.SumAsync(def.Entity, def.Amount.Select(a => a.Field).ToList(), filter, ct);
        //        amount = sums.Select((value, i) => value * def.Amount[i].Sign).Sum();
        //    }

        //    if (def.IncludeChecks)
        //        amount += await GetCheckTotalAsync(sap, request.CardCode, ct);

        //    return new DashboardMetricResponse
        //    {
        //        Key = request.Key,
        //        Amount = amount,
        //        Currency = await sap.GetLocalCurrencyAsync(ct) // cached after the first call
        //    };
        //}

        //private static async Task<decimal> GetCheckTotalAsync(ISapClient sap, string cardCode, CancellationToken ct)
        //{
        //    var paymentsRequest = new GetAllIncomingPaymentsRequest
        //    {
        //        CardCode = cardCode,
        //        Cancelled = new List<string> { "tNO" },
        //        DynamicFilters = new List<DynamicFilter>
        //    {
        //        new()
        //        {
        //            Field = "DocDate",
        //            Operator = OperatorType.GreaterThanOrEqual,
        //            Value1 = new DateTime(DateTime.Today.Year, 1, 1).ToString("yyyy-MM-dd")
        //        }
        //    }
        //    };

        //    if (!IncomingPaymentCheckTotalParams.TryBuild(paymentsRequest, out var sqlParams))
        //        return 0;

        //    var row = await sap.ExecuteQueryObjAsync<CheckTotalRowResponse>(
        //        "IncomingPaymentsCheckTotal",
        //        sqlParams.ToDictionary(x => x.Key, x => (object)x.Value),
        //        ct);

        //    return row?.TotalCheckSum ?? 0;
        //}
        public async Task<DashboardMetricResponse> Handle(GetDashboardMetricRequest request, CancellationToken ct)
        {
            var def = DashboardMetrics.All[request.Key];

            return (await _cache.GetOrCreateAsync($"dash:{request.Key}:{request.CardCode}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);

                var filter = def.BuildFilter(request.CardCode);
                var hasAmount = def.Amount.Count > 0 || def.IncludeChecks;

                // Cartões sem total: só a contagem (1 chamada ao SAP)
                if (!hasAmount)
                {
                    return new DashboardMetricResponse
                    {
                        Key = request.Key,
                        Count = await _sapClient.CountAsync(def.Entity, filter, ct)
                    };
                }

                // Faturas: contagem + soma + moeda
                var countTask = _sapClient.CountAsync(def.Entity, filter, ct);
                var sumTask = _sapClient.SumAsync(def.Entity, def.Amount.Select(a => a.Field).ToList(), filter, ct);
                var currencyTask = _sapClient.GetLocalCurrencyAsync(ct);

                await Task.WhenAll(countTask, sumTask, currencyTask);

                decimal amount = sumTask.Result.Select((value, i) => value * def.Amount[i].Sign).Sum();

                if (def.IncludeChecks)
                    amount += await GetCheckTotalAsync(request.CardCode, ct);

                return new DashboardMetricResponse
                {
                    Key = request.Key,
                    Count = await countTask,
                    Amount = amount,
                    Currency = await currencyTask
                };
            }))!;
        }
        private async Task<decimal> GetCheckTotalAsync(string cardCode, CancellationToken ct)
        {
            var paymentsRequest = new GetAllIncomingPaymentsRequest
            {
                CardCode = cardCode,
                Cancelled = new List<string> { "tNO" },
                DynamicFilters = new List<DynamicFilter>
        {
            new()
            {
                Field = "DocDate",
                Operator = OperatorType.GreaterThanOrEqual,
                Value1 = new DateTime(DateTime.Today.Year, 1, 1).ToString("yyyy-MM-dd")
            }
        }
            };

            if (!IncomingPaymentCheckTotalParams.TryBuild(paymentsRequest, out var sqlParams))
                return 0;

            var row = await _sapClient.ExecuteQueryObjAsync<CheckTotalRowResponse>(
                "IncomingPaymentsCheckTotal",
                sqlParams.ToDictionary(x => x.Key, x => (object)x.Value),
                ct);

            return row?.TotalCheckSum ?? 0;
        }
    }
}


