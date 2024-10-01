using System.Collections.Generic;
using VTimer.Helpers;

// Weather data taken from https://github.com/Asvel/ffxiv-weather/blob/master/src/Weather.ts
// thank you to them for laying this out in an easy to find and understand way,
// and thank you to Rogueadyn"s SaintCoinach library who they credit with finding the weather formula.
namespace VTimer.Consts;

internal class Numbers {
  internal static readonly int Zero = 0;
  internal static Val<int> ZeroVal = new(0);
  internal static readonly int MaxWindowsToPreload = 100;
  internal static readonly long MaxOutlook = 14 * 24*60*60; //2 weeks
}

static class Presets {
  public static readonly Dictionary<string, Conditions> Conditions = new Dictionary<string, Conditions> {
    // Eureka
    {Names.Pazuzu, new Conditions(Zones.EurekaAnemos, new List<Weathers>{Weathers.Gales}, dayCycle.Night) },
    {Names.Crab, new Conditions(Zones.EurekaPagos, new List<Weathers>{Weathers.Fog}, dayCycle.NA) },
    {Names.Cassie, new Conditions(Zones.EurekaPagos, new List<Weathers>{Weathers.Blizzards}, dayCycle.NA) },
    {Names.Skoll, new Conditions(Zones.EurekaPyros, new List<Weathers>{Weathers.Blizzards}, dayCycle.NA) },
    {Names.Penny, new Conditions(Zones.EurekaPyros, new List<Weathers>{Weathers.HeatWaves}, dayCycle.NA) },
    {Names.Luigi, new Conditions(Zones.EurekaPagos, new List<Weathers>{}, dayCycle.Night) },

    //Farms
    // Cold box has 3 different weathers, but they're all for different mobs
    //  TODO consider seperating cold box into different toggles to add weathers to allow people to customize to their perfered mobs, and willingness
    //  to move around the zone during a farm
    {Names.ColdBox, new Conditions(Zones.EurekaPagos, new List<Weathers>{Weathers.Blizzards, Weathers.FairSkies, Weathers.Thunder}, dayCycle.NA) },
    {Names.HeatBox, new Conditions(Zones.EurekaPyros, new List<Weathers>{Weathers.UmbralWind}, dayCycle.NA) },
    {Names.Preparation, new Conditions(Zones.BozjanSouthernFront, new List<Weathers>{Weathers.Thunder}, dayCycle.NA) },
    {Names.Care, new Conditions(Zones.BozjanSouthernFront, new List<Weathers>{Weathers.Wind, Weathers.DustStorms}, dayCycle.NA) },
    {Names.Support, new Conditions(Zones.BozjanSouthernFront, new List<Weathers>{Weathers.Wind}, dayCycle.NA) },
    {Names.History, new Conditions(Zones.BozjanSouthernFront, new List<Weathers>{Weathers.Wind}, dayCycle.NA) },
    //History is also available in Zadnor during Snow, but that is a headache, and most of the rest are in BSF anyways, and snow doesnt overlap with Artistry, so :shrug:
    {Names.Artistry, new Conditions(Zones.Zadnor, new List<Weathers>{Weathers.Thunder, Weathers.Rain}, dayCycle.NA) },
  };
}

//tbh this should probably be an enum :\
static class Names{
  internal static readonly string Pazuzu = "Pazuzu";
  internal static readonly string Crab = "Crab";
  internal static readonly string Cassie = "Cassie";
  internal static readonly string Skoll = "Skoll";
  internal static readonly string Penny = "Penny";
  internal static readonly string Luigi = "Luigi";

  internal static readonly string ColdBox = "Cold Box";
  internal static readonly string HeatBox = "Heat Box";
  internal static readonly string Preparation = "Preparation";
  internal static readonly string Care = "Care";
  internal static readonly string Support = "Support";
  internal static readonly string History = "History";
  internal static readonly string Artistry = "Artistry";

}

class Groups {
  internal static readonly List<string> EurekaNMs = new List<string> {Names.Pazuzu, Names.Crab, Names.Cassie, Names.Skoll, Names.Penny, Names.Luigi};
  internal static readonly List<string> Farms = new List<string> {Names.Preparation, Names.Care, Names.Support, Names.History, Names.Artistry};
}

class Colors {
  internal static readonly System.Numerics.Vector4 up = new(0.0f, 1.0f, 0.0f, 1.0f);
  internal static readonly System.Numerics.Vector4 upSoon = new(1.0f, 1.0f, 0.0f, 1.0f);
  internal static readonly System.Numerics.Vector4 upEventually = new(1.0f, 1.0f, 1.0f, 1.0f);
  internal static readonly System.Numerics.Vector4 error = new(1.0f, 0.0f, 0.0f, 1.0f);
}

