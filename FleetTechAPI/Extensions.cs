namespace FleetTechAPI;

public static class Extensions
{
   public static bool IsLocalDevelopment(this IWebHostEnvironment env) =>
      env.EnvironmentName.ToLower() == "local development";

   public static bool IsDevelopment(this IWebHostEnvironment env) =>
      env.EnvironmentName.ToLower().Contains("development");
   
   internal static void Tagged(string tag, params RouteHandlerBuilder[] routes)
   {
      foreach (var route in routes)
         route.WithTags(tag).Produces<Context.ErrorView>(500).Produces<Context.ErrorView>(400)
            .Produces<Context.ErrorView>(401).Produces<Context.ErrorView>(404);
   }

   internal static void Hidden(string tag, params RouteHandlerBuilder[] routes)
   {
      foreach (var route in routes)
         route.WithTags(tag).ExcludeFromDescription().Produces<Context.ErrorView>(500).Produces<Context.ErrorView>(400)
            .Produces<Context.ErrorView>(401).Produces<Context.ErrorView>(404);
   }
}
