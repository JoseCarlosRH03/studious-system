namespace FleetTechCore.DTOs.Views;

public record struct ContactView
    (
        int     Id,
        string  Name,
        string  Phone,
        string  Email
    );

