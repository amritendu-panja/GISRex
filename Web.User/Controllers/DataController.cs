using Common.Dtos;
using Common.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.User.Helpers;
using Web.User.Models;
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
        private readonly Mapper _mapper;

        public DataController(
            ILogger<DataController> logger, 
            AuthService authService, 
            CacheHelper cacheHelper, 
            LookupsService lookupsService, 
            PartnerService partnerService,
            ViewHelper viewHelper,
            Mapper mapper)
        {
            _logger = logger;
            _authService = authService;
            _cacheHelper = cacheHelper;
            _lookupsService = lookupsService;
            _viewHelper = viewHelper;
            _partnerService = partnerService;
            _mapper = mapper;
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
		[HttpGet("checkdomain/{domain}")]
		public async Task<IActionResult> CheckDomainExists(string domain, CancellationToken cancellationToken)
		{
			var accessToken = _viewHelper.GetAccessToken(User);
            var result = await _partnerService.CheckDomainExistsAsync(domain, accessToken, cancellationToken);			
			return new JsonResult(result);
		}

		[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("countries")]
        public async Task<IActionResult> GetAllCountries(CancellationToken cancellationToken)
        {
            var countryList = _cacheHelper.Get<CountryLookupResponseDto>(Constants.CountryListCacheKey);
            if (countryList == null)
            {
                var accessToken = _viewHelper.GetAccessToken(User);
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

            var accessToken = _viewHelper.GetAccessToken(User);     
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
                var accessToken = _viewHelper.GetAccessToken(User);
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
                var accessToken = _viewHelper.GetAccessToken(User);
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
            var accessToken = _viewHelper.GetAccessToken(User);
            var partnerDto = await _partnerService.GetRecentPartners(accessToken, cancellationToken);
            return new JsonResult(partnerDto);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("users/recent")]
        public async Task<IActionResult> GetRecentUsers(CancellationToken cancellationToken)
        {
            var accessToken = _viewHelper.GetAccessToken(User);
            var partnerDto = await _authService.GetRecentUsersAsync(accessToken, cancellationToken);
            return new JsonResult(partnerDto);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("groups/table")]
        public async Task<IActionResult> GetAllGroups(CancellationToken cancellationToken)
        {
            var accessToken = _viewHelper.GetAccessToken(User);
            DataTableRequestModel requestModel = new DataTableRequestModel();
            _mapper.Map(Request.Form, requestModel);
            GetGroupsDataTableRequest request = new GetGroupsDataTableRequest();
            _mapper.Map(requestModel, request);
            request.UserGuid = Guid.Parse(_viewHelper.GetUserId(User));

            var groupList = await _lookupsService.GetGroupsDataTableAsync(request, accessToken, cancellationToken);
			return new JsonResult(groupList);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("users/table")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var accessToken = _viewHelper.GetAccessToken(User);
            DataTableRequestModel requestModel = new DataTableRequestModel();
            _mapper.Map(Request.Form, requestModel);
            GetUsersDataTableRequest request = new GetUsersDataTableRequest();
            _mapper.Map(requestModel, request);
            request.UserGuid = Guid.Parse(_viewHelper.GetUserId(User));

            var userList = await _authService.GetUserDataTableAsync(request, accessToken, cancellationToken);
            return new JsonResult(userList);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("partners/table")]
        public async Task<IActionResult> GetAllPartners(CancellationToken cancellationToken)
        {
            var accessToken = _viewHelper.GetAccessToken(User);
            DataTableRequestModel requestModel = new DataTableRequestModel();
            _mapper.Map(Request.Form, requestModel);
            GetOrganizationsDataTableRequest request = new GetOrganizationsDataTableRequest();
            _mapper.Map(requestModel, request);
            request.UserGuid = Guid.Parse(_viewHelper.GetUserId(User));

            var userList = await _partnerService.GetPartnerDataTableAsync(request, accessToken, cancellationToken);
            return new JsonResult(userList);
        }
    }
}
