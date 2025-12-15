using System;

namespace InventorySystem
{
    // ИНТЕРФЕЙС ФАБРИКИ - определяет контракт для всех фабрик
    // Каждая фабрика должна уметь создавать разные типы предметов
    public interface IItemFactory
    {
        Weapon CreateWeapon();
        Armor CreateArmor();
        Consumable CreateConsumable();
    }

    // ФАБРИКА ПРОСТЫХ ПРЕДМЕТОВ
    // Создаёт стартовые, обычные предметы (Common)
    public class CommonItemFactory : IItemFactory
    {
        public Weapon CreateWeapon()
        {
            return new Weapon(
                name: "Деревянный меч",
                weight: 2.5,
                rarity: ItemRarity.Common,
                value: 50,
                damage: 5,
                damageType: "Рубящее"
            );
        }

        public Armor CreateArmor()
        {
            return new Armor(
                name: "Кожаная броня",
                weight: 5.0,
                rarity: ItemRarity.Common,
                value: 75,
                defense: 3,
                armorType: "Кожаная"
            );
        }

        public Consumable CreateConsumable()
        {
            return new Consumable(
                name: "Зелье здоровья (малое)",
                weight: 0.3,
                rarity: ItemRarity.Common,
                value: 25,
                effect: "Восстанавливает 30 HP",
                quantity: 3
            );
        }
    }

    // ФАБРИКА РЕДКИХ ПРЕДМЕТОВ
    // Создаёт более мощные предметы (Rare и выше)
    public class RareItemFactory : IItemFactory
    {
        public Weapon CreateWeapon()
        {
            return new Weapon(
                name: "Стальной меч Огня",
                weight: 3.0,
                rarity: ItemRarity.Rare,
                value: 500,
                damage: 15,
                damageType: "Магическое"
            );
        }

        public Armor CreateArmor()
        {
            return new Armor(
                name: "Магическая броня",
                weight: 8.0,
                rarity: ItemRarity.Rare,
                value: 800,
                defense: 8,
                armorType: "Магическая"
            );
        }

        public Consumable CreateConsumable()
        {
            return new Consumable(
                name: "Эликсир манны",
                weight: 0.5,
                rarity: ItemRarity.Rare,
                value: 200,
                effect: "Восстанавливает 100 манны",
                quantity: 1
            );
        }
    }

    // ФАБРИКА ЛЕГЕНДАРНЫХ ПРЕДМЕТОВ
    // Создаёт эпические и легендарные предметы
    public class LegendaryItemFactory : IItemFactory
    {
        public Weapon CreateWeapon()
        {
            return new Weapon(
                name: "Экскалибур",
                weight: 4.0,
                rarity: ItemRarity.Legendary,
                value: 5000,
                damage: 50,
                damageType: "Божественное"
            );
        }

        public Armor CreateArmor()
        {
            return new Armor(
                name: "Броня Драконьей Шкуры",
                weight: 12.0,
                rarity: ItemRarity.Legendary,
                value: 8000,
                defense: 20,
                armorType: "Легендарная"
            );
        }

        public Consumable CreateConsumable()
        {
            return new Consumable(
                name: "Зелье Воскрешения",
                weight: 1.0,
                rarity: ItemRarity.Legendary,
                value: 3000,
                effect: "Воскрешает с 100% HP",
                quantity: 1
            );
        }
    }

    // ВСПОМОГАТЕЛЬНЫЙ КЛАСС - FactoryProvider
    // Удобный способ получить нужную фабрику по редкости
    public static class ItemFactoryProvider
    {
        // Возвращает подходящую фабрику в зависимости от редкости
        public static IItemFactory GetFactory(ItemRarity rarity)
        {
            return rarity switch
            {
                ItemRarity.Common => new CommonItemFactory(),
                ItemRarity.Uncommon => new CommonItemFactory(),
                ItemRarity.Rare => new RareItemFactory(),
                ItemRarity.Epic => new LegendaryItemFactory(),
                ItemRarity.Legendary => new LegendaryItemFactory(),
                _ => new CommonItemFactory()
            };
        }
    }
}