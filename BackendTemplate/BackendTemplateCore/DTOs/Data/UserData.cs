namespace BackendTemplateCore.DTOs.Data;

public record struct UserData (
    string  FirstName,
    string  LastName,
    string  Username,
    string  Email,
    string? Phone,
    string  Password,
    int?    BranchId
);