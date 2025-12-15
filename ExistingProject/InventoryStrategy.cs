using System;
using System.Collections.Generic;
using System.Linq;

namespace InventorySystem
{
    // ИНТЕРФЕЙС СТРАТЕГИИ - договор для всех стратегий сортировки
    public interface IOrganizationStrategy
    {
        // Метод, который сортирует список предметов по определённому критерию
        List<Item> Organize(List<Item> items);
        
        // Описание этой стратегии
        string GetStrategyName();
    }

    // СТРАТЕГИЯ 1 - Сортировка по редкости (от редкого к обычному)
    // Все легендарные предметы в начале, обычные в конце
    public class SortByRarityStrategy : IOrganizationStrategy
    {
        public List<Item> Organize(List<Item> items)
        {
            // LINQ: Сортируем по редкости в порядке убывания
            // Legendary (4) > Epic (3) > Rare (2) > Uncommon (1) > Common (0)
            return items.OrderByDescending(item => item.Rarity).ToList();
        }

        public string GetStrategyName() => "Сортировка по редкости (редкие первыми)";
    }

    // СТРАТЕГИЯ 2 - Сортировка по весу (лёгкие первыми)
    // Зелья и магические предметы впереди, тяжёлая броня сзади
    public class SortByWeightStrategy : IOrganizationStrategy
    {
        public List<Item> Organize(List<Item> items)
        {
            return items.OrderBy(item => item.Weight).ToList();
        }

        public string GetStrategyName() => "Сортировка по весу (лёгкие первыми)";
    }

    // СТРАТЕГИЯ 3 - Сортировка по стоимости (дорогие первыми)
    // Ценные предметы легче найти в начале списка
    public class SortByValueStrategy : IOrganizationStrategy
    {
        public List<Item> Organize(List<Item> items)
        {
            return items.OrderByDescending(item => item.Value).ToList();
        }

        public string GetStrategyName() => "Сортировка по стоимости (дорогие первыми)";
    }

    // СТРАТЕГИЯ 4 - Группировка по типам предметов
    // Оружие, потом броня, потом расходники, потом квестовые предметы
    public class GroupByTypeStrategy : IOrganizationStrategy
    {
        public List<Item> Organize(List<Item> items)
        {
            var organized = new List<Item>();

            // Добавляем в порядке: Weapon -> Armor -> Consumable -> QuestItem
            organized.AddRange(items.OfType<Weapon>().OrderByDescending(w => w.Damage));
            organized.AddRange(items.OfType<Armor>().OrderByDescending(a => a.Defense));
            organized.AddRange(items.OfType<Consumable>());
            organized.AddRange(items.OfType<QuestItem>());

            return organized;
        }

        public string GetStrategyName() => "Группировка по типам (Оружие → Броня → Зелья → Квесты)";
    }

    // СТРАТЕГИЯ 5 - Без сортировки (как есть)
    // Иногда нужно просто вывести предметы в том порядке, в котором они лежат
    public class NoSortingStrategy : IOrganizationStrategy
    {
        public List<Item> Organize(List<Item> items)
        {
            return new List<Item>(items); // Возвращаем копию без изменений
        }

        public string GetStrategyName() => "Без сортировки (как добавлены)";
    }
}