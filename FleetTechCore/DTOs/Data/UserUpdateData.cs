namespace FleetTechCore.DTOs.Data;

public record struct UserUpdateData(
    string  FirstName,
    string  LastName,
    string  Email,
    string? Phone,
    byte[]  ProfilePicture
    );
    
public record struct UserUpdatePictureData(
    string  ProfilePicture,
    string  MimeType
    );
    