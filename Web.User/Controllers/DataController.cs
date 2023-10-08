using Common.Dtos;
using Common.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.User.Helpers;
using Web.User.Services;

namespace Web.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;
        private readonly AuthService _authService;
        private readonly CacheHelper _cacheHelper;
        private readonly LookupsService _lookupsService;
        private readonly PartnerService _partnerService;
        private readonly ViewHelper _viewHelper;

        public DataController(
            ILogger<DataController> logger, 
            AuthService authService, 
            CacheHelper cacheHelper, 
            LookupsService lookupsService, 
            PartnerService partnerService,
            ViewHelper viewHelper)
        {
            _logger = logger;
            _authService = authService;
            _cacheHelper = cacheHelper;
            _lookupsService = lookupsService;
            _viewHelper = viewHelper;
            _partnerService = partnerService;
        }

        private string GetAccessToken()
        {
            var loginDetails = _viewHelper.GetLoginDetails(User);
            var loginData = loginDetails.Item2;
            return loginData.AccessToken;
        }

        [HttpGet("checkuser/{userName}")]
        public async Task<IActionResult> CheckUserExists(string userName, CancellationToken cancellationToken)
        {
            var result = await _authService.CheckUserExists(userName, cancellationToken);
            BaseResponseDto baseResponseDto = new BaseResponseDto
            {
                Success = result.Success
            };
            return new JsonResult(baseResponseDto);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("countries")]
        public async Task<IActionResult> GetAllCountries(CancellationToken cancellationToken)
        {
            var countryList = _cacheHelper.Get<CountryLookupResponseDto>(Constants.CountryListCacheKey);
            if (countryList == null)
            {
                var accessToken = GetAccessToken();
                countryList = await _lookupsService.GetAllCountriesAsync(accessToken, cancellationToken);
                if (countryList.Success)
                {
                    _cacheHelper.Set<CountryLookupResponseDto>(Constants.CountryListCacheKey, countryList);
                }
            }
            return new JsonResult(countryList);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("getCallingCode/{countryCode}")]
        public async Task<IActionResult> GetCallingCode(string countryCode, CancellationToken cancellationToken)
        {
            var countryList = _cacheHelper.Get<CountryLookupResponseDto>(Constants.CountryListCacheKey);
            if (countryList != null)
            {
                if (countryList.Countries.Any(countryList => countryList.CountryCode == countryCode))
                {
                    var callingCode = countryList.Countries.First(countryList => countryList.CountryCode == countryCode).CallingCode;
                    var response = new GetCallingCodeResponseDto { Success = true, CallingCode = callingCode };
                    return new JsonResult(response);
                }
            }

            var accessToken = GetAccessToken();
            var result = await _lookupsService.GetCallingCodeAsync(countryCode, accessToken, cancellationToken);
            return new JsonResult(result);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("states")]
        public async Task<IActionResult> GetAllStates(CancellationToken cancellationToken)
        {
            var stateList = _cacheHelper.Get<StateLookupResponseDto>(Constants.StateListCacheKey);
            if (stateList == null)
            {
                var accessToken = GetAccessToken();
                stateList = await _lookupsService.GetAllStatesAsync(accessToken, cancellationToken);
                if (stateList.Success)
                {
                    _cacheHelper.Set<StateLookupResponseDto>(Constants.StateListCacheKey, stateList);
                }
            }
            return new JsonResult(stateList);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("states/{countryCode}")]
        public async Task<IActionResult> GetStatesByCountry(string countryCode, CancellationToken cancellationToken)
        {
            var stateList = _cacheHelper.Get<StateLookupResponseDto>(Constants.StateListCacheKey);
            if (stateList == null)
            {
                var accessToken = GetAccessToken();
                stateList = await _lookupsService.GetAllStatesAsync(accessToken, cancellationToken);
                if (stateList.Success)
                {
                    _cacheHelper.Set<StateLookupResponseDto>(Constants.StateListCacheKey, stateList);
                }
            }
            StateLookupResponseDto stateLookupResponseDto = new StateLookupResponseDto();
            if (stateList.Success)
            {
                stateLookupResponseDto.States = stateList.States.Where(s => s.CountryCode == countryCode).ToList();
            }
            else
            {
                stateLookupResponseDto.SetError(stateList.Message);
            }
            return new JsonResult(stateLookupResponseDto);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("partners/recent")]
        public async Task<IActionResult> GetRecentPartners(CancellationToken cancellationToken)
        {
            var accessToken = GetAccessToken();
            var partnerDto = await _partnerService.GetRecentPartners(accessToken, cancellationToken);
            return new JsonResult(partnerDto);
        }
    }
}
