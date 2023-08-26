using BackendTemplateCore.DTOs.Shared;
using BackendTemplateCore.Enums;
using BackendTemplateCore.Models.Address;

namespace BackendTemplateCore.DTOs.Views;

public record struct CityView
(
    int Id,
    string Name,
    StateView? State,
    Item Status
)
{
    public static CityView From(City city) =>
        new(city.Id, city.Name, city.State is not null ? StateView.From(city.State) : null,
            Item.From((GenericStatus) city.Status));
}

public record struct StateView
(
    int Id,
    string Name,
    string Country,
    Item Status
)
{
    public static StateView From(State state) =>
        new(state.Id, state.Name, state.Country is not null? state.Country.Name : null, Item.From((GenericStatus) state.Status));
}

public record struct CountryView
(
    int Id,
    string Name,
    DateTime CreatedOn,
    StateView[]? States
) { public static CountryView From(Country country) =>
        new(country.Id, country.Name, country.CreatedOn, country.States is not null && country.States.Any()? country.States.Select(StateView.From).ToArray() : null); }