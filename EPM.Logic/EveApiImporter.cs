namespace DBSoft.EPM.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using DAL.Aspects;
    using DAL.DTOs;
    using DbSoft.Cache.Aspect;
    using JetBrains.Annotations;
    using RefreshApi;

    [UsedImplicitly]
    public class EveApiImporter : IEveApiImporter
    {
        private readonly IAssetMapper _assetMapper;
        private readonly IMarketOrderMapper _orderMapper;
        private readonly IAccountBalanceMapper _balanceMapper;
        private readonly IIndustryJobMapper _jobMapper;
        private readonly ITransactionMapper _transactionMapper;
        private readonly IBlueprintMapper _blueprintMapper;
        private readonly IMarketImportMapper _marketMapper;
        private readonly IImportNotifier _notifier;

        private readonly string[] _steps =
        {
            "AssetList", "BlueprintList", "AccountBalance", "MarketOrders",
            "IndustryJobs", "MarketImports", "WalletTransactions"
        };
        private readonly List<string> _running;

        public EveApiImporter(IAssetMapper assetMapper, IMarketOrderMapper orderMapper,
            IAccountBalanceMapper balanceMapper,
            IIndustryJobMapper jobMapper, ITransactionMapper transactionMapper, IBlueprintMapper blueprintMapper,
            IMarketImportMapper marketMapper,
            IImportNotifier notifier)
        {
            _assetMapper = assetMapper;
            _orderMapper = orderMapper;
            _balanceMapper = balanceMapper;
            _jobMapper = jobMapper;
            _transactionMapper = transactionMapper;
            _blueprintMapper = blueprintMapper;
            _marketMapper = marketMapper;
            _notifier = notifier;
            _running = new List<string>();
        }

        [Trace]
        public async Task Start(string token)
        {
            try
            {
                var allsteps = new[]
                {
                    "AssetList", "BlueprintList", "AccountBalance", "MarketOrders", "IndustryJobs", "MarketImports",
                    "WalletTransactions"
                };
                StartSteps(token, allsteps);
                await
                    Task.WhenAll(PullAssetList(token), PullMarketImport(token),
                        DoPull(token, "AccountBalance", _balanceMapper.Pull));
            }
            catch (Exception)
            {
                StopSteps(token, _running.ToArray());
            }
            finally { Flush(token); }
        }

        public IEnumerable<EveApiStatusDTO> List(string token)
        {
            return (from step in _steps
                    where _running.Any(f => f == step)
                    select new EveApiStatusDTO { ApiName = step, Result = "Running" }).ToList();
        }

        private void StartSteps(string token, params string[] names)
        {
            foreach (var name in names)
            {
                _running.Add(name);
                _notifier.Start(token, name);
            }
        }

        private void StopSteps(string token, params string[] names)
        {
            foreach (var name in names)
            {
                _running.Remove(name);
                _notifier.Stop(token, name);
            }
        }

        private async Task DoPull(string token, string name, Func<string, Task> func)
        {
            Trace.WriteLine($"Pulling {name}");
            await func(token);
            StopSteps(token, name);
            Trace.WriteLine($"{name} Pulled");
        }

        private async Task PullAssetList(string tk)
        {
            await DoPull(tk, "AssetList", _assetMapper.Pull);
            Flush(tk);
            await Task.WhenAll(
                        DoPull(tk, "BlueprintList", _blueprintMapper.Pull),
                        DoPull(tk, "MarketOrders", _orderMapper.Pull),
                        DoPull(tk, "IndustryJobs", _jobMapper.Pull));
        }

        private async Task PullMarketImport(string tk)
        {
            await DoPull(tk, "MarketImports", _marketMapper.Pull);
            Flush(tk);
            await DoPull(tk, "WalletTransactions", _transactionMapper.Pull);
        }

        private static void Flush(string token)
        {
            Trace.WriteLine("Flushing");
            CacheService.ClearCache(token);
        }
    }
}
