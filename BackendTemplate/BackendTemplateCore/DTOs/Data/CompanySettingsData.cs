namespace BackendTemplateCore.DTOs.Data;

public record struct CompanySettingsData(
    string TimeZone,
    string DatePattern,
    string TimePattern,
    int LateFeeProductId);