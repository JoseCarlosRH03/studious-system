﻿using BackendTemplateCore.DTOs.Shared;
using BackendTemplateCore.Enums;
using BackendTemplateCore.Models.Company;

using BackendTemplateCore.Models.Company;

namespace BackendTemplateCore.DTOs.Views;

public record struct BranchView(
    int Id,
    string Code,
    CityView City,
    Item BranchType,
    string Locality,
    string Address,
    string Phone,
    Item Status
)
{
    public static BranchView From (Branch cam) => new(
        cam.Id,
        cam.Code,
        CityView.From(cam.City),
        new Item(cam.BranchTypeId, cam.BranchType.Name),
        cam.Locality,
        cam.Address,
        cam.Phone,
        Item.From((GenericStatus) cam.Status)
    );
}