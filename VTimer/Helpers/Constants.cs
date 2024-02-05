using System.Collections;
using System.Collections.Generic;
using VTimer.Helpers;

// Weather data taken from https://github.com/Asvel/ffxiv-weather/blob/master/src/Weather.ts
// thank you to them for laying this out in an easy to find and understand way,
// and thank you to Rogueadyn"s SaintCoinach library who they credit with finding the weather formula.
namespace VTimer.Consts;
public enum Weathers {
  NA,
  ClearSkies,
  FairSkies,
  Wind,
  Rain,
  Showers,
  Thunder,
  Thunderstorms,
  Clouds,
  Fog,
  Gales,
  DustStorms,
  HeatWaves,
  Snow,
  Blizzards,
  UmbralWind,
  MoonDust,
  AstromagneticStorm,
  UmbralStatic,
  Gloom,

}

public enum Zones {
  //La Noscea
  LimsaLominsa, MiddleLaNoscea, LowerLaNoscea, EasternLaNoscea, WesternLaNoscea, UpperLaNoscea, OuterLaNoscea, TheMist,

  //The Black Shroud
  Gridania, CentralShroud, EastShroud, SouthShroud, NorthShroud, TheLavenderBeds,

  //Thanalan
  Uldah, WesternThanalan, CentralThanalan, EasternThanalan, SouthernThanalan, NorthernThanalan, TheGoblet,

  //ARR
  MorDhona, CoerthasCentralHighlands,

  //Heavensward
  Ishgard,  CoerthasWesternHighlands, Empyreum, TheSeaofClouds, AzysLla, Idyllshire, TheDravanianForelands, TheDravanianHinterlands, TheChurningMists, 
  
  //Stormblood
  RhalgrsReach, TheFringes, ThePeaks, TheLochs, Kugane, Shirogane, TheRubySea, Yanxia, TheAzimSteppe, EurekaAnemos, EurekaPagos, EurekaPyros, EurekaHydatos, TheCrystarium,

  //Shadowbringers
  Eulmore, Lakeland, Kholusia, AmhAraeng, IlMheg, TheRaktikaGreatwood, BozjanSouthernFront, Zadnor, TheTempest, TheDiadem,

  //Endwalker
  OldSharlayan, RadzatHan, Labyrinthos, Thavnair, Garlemald, MareLamentorum, Elpis, UltimaThule, UnnamedIsland,
}

public enum dayCycle {
  NA,
  Day,
  Night,
}

class WeatherList
{
  public static Dictionary<Zones, ArrayList> ByZone = new Dictionary<Zones, ArrayList> {
    {Zones.LimsaLominsa, new ArrayList() {Weathers.Clouds, 20, Weathers.ClearSkies, 50, Weathers.FairSkies, 80, Weathers.Fog, 90, Weathers.Rain} },
    {Zones.MiddleLaNoscea, new ArrayList() {Weathers.Clouds, 20, Weathers.ClearSkies, 50, Weathers.FairSkies, 70, Weathers.Wind, 80, Weathers.Fog, 90, Weathers.Rain} },
    {Zones.LowerLaNoscea, new ArrayList() {Weathers.Clouds, 20, Weathers.ClearSkies, 50, Weathers.FairSkies, 70, Weathers.Wind, 80, Weathers.Fog, 90, Weathers.Rain} },
    {Zones.EasternLaNoscea, new ArrayList() {Weathers.Fog, 5, Weathers.ClearSkies, 50, Weathers.FairSkies, 80, Weathers.Clouds, 90, Weathers.Rain, 95, Weathers.Showers} },
    {Zones.WesternLaNoscea, new ArrayList() {Weathers.Fog, 10, Weathers.ClearSkies, 40, Weathers.FairSkies, 60, Weathers.Clouds, 80, Weathers.Wind, 90, Weathers.Gales} },
    {Zones.UpperLaNoscea, new ArrayList() {Weathers.ClearSkies, 30, Weathers.FairSkies, 50, Weathers.Clouds, 70, Weathers.Fog, 80, Weathers.Thunder, 90, Weathers.Thunderstorms} },
    {Zones.OuterLaNoscea, new ArrayList() {Weathers.ClearSkies, 30, Weathers.FairSkies, 50, Weathers.Clouds, 70, Weathers.Fog, 85, Weathers.Rain} },
    {Zones.TheMist, new ArrayList() {Weathers.Clouds, 20, Weathers.ClearSkies, 50, Weathers.FairSkies, 70, Weathers.FairSkies, 80, Weathers.Fog, 90, Weathers.Rain} },
    {Zones.Gridania, new ArrayList() {Weathers.Rain, 5, Weathers.Rain, 20, Weathers.Fog, 30, Weathers.Clouds, 40, Weathers.FairSkies, 55, Weathers.ClearSkies, 85, Weathers.FairSkies} },
    {Zones.CentralShroud, new ArrayList() {Weathers.Thunder, 5, Weathers.Rain, 20, Weathers.Fog, 30, Weathers.Clouds, 40, Weathers.FairSkies, 55, Weathers.ClearSkies, 85, Weathers.FairSkies} },
    {Zones.EastShroud, new ArrayList() {Weathers.Thunder, 5, Weathers.Rain, 20, Weathers.Fog, 30, Weathers.Clouds, 40, Weathers.FairSkies, 55, Weathers.ClearSkies, 85, Weathers.FairSkies} },
    {Zones.SouthShroud, new ArrayList() {Weathers.Fog, 5, Weathers.Thunderstorms, 10, Weathers.Thunder, 25, Weathers.Fog, 30, Weathers.Clouds, 40, Weathers.FairSkies, 70, Weathers.ClearSkies} },
    {Zones.NorthShroud, new ArrayList() {Weathers.Fog, 5, Weathers.Showers, 10, Weathers.Rain, 25, Weathers.Fog, 30, Weathers.Clouds, 40, Weathers.FairSkies, 70, Weathers.ClearSkies} },
    {Zones.TheLavenderBeds, new ArrayList() {Weathers.Clouds, 5, Weathers.Rain, 20, Weathers.Fog, 30, Weathers.Clouds, 40, Weathers.FairSkies, 55, Weathers.ClearSkies, 85, Weathers.FairSkies} },
    {Zones.Uldah, new ArrayList() {Weathers.ClearSkies, 40, Weathers.FairSkies, 60, Weathers.Clouds, 85, Weathers.Fog, 95, Weathers.Rain} },
    {Zones.WesternThanalan, new ArrayList() {Weathers.ClearSkies, 40, Weathers.FairSkies, 60, Weathers.Clouds, 85, Weathers.Fog, 95, Weathers.Rain} },
    {Zones.CentralThanalan, new ArrayList() {Weathers.DustStorms, 15, Weathers.ClearSkies, 55, Weathers.FairSkies, 75, Weathers.Clouds, 85, Weathers.Fog, 95, Weathers.Rain} },
    {Zones.EasternThanalan, new ArrayList() {Weathers.ClearSkies, 40, Weathers.FairSkies, 60, Weathers.Clouds, 70, Weathers.Fog, 80, Weathers.Rain, 85, Weathers.Showers} },
    {Zones.SouthernThanalan, new ArrayList() {Weathers.HeatWaves, 20, Weathers.ClearSkies, 60, Weathers.FairSkies, 80, Weathers.Clouds, 90, Weathers.Fog} },
    {Zones.NorthernThanalan, new ArrayList() {Weathers.ClearSkies, 5, Weathers.FairSkies, 20, Weathers.Clouds, 50, Weathers.Fog} },
    {Zones.TheGoblet, new ArrayList() {Weathers.ClearSkies, 40, Weathers.FairSkies, 60, Weathers.Clouds, 85, Weathers.Fog, 95, Weathers.Rain} },
    {Zones.Ishgard, new ArrayList() {Weathers.Snow, 60, Weathers.FairSkies, 70, Weathers.ClearSkies, 75, Weathers.Clouds, 90, Weathers.Fog} },
    {Zones.CoerthasCentralHighlands, new ArrayList() {Weathers.Blizzards, 20, Weathers.Snow, 60, Weathers.FairSkies, 70, Weathers.ClearSkies, 75, Weathers.Clouds, 90, Weathers.Fog} },
    {Zones.CoerthasWesternHighlands, new ArrayList() {Weathers.Blizzards, 20, Weathers.Snow, 60, Weathers.FairSkies, 70, Weathers.ClearSkies, 75, Weathers.Clouds, 90, Weathers.Fog} },
    {Zones.Empyreum, new ArrayList() {Weathers.Snow, 5, Weathers.FairSkies, 25, Weathers.ClearSkies, 65, Weathers.Clouds, 80, Weathers.Fog} },
    {Zones.TheSeaofClouds, new ArrayList() {Weathers.ClearSkies, 30, Weathers.FairSkies, 60, Weathers.Clouds, 70, Weathers.Fog, 80, Weathers.Wind, 90, Weathers.UmbralWind} },
    {Zones.AzysLla, new ArrayList() {Weathers.FairSkies, 35, Weathers.Clouds, 70, Weathers.Thunder} },
    {Zones.TheDiadem, new ArrayList() {Weathers.FairSkies, 30, Weathers.Fog, 60, Weathers.Wind, 90, Weathers.UmbralWind} },
    {Zones.Idyllshire, new ArrayList() {Weathers.Clouds, 10, Weathers.Fog, 20, Weathers.Rain, 30, Weathers.Showers, 40, Weathers.ClearSkies, 70, Weathers.FairSkies} },
    {Zones.TheDravanianForelands, new ArrayList() {Weathers.Clouds, 10, Weathers.Fog, 20, Weathers.Thunder, 30, Weathers.DustStorms, 40, Weathers.ClearSkies, 70, Weathers.FairSkies} },
    {Zones.TheDravanianHinterlands, new ArrayList() {Weathers.Clouds, 10, Weathers.Fog, 20, Weathers.Rain, 30, Weathers.Showers, 40, Weathers.ClearSkies, 70, Weathers.FairSkies} },
    {Zones.TheChurningMists, new ArrayList() {Weathers.Clouds, 10, Weathers.Gales, 20, Weathers.UmbralStatic, 40, Weathers.ClearSkies, 70, Weathers.FairSkies} },
    {Zones.MorDhona, new ArrayList() {Weathers.Clouds, 15, Weathers.Fog, 30, Weathers.Gloom, 60, Weathers.ClearSkies, 75, Weathers.FairSkies} },
    {Zones.RhalgrsReach, new ArrayList() {Weathers.ClearSkies, 15, Weathers.FairSkies, 60, Weathers.Clouds, 80, Weathers.Fog, 90, Weathers.Thunder} },
    {Zones.TheFringes, new ArrayList() {Weathers.ClearSkies, 15, Weathers.FairSkies, 60, Weathers.Clouds, 80, Weathers.Fog, 90, Weathers.Thunder} },
    {Zones.ThePeaks, new ArrayList() {Weathers.ClearSkies, 10, Weathers.FairSkies, 60, Weathers.Clouds, 75, Weathers.Fog, 85, Weathers.Wind, 95, Weathers.DustStorms} },
    {Zones.TheLochs, new ArrayList() {Weathers.ClearSkies, 20, Weathers.FairSkies, 60, Weathers.Clouds, 80, Weathers.Fog, 90, Weathers.Thunderstorms} },
    {Zones.Kugane, new ArrayList() {Weathers.Rain, 10, Weathers.Fog, 20, Weathers.Clouds, 40, Weathers.FairSkies, 80, Weathers.ClearSkies} },
    {Zones.Shirogane, new ArrayList() {Weathers.Rain, 10, Weathers.Fog, 20, Weathers.Clouds, 40, Weathers.FairSkies, 80, Weathers.ClearSkies} },
    {Zones.TheRubySea, new ArrayList() {Weathers.Thunder, 10, Weathers.Wind, 20, Weathers.Clouds, 35, Weathers.FairSkies, 75, Weathers.ClearSkies} },
    {Zones.Yanxia, new ArrayList() {Weathers.Showers, 5, Weathers.Rain, 15, Weathers.Fog, 25, Weathers.Clouds, 40, Weathers.FairSkies, 80, Weathers.ClearSkies} },
    {Zones.TheAzimSteppe, new ArrayList() {Weathers.Gales, 5, Weathers.Wind, 10, Weathers.Rain, 17, Weathers.Fog, 25, Weathers.Clouds, 35, Weathers.FairSkies, 75, Weathers.ClearSkies} },
    {Zones.EurekaAnemos, new ArrayList() {Weathers.FairSkies, 30, Weathers.Gales, 60, Weathers.Showers, 90, Weathers.Snow} },
    {Zones.EurekaPagos, new ArrayList() {Weathers.ClearSkies, 10, Weathers.Fog, 28, Weathers.HeatWaves, 46, Weathers.Snow, 64, Weathers.Thunder, 82, Weathers.Blizzards} },
    {Zones.EurekaPyros, new ArrayList() {Weathers.FairSkies, 10, Weathers.HeatWaves, 28, Weathers.Thunder, 46, Weathers.Blizzards, 64, Weathers.UmbralWind, 82, Weathers.Snow} },
    {Zones.EurekaHydatos, new ArrayList() {Weathers.FairSkies, 12, Weathers.Showers, 34, Weathers.Gloom, 56, Weathers.Thunderstorms, 78, Weathers.Snow} },
    {Zones.BozjanSouthernFront, new ArrayList() {Weathers.FairSkies, 52, Weathers.Rain, 64, Weathers.Wind, 76, Weathers.Thunder, 88, Weathers.DustStorms} },
    {Zones.Zadnor, new ArrayList() {Weathers.FairSkies, 60, Weathers.Rain, 70, Weathers.Wind, 80, Weathers.Thunder, 90, Weathers.Snow} },
    {Zones.TheCrystarium, new ArrayList() {Weathers.ClearSkies, 20, Weathers.FairSkies, 60, Weathers.Clouds, 75, Weathers.Fog, 85, Weathers.Rain, 95, Weathers.Thunderstorms} },
    {Zones.Eulmore, new ArrayList() {Weathers.Gales, 10, Weathers.Rain, 20, Weathers.Fog, 30, Weathers.Clouds, 45, Weathers.FairSkies, 85, Weathers.ClearSkies} },
    {Zones.Lakeland, new ArrayList() {Weathers.ClearSkies, 20, Weathers.FairSkies, 60, Weathers.Clouds, 75, Weathers.Fog, 85, Weathers.Rain, 95, Weathers.Thunderstorms} },
    {Zones.Kholusia, new ArrayList() {Weathers.Gales, 10, Weathers.Rain, 20, Weathers.Fog, 30, Weathers.Clouds, 45, Weathers.FairSkies, 85, Weathers.ClearSkies} },
    {Zones.AmhAraeng, new ArrayList() {Weathers.FairSkies, 45, Weathers.Clouds, 60, Weathers.DustStorms, 70, Weathers.HeatWaves, 80, Weathers.ClearSkies} },
    {Zones.IlMheg, new ArrayList() {Weathers.Rain, 10, Weathers.Fog, 20, Weathers.Clouds, 35, Weathers.Thunderstorms, 45, Weathers.ClearSkies, 60, Weathers.FairSkies} },
    {Zones.TheRaktikaGreatwood, new ArrayList() {Weathers.Fog, 10, Weathers.Rain, 20, Weathers.UmbralWind, 30, Weathers.ClearSkies, 45, Weathers.FairSkies, 85, Weathers.Clouds} },
    {Zones.TheTempest, new ArrayList() {Weathers.Clouds, 20, Weathers.FairSkies, 80, Weathers.ClearSkies} },
    {Zones.OldSharlayan, new ArrayList() {Weathers.ClearSkies, 10, Weathers.FairSkies, 50, Weathers.Clouds, 70, Weathers.Fog, 85, Weathers.Snow} },
    {Zones.RadzatHan, new ArrayList() {Weathers.Fog, 10, Weathers.Rain, 25, Weathers.ClearSkies, 40, Weathers.FairSkies, 80, Weathers.Clouds} },
    {Zones.Labyrinthos, new ArrayList() {Weathers.ClearSkies, 15, Weathers.FairSkies, 60, Weathers.Clouds, 85, Weathers.Rain} },
    {Zones.Thavnair, new ArrayList() {Weathers.Fog, 10, Weathers.Rain, 20, Weathers.Showers, 25, Weathers.ClearSkies, 40, Weathers.FairSkies, 80, Weathers.Clouds} },
    {Zones.Garlemald, new ArrayList() {Weathers.Snow, 45, Weathers.Thunder, 50, Weathers.Rain, 55, Weathers.Fog, 60, Weathers.Clouds, 85, Weathers.FairSkies, 95, Weathers.ClearSkies} },
    {Zones.MareLamentorum, new ArrayList() {Weathers.UmbralWind, 15, Weathers.MoonDust, 30, Weathers.FairSkies} },
    {Zones.Elpis, new ArrayList() {Weathers.Clouds, 25, Weathers.UmbralWind, 40, Weathers.FairSkies, 85, Weathers.ClearSkies} },
    {Zones.UltimaThule, new ArrayList() {Weathers.AstromagneticStorm, 15, Weathers.FairSkies, 85, Weathers.UmbralWind} },
    {Zones.UnnamedIsland, new ArrayList() {Weathers.ClearSkies, 25, Weathers.FairSkies, 70, Weathers.Clouds, 80, Weathers.Rain, 90, Weathers.Fog, 95, Weathers.Showers} },
  };
/*
  public Dictionary<string, ArrayList> byZone = new Dictionary<string, ArrayList> {
    {"Limsa Lominsa", new ArrayList() {weathers.Clouds, 20, weathers.ClearSkies, 50, weathers.FairSkies, 80, weathers.Fog, 90, weathers.Rain} },
  };
  */
}

class Presets {
  public static Dictionary<string, Conditions> Timers= new Dictionary<string, Conditions> {
    {"Pazuzu", new Conditions(Zones.EurekaAnemos, new List<Weathers>{Weathers.Gales}, dayCycle.Night, 0) },
    {"Crab", new Conditions(Zones.EurekaPagos, new List<Weathers>{Weathers.Fog}, dayCycle.NA, 0) },
    {"Cassie", new Conditions(Zones.EurekaPagos, new List<Weathers>{Weathers.Blizzards}, dayCycle.NA, 0) },
    {"Skoll", new Conditions(Zones.EurekaPyros, new List<Weathers>{Weathers.Blizzards}, dayCycle.NA, 0) },
    {"Penny", new Conditions(Zones.EurekaPyros, new List<Weathers>{Weathers.HeatWaves}, dayCycle.NA, 0) },
    {"Luigi", new Conditions(Zones.EurekaPagos, new List<Weathers>{}, dayCycle.Night, 0) }
  };
}

