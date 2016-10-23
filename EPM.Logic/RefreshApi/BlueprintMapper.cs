namespace DBSoft.EPM.Logic.RefreshApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DAL.Interfaces;
    using DAL.Requests;
    using DAL.Services.AccountApi;
    using EVEAPI.Entities.Blueprint;
    using NLog;

    public class BlueprintMapper : EveApiMapper, IBlueprintMapper
    {
        private readonly IBlueprintInstanceService _blueprintInstanceService;
        private readonly IAccountApiService _accounts;
        private readonly IBlueprintService _service;

        public BlueprintMapper(IBlueprintInstanceService blueprints, IAccountApiService accounts, 
            IEveApiStatusService statusService, IBlueprintService service)
            : base(statusService)
        {
            _blueprintInstanceService = blueprints;
            _accounts = accounts;
            _service = service;
        }

        public async Task Pull(string token)
        {
            const string serviceName = "BlueprintList";

            await _blueprintInstanceService.DeleteAll(token);

            var instanceQueue = new List<SaveBlueprintInstanceRequest>();
            var cachedUntil = DateTime.Now;
            foreach (var account in _accounts.List(token))
            {
                try
                {
                    var response = await _service.Load(account.ApiKeyType, account.ApiKeyID, account.ApiVerificationCode, account.EveApiID);
                    ProcessBlueprints(response.Data, token, instanceQueue);
                    cachedUntil = response.CachedUntil;
                }
                catch (Exception e)
                {
                    SaveError(serviceName, token, e.Message);
                    throw;
                }
            }
            
            foreach (var request in instanceQueue)
            {
                try
                {
                    _blueprintInstanceService.SaveBlueprintInstance(request);
                }
                catch (Exception e)
                {
                    LogManager.GetCurrentClassLogger().Error(e);
                }
            }

            UpdateStatus(serviceName, cachedUntil, token);
        }

        private static void ProcessBlueprints(IEnumerable<Blueprint> eveBlueprints, string token,
            List<SaveBlueprintInstanceRequest> instanceQueue)
        {
            instanceQueue.AddRange(eveBlueprints.Select(blueprint => new SaveBlueprintInstanceRequest
            {
                Token = token, 
                AssetID = blueprint.AssetID, 
                BlueprintItemID = blueprint.BlueprintID,
                IsCopy = blueprint.IsCopy, 
                MaterialEfficiency = blueprint.MaterialEfficiency, 
                ProductionEfficiency = blueprint.TimeEfficiency
            }));
        }
    }
}