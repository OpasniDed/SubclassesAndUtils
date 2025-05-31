using Exiled.API.Interfaces;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subclasses
{

   
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public RoleNames Names { get; set; } = new RoleNames();
        public List<Subclass> Subclasses { get; set; } = new List<Subclass>()
        {
            new Subclass()
            {
              Name = "Научный сотрудник",
              BaseRole = RoleTypeId.Scientist,
              Cinfo = "<color=#00FFFF>Младший Научный сотрудник, 2 УД, Класс C</color>",
              
              BroadcastColor = "yellow",
              Health = 100f,
              Chance = 90,
              SubId = 1,
              substext = "Младший НС",
              Items = {ItemType.KeycardScientist, ItemType.Medkit, ItemType.Radio}
            },

            new Subclass()
            {
              Name = "Научный сотрудник",
              BaseRole = RoleTypeId.Scientist,
              Cinfo = "<color=#00FFFF>Старший Научный сотрудник, 3 УД, Класс C</color>",
              Health = 100f,
              BroadcastColor = "yellow",
              Chance = 60,
              SubId = 2,
              substext = "Старший НС",
              Items = {ItemType.KeycardResearchCoordinator, ItemType.Medkit, ItemType.Radio}

            },
            new Subclass()
            {
              Name = "Научный сотрудник",
              BaseRole = RoleTypeId.Scientist,
              Cinfo = "<color=#C50000>Руководитель комплекса, 4 УД, Класс B</color>",
              Health = 100f,
              BroadcastColor = "#C50000",
              Chance = 10,
              SubId = 3,
              Limit = 1,
              substext = "Руководитель комплекса",
              Items = {ItemType.KeycardFacilityManager, ItemType.Medkit, ItemType.Radio, ItemType.GunCOM18, ItemType.Adrenaline, ItemType.ArmorLight, ItemType.Ammo9x19}

            },
            new Subclass()
            {
              Name = "Служба безопасности",
              BaseRole = RoleTypeId.FacilityGuard,
              Cinfo = "<color=#00FFFF>Стажер СБ, 2 УД, Класс C</color>",
              Health = 100f,
              BroadcastColor = "grey",
              Chance = 80,
              SubId = 4,
              substext = "Стажер СБ",
              Items = {ItemType.KeycardGuard, ItemType.Medkit, ItemType.Radio, ItemType.GunFSP9, ItemType.Painkillers, ItemType.ArmorLight, ItemType.Ammo9x19, ItemType.Ammo9x19}

            },
            new Subclass()
            {
              Name = "Служба безопасности",
              BaseRole = RoleTypeId.FacilityGuard,
              Cinfo = "<color=#00FFFF>Сержант СБ, 3 УД, Класс C</color>",
              Health = 100f,
              BroadcastColor = "grey",
              Chance = 50,
              SubId = 5,
              substext = "Сержант СБ",
              Items = {ItemType.KeycardMTFPrivate, ItemType.Medkit, ItemType.Radio, ItemType.GunCrossvec, ItemType.Adrenaline, ItemType.ArmorCombat, ItemType.Ammo9x19, ItemType.Ammo9x19, ItemType.GrenadeFlash}

            },
            new Subclass()
            {
              Name = "Служба безопасности",
              BaseRole = RoleTypeId.FacilityGuard,
              Cinfo = "<color=#C50000>Глава СБ, 4 УД, Класс B</color>",
              Health = 100f,
              BroadcastColor = "#C50000",
              Chance = 20,
              SubId = 6,
              Limit = 1,
              substext = "Капитан СБ",
              Items = {ItemType.KeycardMTFOperative, ItemType.Medkit, ItemType.Radio, ItemType.GunE11SR, ItemType.Adrenaline, ItemType.ArmorCombat, ItemType.GrenadeFlash, ItemType.Ammo556x45, ItemType.Ammo556x45}

            },
            new Subclass()
            {
              Name = "Мобильная Оперативная Группа Эпсилон-11",
              BaseRole = RoleTypeId.NtfPrivate,
              Cinfo = "<color=#00B7EB>Рядовой МОГ, 3 УД, Класс B</color>",
              Health = 100f,
              BroadcastColor = "#00B7EB",
              Chance = 80,
              SubId = 7,
              substext = "Рядовой МОГ Эпсилон-11",
              Items = {ItemType.KeycardMTFPrivate, ItemType.Medkit, ItemType.Radio, ItemType.GunCrossvec, ItemType.ArmorCombat, ItemType.GrenadeHE, ItemType.Ammo9x19, ItemType.Ammo9x19}

            },
            new Subclass()
            {
              Name = "Мобильная Оперативная Группа Эпсилон-11",
              BaseRole = RoleTypeId.NtfSergeant,
              Cinfo = "<color=#00B7EB>Сержант МОГ, 3 УД, Класс B</color>",
              Health = 100f,
              BroadcastColor = "#00B7EB",
              Chance = 60,
              SubId = 8,
              substext = "Сержант МОГ Эпсилон-11",
              Items = {ItemType.KeycardMTFOperative, ItemType.Medkit, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorCombat, ItemType.GrenadeHE, ItemType.Adrenaline, ItemType.Ammo556x45, ItemType.Ammo556x45}

            },
            new Subclass()
            {
              Name = "Мобильная Оперативная Группа Эпсилон-11",
              BaseRole = RoleTypeId.NtfCaptain,
              Cinfo = "<color=#00B7EB>Капитан МОГ, 4 УД, Класс B</color>",
              Health = 100f,
              BroadcastColor = "#00B7EB",
              Chance = 30,
              SubId = 9,
              Limit = 1,
              substext = "Капитан МОГ Эпсилон-11",
              Items = {ItemType.KeycardMTFCaptain, ItemType.Medkit, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.GrenadeHE, ItemType.Adrenaline, ItemType.Ammo556x45, ItemType.Ammo556x45 }

            },



        };
    }

    public class RoleNames
    {
        public List<string> ScientistNames { get; set; } = new List<string>
        {
            "Др. Джеймс",
            "Др. Джон",
            "Михаил Витте"
        };
        public List<string> GuardNames { get; set; } = new List<string>
        {
            "Др. Джеймс",
            "Др. Джон",
            "Михаил Витте"
        };
        public List<string> NtfNames { get; set; } = new List<string>
        {
            "Др. Джеймс",
            "Др. Джон",
            "Михаил Витте"
        };
        public List<string> CINames { get; set; } = new List<string>
        {
            "Др. Джеймс",
            "Др. Джон",
            "Михаил Витте"
        };
        public string ClassDNames { get; set; } = "D-{0:0000}";
    }

    public class Subclass
    {
        public string Name { get; set; } = "Пример";
        public string Cinfo { get; set; } = "Пример";
        public string BroadcastColor { get; set; } = "red";
        public RoleTypeId BaseRole { get; set; } = RoleTypeId.Scientist;
        public float Health { get; set; } = 100f;
        public int Chance { get; set; } = 0;
        public int SubId { get; set; } = 0;
        public int Limit { get; set; } = 0;
        public string substext { get; set; } = "test";
        public List<ItemType> Items { get; set; } = new List<ItemType>();

    }
}
