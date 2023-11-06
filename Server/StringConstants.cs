using ErrorOr;

namespace SycappsWeb.Server;

public static class StringConstants
{
    public static readonly string ActivityNotFoundErrorCode = "A001";
    public static readonly string ActivityWithNoTrekisErrorCode = "A002";
                   
    public static readonly string TrekiNotFoundErrorCode = "T001";    
    public static readonly string TrekiNotFoundInActivityErrorCode = "T003";    
                   
    public static readonly string UserNotFoundErrorCode = "U001";
    public static readonly string InvalidCredentialsErrorCode = "U002";
                   
    public static readonly string RequiredFieldsMissing = "V001";
}
public static class SycappsError
{
    public static readonly Error InvalidDistance = Error.Validation(code: "T002", description: "Invalid distance to capture treki");
    public static readonly Error TrekiAlreadyCaptured = Error.Validation(code: "T004", description: "Treki already capture by user");
}
