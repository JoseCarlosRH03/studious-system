namespace BackendTemplateCore.DTOs.Shared;

public record struct StatusItemView(
    int Id,
    string Description,
    Item Status);