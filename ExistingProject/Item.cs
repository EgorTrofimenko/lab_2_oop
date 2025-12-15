using System;

namespace InventorySystem
{
    // Перечисление для редкости предметов
    public enum ItemRarity
    {
        Common,      // Обычное
        Uncommon,    // Необычное
        Rare,        // Редкое
        Epic,        // Эпическое
        Legendary    // Легендарное
    }

    // БАЗОВЫЙ КЛАСС для всех предметов
    // Содержит общие свойства: имя, вес, редкость, стоимость
    public abstract class Item
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public ItemRarity Rarity { get; set; }
        public int Value { get; set; } // Стоимость в золоте

        public Item(string name, double weight, ItemRarity rarity, int value) // конструктор класса
        {
            Name = name;
            Weight = weight;
            Rarity = rarity;
            Value = value;
        }

        // Абстрактный метод - каждый предмет описывает себя по-своему
        public abstract string GetDescription();

        public override string ToString() // переопределение метода, ToString - вызывается при работе ф-ии, выводящей текст в консоль
        {
            return $"[{Rarity}] {Name} | Вес: {Weight}кг | Цена: {Value}";
        }
    }

    // ОРУЖИЕ - наследуется от Item
    // Дополняет базовый класс параметром "Урон"
    public class Weapon : Item
    {
        public int Damage { get; set; }
        public string DamageType { get; set; } // "Рубящее", "Колющее", "Магическое"

        public Weapon(string name, double weight, ItemRarity rarity, int value, 
                      int damage, string damageType)
            : base(name, weight, rarity, value)
        {
            Damage = damage;
            DamageType = damageType;
        }

        public override string GetDescription()
        {
            return $"{ToString()} | Урон: {Damage} ({DamageType})";
        }
        public void EquipItem()
        {
            Console.WriteLine($"Экипирован предмет: {Name}");
        }
    }

    // БРОНЯ - наследуется от Item
    // Дополняет базовый класс параметром "Защита"
    public class Armor : Item
    {
        public int Defense { get; set; }
        public string ArmorType { get; set; } // "Кожаная", "Металлическая", "Магическая"

        public Armor(string name, double weight, ItemRarity rarity, int value, 
                     int defense, string armorType)
            : base(name, weight, rarity, value)
        {
            Defense = defense;
            ArmorType = armorType;
        }

        public override string GetDescription()
        {
            return $"{ToString()} | Защита: +{Defense} ({ArmorType})";
        }
    }

    // ЗЕЛЬЕ/РАСХОДНИК - наследуется от Item
    // Дополняет базовый класс количеством использований
    public class Consumable : Item
    {
        public int Quantity { get; set; } // Сколько штук в инвентаре
        public string Effect { get; set; } // Эффект: "Исцеление", "Манна", "Буст урона"

        public Consumable(string name, double weight, ItemRarity rarity, int value, 
                          string effect, int quantity = 1)
            : base(name, weight, rarity, value)
        {
            Effect = effect;
            Quantity = quantity;
        }

        public override string GetDescription()
        {
            return $"{ToString()} x{Quantity} | Эффект: {Effect}";
        }
    }

    // КВЕСТОВЫЙ ПРЕДМЕТ - наследуется от Item
    // Специальный предмет, который нельзя выбросить просто так
    public class QuestItem : Item
    {
        public string QuestName { get; set; }
        public bool IsOptional { get; set; }

        public QuestItem(string name, double weight, ItemRarity rarity, int value, 
                         string questName, bool isOptional = false)
            : base(name, weight, rarity, value)
        {
            QuestName = questName;
            IsOptional = isOptional;
        }

        public override string GetDescription()
        {
            string optional = IsOptional ? " (Опционально)" : " (Обязательно)";
            return $"{ToString()} | Квест: {QuestName}{optional}";
        }
    }
}