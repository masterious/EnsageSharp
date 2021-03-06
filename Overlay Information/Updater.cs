using System;
using System.Linq;
using Ensage;
using Ensage.Common.Extensions;
using Ensage.Common.Objects;
using Ensage.Common.Objects.UtilityObjects;

namespace OverlayInformation
{
    internal abstract class Updater
    {
        public abstract class HeroList
        {
            private static readonly Sleeper AbilityUpdate = new Sleeper();
            //private static readonly Sleeper ItemUpdate = new Sleeper();
            private static readonly Sleeper HeroUpdate = new Sleeper();

            public static void Update(EventArgs args)
            {
                if (!Checker.IsActive()) return;
                if (!HeroUpdate.Sleeping)
                {
                    HeroUpdate.Sleep(2000);
                    if (Members.Heroes.Count < 10)
                    {
                        Members.Heroes = Heroes.All.Where(x => x != null && x.IsValid && !x.IsIllusion).ToList();
                        Members.AllyHeroes = Members.Heroes.Where(x => x.Team == Members.MyHero.Team).ToList();
                        Members.EnemyHeroes =
                            Members.Heroes.Where(x => x.Team == Members.MyHero.GetEnemyTeam()).ToList();
                        //Printer.Print("STATUS:[all] " + Members.Heroes.Count+ " [enemy] " + Members.EnemyHeroes.Count + " [ally] " + Members.AllyHeroes.Count);
                        if (!Members.Apparition &&
                            Members.EnemyHeroes.Any(x => x.ClassID == ClassID.CDOTA_Unit_Hero_AncientApparition))
                        {
                            Members.Apparition = true;
                        }
                        if (Members.PAisHere == null &&
                            Members.EnemyHeroes.Any(x => x.ClassID == ClassID.CDOTA_Unit_Hero_PhantomAssassin))
                        {
                            Members.PAisHere = Members.EnemyHeroes.FirstOrDefault(x => x.ClassID == ClassID.CDOTA_Unit_Hero_PhantomAssassin);
                        }
                        if (!Members.BaraIsHere &&
                            Members.EnemyHeroes.Any(x => x.ClassID == ClassID.CDOTA_Unit_Hero_SpiritBreaker))
                        {
                            Members.BaraIsHere = true;
                        }
                        if (Members.Mirana == null &&
                            Members.EnemyHeroes.Any(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Mirana))
                        {
                            Members.Mirana = Members.EnemyHeroes.FirstOrDefault(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Mirana);
                        }
                        if (Members.Windrunner == null &&
                            Members.EnemyHeroes.Any(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Windrunner))
                        {
                            Members.Windrunner = Members.EnemyHeroes.FirstOrDefault(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Windrunner);
                        }
                        if (Members.Invoker == null &&
                            Members.EnemyHeroes.Any(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Invoker))
                        {
                            Members.Invoker = Members.EnemyHeroes.FirstOrDefault(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Invoker);
                        }
                        if (Members.Kunkka == null &&
                            Members.EnemyHeroes.Any(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Kunkka))
                        {
                            Members.Kunkka = Members.EnemyHeroes.FirstOrDefault(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Kunkka);
                        }
                        if (Members.Lina == null &&
                            Members.EnemyHeroes.Any(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Lina))
                        {
                            Members.Lina = Members.EnemyHeroes.FirstOrDefault(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Lina);
                        }
                        if (Members.Leshrac == null &&
                            Members.EnemyHeroes.Any(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Leshrac))
                        {
                            Members.Leshrac = Members.EnemyHeroes.FirstOrDefault(x => x.ClassID == ClassID.CDOTA_Unit_Hero_Leshrac);
                        }
                    }
                }
                
                if (!AbilityUpdate.Sleeping)
                {
                    AbilityUpdate.Sleep(1000);
                    foreach (var hero in /*Members.Heroes */Manager.HeroManager.GetViableHeroes())
                    {
                        /*if ((hero.ClassID==ClassID.CDOTA_Unit_Hero_DoomBringer || hero.ClassID==ClassID.CDOTA_Unit_Hero_Rubick) && !hero.IsVisible)
                            continue;*/
                        try
                        {
                            if (!Members.AbilityDictionary.ContainsKey(hero.StoredName()))
                                Members.AbilityDictionary.Add(hero.StoredName(),
                                    hero.Spellbook.Spells.Where(
                                        x =>
                                            x!=null && x.IsValid && x.AbilityType != AbilityType.Attribute && x.AbilityType != AbilityType.Hidden &&
                                            x.AbilitySlot.ToString() != "-1")
                                        .ToList());
                            if (
                                !Members.AbilityDictionary.ContainsValue(
                                    hero.Spellbook.Spells.Where(x => x.AbilitySlot.ToString() != "-1").ToList()))
                            {
                                Members.AbilityDictionary.Remove(hero.StoredName());
                                Members.AbilityDictionary.Add(hero.StoredName(), hero.Spellbook.Spells.Where(
                                    x =>
                                        x!=null && x.IsValid && x.AbilityType != AbilityType.Attribute && x.AbilityType != AbilityType.Hidden &&
                                            x.AbilitySlot.ToString() != "-1")
                                        .ToList());

                            }
                            if (!Members.ItemDictionary.ContainsValue(
                                    hero.Inventory.Items.Where(x => x != null && x.IsValid).ToList()))
                            {
                                Members.ItemDictionary.Remove(hero.StoredName());
                                Members.ItemDictionary.Add(hero.StoredName(),
                                    hero.Inventory.Items.Where(x => x != null && x.IsValid).ToList());
                            }
                            if (Members.Menu.Item("itempanel.Stash.Enable").GetValue<bool>() &&
                                !Members.StashItemDictionary.ContainsValue(
                                    hero.Inventory.StashItems.Where(x => x != null && x.IsValid).ToList()))
                            {
                                Members.StashItemDictionary.Remove(hero.StoredName());
                                Members.StashItemDictionary.Add(hero.StoredName(),
                                    hero.Inventory.StashItems.Where(x => x != null && x.IsValid).ToList());
                            }

                        }
                        catch (Exception)
                        {
                            Printer.Print("[UPDATER.ITEMS/ABILITY: ] " + hero.StoredName());
                        }
                        
                    }
                }
            }
        }

        public abstract class PlayerList
        {
            private static readonly Sleeper Sleeper = new Sleeper();

            public static void Update(EventArgs args)
            {
                if (!Checker.IsActive()) return;
                if (Sleeper.Sleeping) return;
                Sleeper.Sleep(2000);
                if (Members.Players.Count(x => x != null && x.IsValid && x.Hero.IsValid) < 10)
                {
                    Members.Players = Players.All.Where(x => x != null && x.IsValid && x.Hero!=null && x.Hero.IsValid).ToList();
                    Members.AllyPlayers = Members.Players.Where(x => x.Team == Members.MyHero.Team).ToList();
                    Members.EnemyPlayers = Members.Players.Where(x => x.Team == Members.MyHero.GetEnemyTeam()).ToList();
                }
            }

        }

        public abstract class BaseList
        {
            private static readonly Sleeper Sleeper = new Sleeper();

            public static void Update(EventArgs args)
            {
                if (!Checker.IsActive()) return;
                if (Sleeper.Sleeping) return;
                Sleeper.Sleep(100);
                Members.BaseList =
                    ObjectManager.GetEntities<Unit>()
                        .Where(x => x.ClassID == ClassID.CDOTA_BaseNPC && x.Team == Members.MyHero.GetEnemyTeam())
                        .ToList();
            }
        }
    }
}