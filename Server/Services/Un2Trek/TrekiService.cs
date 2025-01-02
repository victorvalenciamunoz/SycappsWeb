using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SycappsWeb.Server.Data;
using SycappsWeb.Server.ExtensionMethods;
using SycappsWeb.Server.Models.IntegrationModels;
using SycappsWeb.Server.Models.Un2Trek;
using SycappsWeb.Shared.Entities;
using SycappsWeb.Shared.Entities.Un2Trek;
using SycappsWeb.Shared.ExtensionMethods;
using SycappsWeb.Shared.Models.Un2Trek;
using System.Net.Mime;

namespace SycappsWeb.Server.Services;

public class TrekiService : ITrekiService
{    
    private readonly IUnitOfWork unitOfWork;
    private readonly IConfiguration config;
    private readonly UserManager<ApplicationUser> userManager;

    public TrekiService(IUnitOfWork unitOfWork,
                        IConfiguration configuration,
                        UserManager<ApplicationUser> userManager)
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

    public async Task<ErrorOr<Updated>> Modify(TrekiDto trekiToModify)
    {
        ServiceResultSingleElement<bool> result = new();
        var treki = await unitOfWork.TrekiRepository.GetById(trekiToModify.Id);
        if (treki == null)
        {
            return Error.NotFound(StringConstants.TrekiNotFoundErrorCode, description: "Treki not found");
        }
        result.Element =  await unitOfWork.TrekiRepository.Modify(trekiToModify.ToEntity());

        return Result.Updated;
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

    public async Task<ErrorOr<Deleted>> Delete(int id)
    {
        ServiceResultSingleElement<bool> result = new();
        var treki = await unitOfWork.TrekiRepository.GetById(id);
        if (treki == null)
        {
            return Error.NotFound(StringConstants.TrekiNotFoundErrorCode, description: "Treki not found");            
        }
        
        treki.Activo = false;
        result.Element =  await unitOfWork.TrekiRepository.Modify(treki);
        
        return Result.Deleted;
    }

    public async Task<ErrorOr<Success>> Capture(CaptureTrekiRequest capture, string userId)
    {
        ServiceResultSingleElement<bool> result = new ServiceResultSingleElement<bool>();

        var user = await userManager.FindByIdAsync(userId);

        var treki = await unitOfWork.TrekiRepository.GetTrekiByCoordinates(capture.TrekiLatitude, capture.TrekiLongitude);        

        var validationResult = await ValidateCaptureData(capture, treki, user);
        
        if (validationResult.IsError)
        {
            return validationResult;
        }

        await unitOfWork.TrekiRepository.CaptureTreki(treki, user!.Id, capture.ActivityId);

        return Result.Success;
    }

    private async Task<ErrorOr<Success>> ValidateCaptureData(CaptureTrekiRequest captureData, Treki treki, IdentityUser user)
    {        
        if (user == null)
        {
            return Error.NotFound(StringConstants.UserNotFoundErrorCode, description: "User not found");
        }
        
        if (treki == null)
        {
            return Error.NotFound(StringConstants.TrekiNotFoundErrorCode, description: "Treki not found");                        
        }

        var activity = await unitOfWork.ActivityRepository.GetById(captureData.ActivityId);
        if (activity == null)
        {
            return Error.NotFound(StringConstants.ActivityNotFoundErrorCode, description: "Activity not found");            
        }

        var trekiListInActivity = await GetTrekiListByActivity(captureData.ActivityId);
        if (trekiListInActivity == null || !trekiListInActivity.Any())
        {
            return Error.NotFound(StringConstants.ActivityWithNoTrekisErrorCode, description: "Activity with no trekis");            
        }

        if (!trekiListInActivity.Any(c=> c.Id == treki.Id))
        {
            return Error.NotFound(StringConstants.TrekiNotFoundInActivityErrorCode, description: "Treki not found in activity");
        }

        if (unitOfWork.TrekiRepository.IsTrekiAlreadyCaptured(treki.Id,user.Id, captureData.ActivityId))
        {
            return SycappsError.TrekiAlreadyCaptured;            
        }

        Geolocation.Coordinate origin = new Geolocation.Coordinate(captureData.TrekiLatitude, captureData.TrekiLongitude);
        Geolocation.Coordinate destination = new Geolocation.Coordinate(captureData.CurrentLatitude, captureData.CurrentLongitude);
        double distance = Geolocation.GeoCalculator.GetDistance(origin, destination, decimalPlaces: 2, Geolocation.DistanceUnit.Meters);
        var threshold = Convert.ToDouble(config.GetValue<string>("Threshold")!);
        if (distance> threshold)
        {
            return SycappsError.InvalidDistance;
        }



        return Result.Success;
    }
}
