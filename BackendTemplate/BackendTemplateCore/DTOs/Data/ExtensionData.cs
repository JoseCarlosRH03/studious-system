namespace BackendTemplateCore.DTOs.Data;

public record struct ExtensionData(
    string Name,
    List<PropertyData> Properties
    );
    
public record struct PropertyData(
    string Name,
    string Value
);