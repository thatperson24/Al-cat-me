using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private string ingredientName;
    [SerializeField] private int damage;
    [SerializeField] private int range;
    [SerializeField] private int aoe;
    [SerializeField] private int castType;
    [SerializeField] private int blocking;
    [SerializeField] private int delay;

    public static readonly IReadOnlyDictionary<int, string> Adjectives =
        new Dictionary<int, string>
        {
            {1, "Ancient"}, {2, "Arcane"}, {3, "Brilliant"}, {4, "Chaotic"}, {5, "Cursed"},
            {6, "Dazzling"}, {7, "Ethereal"}, {8, "Fiery"}, {9, "Frozen"}, {10, "Glorious"},
            {11, "Grim"}, {12, "Hollow"}, {13, "Infinite"}, {14, "Jagged"}, {15, "Luminous"},
            {16, "Mighty"}, {17, "Mystic"}, {18, "Noble"}, {19, "Obscure"}, {20, "Radiant"},
            {21, "Sacred"}, {22, "Savage"}, {23, "Shimmering"}, {24, "Silent"}, {25, "Sinister"},
            {26, "Spectral"}, {27, "Spiky"}, {28, "Stormy"}, {29, "Swift"}, {30, "Thundering"},
            {31, "Timeless"}, {32, "Twisted"}, {33, "Unholy"}, {34, "Vast"}, {35, "Venomous"},
            {36, "Vicious"}, {37, "Volatile"}, {38, "Weird"}, {39, "Wild"}, {40, "Wicked"},
            {41, "Agile"}, {42, "Brittle"}, {43, "Burning"}, {44, "Calm"}, {45, "Celestial"},
            {46, "Colossal"}, {47, "Corrupted"}, {48, "Crimson"}, {49, "Dark"}, {50, "Deadly"},
            {51, "Delirious"}, {52, "Divine"}, {53, "Draconic"}, {54, "Dusty"}, {55, "Elegant"},
            {56, "Enchanted"}, {57, "Enigmatic"}, {58, "Feral"}, {59, "Forgotten"}, {60, "Fragile"},
            {61, "Ghastly"}, {62, "Gleaming"}, {63, "Golden"}, {64, "Grotesque"}, {65, "Haunted"},
            {66, "Heroic"}, {67, "Hidden"}, {68, "Horrid"}, {69, "Icy"}, {70, "Immortal"},
            {71, "Infernal"}, {72, "Iron"}, {73, "Jeweled"}, {74, "Kindred"}, {75, "Lethal"},
            {76, "Living"}, {77, "Lost"}, {78, "Magnetic"}, {79, "Malevolent"}, {80, "Mournful"},
            {81, "Mysterious"}, {82, "Ominous"}, {83, "Otherworldly"}, {84, "Phantom"}, {85, "Poisonous"},
            {86, "Primal"}, {87, "Proud"}, {88, "Raging"}, {89, "Reckless"}, {90, "Runic"},
            {91, "Sacrificial"}, {92, "Secret"}, {93, "Shadowy"}, {94, "Sharp"}, {95, "Shattered"},
            {96, "Smoldering"}, {97, "Solar"}, {98, "Terrifying"}, {99, "Toxic"}, {100, "Unseen"}
        };

    public static readonly IReadOnlyDictionary<int, string> Colors =
    new Dictionary<int, string>
    {
            {1, "Amber"}, {2, "Amethyst"}, {3, "Aqua"}, {4, "Azure"}, {5, "Beige"},
            {6, "Black"}, {7, "Blue"}, {8, "Bronze"}, {9, "Brown"}, {10, "Cerulean"},
            {11, "Charcoal"}, {12, "Copper"}, {13, "Crimson"}, {14, "Cyan"}, {15, "Emerald"},
            {16, "Gold"}, {17, "Gray"}, {18, "Green"}, {19, "Indigo"}, {20, "Ivory"},
            {21, "Jade"}, {22, "Lavender"}, {23, "Lilac"}, {24, "Magenta"}, {25, "Maroon"},
            {26, "Mint"}, {27, "Navy"}, {28, "Obsidian"}, {29, "Olive"}, {30, "Onyx"},
            {31, "Orange"}, {32, "Peach"}, {33, "Pearl"}, {34, "Pink"}, {35, "Platinum"},
            {36, "Purple"}, {37, "Quartz"}, {38, "Red"}, {39, "Rose"}, {40, "Ruby"},
            {41, "Saffron"}, {42, "Sapphire"}, {43, "Scarlet"}, {44, "Sepia"}, {45, "Silver"},
            {46, "Teal"}, {47, "Turquoise"}, {48, "Violet"}, {49, "White"}, {50, "Yellow"}
    };

    public static readonly IReadOnlyDictionary<int, string> Objects =
        new Dictionary<int, string>
        {
            {1, "Amulet"}, {2, "Ankh"}, {3, "Arrow"}, {4, "Axe"}, {5, "Banner"},
            {6, "Bell"}, {7, "Blade"}, {8, "Book"}, {9, "Bottle"}, {10, "Box"},
            {11, "Bracelet"}, {12, "Brick"}, {13, "Brooch"}, {14, "Candle"}, {15, "Cauldron"},
            {16, "Chain"}, {17, "Charm"}, {18, "Chest"}, {19, "Claw"}, {20, "Coin"},
            {21, "Compass"}, {22, "Crown"}, {23, "Crystal"}, {24, "Cup"}, {25, "Dagger"},
            {26, "Dice"}, {27, "Disk"}, {28, "Drum"}, {29, "Egg"}, {30, "Feather"},
            {31, "Figurine"}, {32, "Flame"}, {33, "Flask"}, {34, "Flower"}, {35, "Gem"},
            {36, "Glove"}, {37, "Goblet"}, {38, "Hammer"}, {39, "Heart"}, {40, "Helm"},
            {41, "Horn"}, {42, "Idol"}, {43, "Key"}, {44, "Lantern"}, {45, "Leaf"},
            {46, "Mask"}, {47, "Medallion"}, {48, "Mirror"}, {49, "Moon"}, {50, "Necklace"},
            {51, "Needle"}, {52, "Orb"}, {53, "Pendant"}, {54, "Phial"}, {55, "Pillar"},
            {56, "Pipe"}, {57, "Ring"}, {58, "Rod"}, {59, "Rune"}, {60, "Scepter"},
            {61, "Scroll"}, {62, "Shard"}, {63, "Shield"}, {64, "Sigil"}, {65, "Skull"},
            {66, "Sphere"}, {67, "Spike"}, {68, "Staff"}, {69, "Star"}, {70, "Stone"},
            {71, "Sword"}, {72, "Tablet"}, {73, "Talisman"}, {74, "Thread"}, {75, "Throne"},
            {76, "Torch"}, {77, "Totem"}, {78, "Tower"}, {79, "Trident"}, {80, "Vial"},
            {81, "Wand"}, {82, "Wheel"}, {83, "Wing"}, {84, "Bone"}, {85, "Anchor"},
            {86, "Helmet"}, {87, "Chainmail"}, {88, "Gauntlet"}, {89, "Boot"}, {90, "Cloak"},
            {91, "Quill"}, {92, "Scrollcase"}, {93, "Censer"}, {94, "Chalice"}, {95, "Casket"},
            {96, "Obelisk"}, {97, "Totem Pole"}, {98, "Hourglass"}, {99, "Lyre"}, {100, "Crown Jewel"}
        };


    public Ingredient()
    {
        var rand = new System.Random();
        string adjective = Adjectives[rand.Next(1, 101)];
        string color = Colors[rand.Next(1, 51)];
        string obj = Objects[rand.Next(1, 101)];
        this.ingredientName = $"{adjective} {color} {obj}";

        this.damage = rand.Next(-3, 8);
        this.range = rand.Next(-3, 8);
        this.aoe = rand.Next(-3, 8);
        this.castType = rand.Next(-3, 8);
        this.blocking = rand.Next(-3, 8);
        this.delay = rand.Next(-3, 8);

    }

    public int[] GetStats()
    {
        int[] stats = { damage, range, aoe, castType, blocking, delay };
        return stats;
    }

    public string GetName()
    {
        return ingredientName;
    }
}
