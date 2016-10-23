using Microsoft.AspNetCore.Mvc;

namespace Dbsoft.Epm.Web.Controllers.Maintenance
{
    using AutoMapper;
    using DBSoft.EPM.DAL.Interfaces;
    using DBSoft.EPM.DAL.Services;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Requests;
    using DBSoft.EPM.DAL.Services.AccountApi;
    using DBSoft.EPM.Logic;

    public class MaintenanceController : EpmController
    {
        private readonly IConfigurationService _config;
        private readonly IConfigurationProcessor _configProcessor;
        private readonly IUniverseService _universeService;
        private readonly IAccountService _accounts;
        private readonly IEveAccountProcessor _accountProcessor;

        public MaintenanceController(IConfigurationService config, IConfigurationProcessor configProcessor, IUniverseService universe, 
            IUserService users, IAccountService accounts, IEveAccountProcessor accountProcessor) : base(users)
        {
            _config = config;
            _configProcessor = configProcessor;
            _universeService = universe;
            _accounts = accounts;
            _accountProcessor = accountProcessor;
        }

        public ActionResult Accounts()
        {
            var model = new AccountsModel
            {
                Accounts = Mapper.Map<IEnumerable<AccountDTO>, List<AccountModel>>(
                        _accounts.List(new AccountRequest { Token = Token, IncludeDeleted = true }))
            };
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> SaveAccount([FromBody] AccountModel model)
        {
            try
            {
                var account = Mapper.Map<AccountDTO>(model);
                var result = await _accountProcessor.SaveAccount(account, Token);
                return Json(new
                {
                    Success = true,
                    Model = result
                });
            }
            catch (Exception e)
            {
                var result = Json(new { Success = false, e.Message });
                return result;
            }
        }


        public ActionResult Configuration()
        {
            var settings = _config.List(Token);
            var model = Mapper.Map<ConfigurationModel>(settings);
            model.ConfigurationValid = _configProcessor.ConfigurationIsValid(Token);

            return View(model);
        }

        [HttpPost]
        public JsonResult Configuration([FromBody] ConfigurationModel config)
        {
            var settings = Mapper.Map<ConfigurationSettingsDTO>(config);
            _config.SaveSettings(Token, settings);
            return Json("OK");
        }

        private JsonResult JsonResponse(dynamic model)
        {
            return Json(new
            {
                Success = true,
                Model = model
            });
        }

        public async Task<JsonResult> RegionLookup(int regionId)
        {
            return JsonResponse((await _universeService
                            .ListRegions())
                            .SingleOrDefault(f => f.RegionID == regionId));
        }

        public JsonResult StationLookup(int stationId)
        {
            var result = _universeService
                        .ListStations()
                        .SingleOrDefault(f => f.StationID == stationId);
            return JsonResponse(result);
        }

        public JsonResult StationList(string stationPartial)
        {
            return JsonResponse(_universeService
                        .ListStations()
                        .Where(f => f.StationName.StartsWith(stationPartial, StringComparison.InvariantCultureIgnoreCase))
                        .OrderBy(f => f.StationName));
        }

        public JsonResult SolarSystemList(string systemPartial)
        {
            return JsonResponse(_universeService.ListSolarSystems()
                .Where(f => f.SolarSystemName.StartsWith(systemPartial, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(f => f.SolarSystemName));
        }

        public JsonResult SolarSystemLookup(int? solarSystemId)
        {
            return JsonResponse(_universeService
                .ListSolarSystems()
                .SingleOrDefault((f => f.SolarSystemID == solarSystemId)));
        }

    }
}
