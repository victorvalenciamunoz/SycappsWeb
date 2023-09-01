using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SycappsWeb.Server.Data;
using SycappsWeb.Server.ExtensionMethods;
using SycappsWeb.Server.Models.IntegrationModels;
using SycappsWeb.Server.Models.Un2Trek;
using SycappsWeb.Shared.Entities.Un2Trek;
using SycappsWeb.Shared.ExtensionMethods;
using SycappsWeb.Shared.Models.Un2Trek;
using System.Net.Mime;

namespace SycappsWeb.Server.Services;

public class TrekiService : ITrekiService
{    
    private readonly IUnitOfWork unitOfWork;
    private readonly IConfiguration config;
    private readonly UserManager<IdentityUser> userManager;

    public TrekiService(IUnitOfWork unitOfWork,
                        IConfiguration configuration,
                        UserManager<IdentityUser> userManager)
    {        
        this.unitOfWork = unitOfWork;
        this.config = configuration;
        this.userManager = userManager;
    }

    public Task Add(TrekiDto newTreki)
    {
        var trekiToInsert = newTreki.ToEntity();
        return unitOfWork.TrekiRepository.Add(trekiToInsert);
    }

    public async Task<bool> Modify(TrekiDto trekiToModify)
    {
        return await unitOfWork.TrekiRepository.Modify(trekiToModify.ToEntity());
    }

    public async Task<List<TrekiDto>> GetTrekisAround(double currentLatitude, double currentLongitude)
    {
        var result = new List<TrekiDto>();
        var threshold = config.GetValue<string>("Threshold")!;

        var trekis = await unitOfWork.TrekiRepository.GetTrekisAround(currentLatitude, currentLongitude, threshold);    

        foreach (var treki in trekis)
        {
            var puntoDto = treki.ToDto();
            result.Add(puntoDto);
        }

        return result;
    }

    public async Task<List<TrekiDto>> GetTrekiListByActivity(int activityId)
    {
        var result = new List<TrekiDto>();
        var trekis = await unitOfWork.TrekiRepository.GetTrekiListByActivity(activityId);

        foreach (var treki in trekis)
        {
            var puntoDto = treki.ToDto();
            result.Add(puntoDto);
        }

        return result;
    }

    public async Task<List<TrekiDto>> GetTrekiListNotInActivity(int activityId)
    {
        var result = new List<TrekiDto>();
        var trekis = await unitOfWork.TrekiRepository.GetTrekiListNotInActivity(activityId);

        foreach (var treki in trekis)
        {
            var puntoDto = treki.ToDto();
            result.Add(puntoDto);
        }

        return result;
    }

    public async Task<ServiceResultSingleElement<bool>> Capture(CaptureTrekiRequest capture, string userId)
    {
        ServiceResultSingleElement<bool> result = new ServiceResultSingleElement<bool>();

        var user = await userManager.FindByIdAsync(userId);

        var treki = await unitOfWork.TrekiRepository.GetTrekiByCoordinates(capture.TrekiLatitude, capture.TrekiLongitude);        

        var validationResult = await ValidateCaptureData(capture, treki, user);
        
        if (validationResult.HasErrors)
        {
            result.Errors.Add(validationResult.Errors[0]);
            return result;
        }

        await unitOfWork.TrekiRepository.CaptureTreki(treki, user!.Id, capture.ActivityId);
        
        result.Element = true;
        return result;
    }

    private async Task<ServiceResult> ValidateCaptureData(CaptureTrekiRequest captureData, Treki treki, IdentityUser user)
    {
        var result = new ServiceResult();
        if (user == null)
        {
            result.Errors.Add(StringConstants.UserNotFoundErrorCode);
            return result;
        }
        
        if (treki == null)
        {
            result.Errors.Add(StringConstants.TrekiNotFoundErrorCode);
            return result;
        }

        var activity = unitOfWork.ActivityRepository.GetById(captureData.ActivityId);
        if (activity == null)
        {
            result.Errors.Add(StringConstants.ActivityNotFoundErrorCode);
            return result;
        }

        var trekiListInActivity = await GetTrekiListByActivity(captureData.ActivityId);
        if (trekiListInActivity == null || !trekiListInActivity.Any())
        {
            result.Errors.Add(StringConstants.ActivityWithNoTrekisErrorCode);
            return result;
        }

        if (!trekiListInActivity.Any(c=> c.Id == treki.Id))
        {
            result.Errors.Add(StringConstants.TrekiNotFoundInActivityErrorCode);
            return result;
        }

        if (unitOfWork.TrekiRepository.IsTrekiAlreadyCaptured(treki.Id,user.Id, captureData.ActivityId))
        {
            result.Errors.Add(StringConstants.TrekiAlreadyCapturedErrorCode);
            return result;
        }

        Geolocation.Coordinate origin = new Geolocation.Coordinate(captureData.TrekiLatitude, captureData.TrekiLongitude);
        Geolocation.Coordinate destination = new Geolocation.Coordinate(captureData.CurrentLatitude, captureData.CurrentLongitude);
        double distance = Geolocation.GeoCalculator.GetDistance(origin, destination, decimalPlaces: 2, Geolocation.DistanceUnit.Meters);
        var threshold = Convert.ToDouble(config.GetValue<string>("Threshold")!);
        if (distance> threshold)
        {
            result.Errors.Add(StringConstants.InvalidDistanceErrorCode);
            return result;
        }

      

        return result;
    }
}
