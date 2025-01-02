using System.Collections.Generic;
using VTimer.Helpers;

// Weather data taken from https://github.com/Asvel/ffxiv-weather/blob/master/src/Weather.ts
// thank you to them for laying this out in an easy to find and understand way,
// and thank you to Rogueadyn"s SaintCoinach library who they credit with finding the weather formula.
namespace VTimer.Consts;

internal class Numbers {

  internal static long VERMINION_OFFSET = 486000;
  internal static long minute = 60;
  internal static long hour = 60 * 60;
  internal static long day = hour * 24;
  internal static long week = day * 7;
  internal static readonly int Zero = 0;
  internal static Val<int> ZeroVal = new(0);
  internal static readonly int MaxWindowsToPreload = 100;
  internal static readonly long MaxOutlook = 2 * week;
}

static class Presets {
  public static readonly Dictionary<string, List<Condition>> Conditions = new Dictionary<string, List<Condition>> {
    // Eureka
    {Names.Pazuzu, new List<Condition>{new Weather_Condition(Zones.EurekaAnemos, new List<Weathers>{Weathers.Gales}), new DayCycleCondition(dayCycle.Night) }},
    {Names.Crab, new List<Condition>{new Weather_Condition(Zones.EurekaPagos, new List<Weathers>{Weathers.Fog}) }},
    {Names.Cassie, new List<Condition>{new Weather_Condition(Zones.EurekaPagos, new List<Weathers>{Weathers.Blizzards}) }},
    {Names.Skoll, new List<Condition>{new Weather_Condition(Zones.EurekaPyros, new List<Weathers>{Weathers.Blizzards}) }},
    {Names.Penny, new List<Condition>{new Weather_Condition(Zones.EurekaPyros, new List<Weathers>{Weathers.HeatWaves}) }},
    {Names.Luigi, new List<Condition>{new Weather_Condition(Zones.EurekaPagos, new List<Weathers>{}), new DayCycleCondition(dayCycle.Night)}},

    //Farms
    // Cold box has 3 different weathers, but they're all for different mobs
    //  TODO consider seperating cold box into different toggles to add weathers to allow people to customize to their perfered mobs, and willingness
    //  to move around the zone during a farm
    {Names.ColdBox, new List<Condition>{new Weather_Condition(Zones.EurekaPagos, new List<Weathers>{Weathers.Blizzards, Weathers.FairSkies, Weathers.Thunder}) }},
    {Names.HeatBox, new List<Condition>{new Weather_Condition(Zones.EurekaPyros, new List<Weathers>{Weathers.UmbralWind}) }},
    {Names.Preparation, new List<Condition>{new Weather_Condition(Zones.BozjanSouthernFront, new List<Weathers>{Weathers.Thunder}) }},
    {Names.Care, new List<Condition>{new Weather_Condition(Zones.BozjanSouthernFront, new List<Weathers>{Weathers.Wind, Weathers.DustStorms}) }},
    {Names.Support, new List<Condition>{new Weather_Condition(Zones.BozjanSouthernFront, new List<Weathers>{Weathers.Wind}) }},
    {Names.History, new List<Condition>{new Weather_Condition(Zones.BozjanSouthernFront, new List<Weathers>{Weathers.Wind}) }},
    //History is also available in Zadnor during Snow, but that is a headache, and most of the rest are in BSF anyways, and snow doesnt overlap with Artistry, so :shrug:
    {Names.Artistry, new List<Condition>{new Weather_Condition(Zones.Zadnor, new List<Weathers>{Weathers.Thunder, Weathers.Rain}) }},

    //Realtime
    {Names.Verminion, new List<Condition>{new RealTimeConditions(Numbers.day * 6, Numbers.VERMINION_OFFSET, Numbers.day * 3)}},
    {Names.Boat, new List<Condition>{new RealTimeConditions(Numbers.hour * 2, 0, Numbers.minute * 15)}},
    {Names.OpenTournament, new List<Condition>{new RealTimeConditions(Numbers.hour * 2, Numbers.hour, Numbers.minute * 30)}},
    {Names.BiweeklyTournament, new List<Condition>{new RealTimeConditions(Numbers.day * 14, 0, Numbers.day * 7)}},
    {Names.FashionReport, new List<Condition>{new RealTimeConditions(Numbers.day * 7, Numbers.day * 3, Numbers.day * 4)}},
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
  internal static readonly string Verminion = "Verminion";
  internal static readonly string Boat = "Ocean Fishing Boat";
  internal static readonly string OpenTournament = "Triple Triad Open Tournament";
  internal static readonly string BiweeklyTournament = "Biweekly Triple Triad Tournament";
  internal static readonly string FashionReport = "Fashion Report";

}

class Groups {
  internal static readonly List<string> EurekaNMs = new List<string> {Names.Pazuzu, Names.Crab, Names.Cassie, Names.Skoll, Names.Penny, Names.Luigi};
  internal static readonly List<string> Farms = new List<string> {Names.Preparation, Names.Care, Names.Support, Names.History, Names.Artistry};
  internal static readonly List<string> realTime = new List<string> {Names. Verminion, Names.Boat, Names.OpenTournament, Names.BiweeklyTournament, Names.FashionReport};
}

class Colors {
  internal static readonly System.Numerics.Vector4 up = new(0.0f, 1.0f, 0.0f, 1.0f);
  internal static readonly System.Numerics.Vector4 upSoon = new(1.0f, 1.0f, 0.0f, 1.0f);
  internal static readonly System.Numerics.Vector4 upEventually = new(1.0f, 1.0f, 1.0f, 1.0f);
  internal static readonly System.Numerics.Vector4 error = new(1.0f, 0.0f, 0.0f, 1.0f);
}

