using System;
using System.Collections.Generic;
using Xunit;
using InventorySystem;namespace ExistingProject.Tests;

public class InventoryStrategyTests
{
    [Fact]
    public void SortByRarityStrategy_SortsItemsByRarity()
    {
        // Arrange
        var strategy = new SortByRarityStrategy();
        var items = new List<Item>
        {
            new Weapon("Обычный меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее"),
            new Weapon("Легендарный меч", 3.0, ItemRarity.Legendary, 5000, 50, "Божественное"),
            new Weapon("Редкий меч", 2.5, ItemRarity.Rare, 500, 15, "Магическое")
        };
        
        // Act
        var sorted = strategy.Organize(items);
        
        // Assert
        Assert.Equal(ItemRarity.Legendary, sorted[0].Rarity);
        Assert.Equal(ItemRarity.Rare, sorted[1].Rarity);
        Assert.Equal(ItemRarity.Common, sorted[2].Rarity);
    }

    [Fact]
    public void SortByWeightStrategy_SortsItemsByWeight()
    {
        // Arrange
        var strategy = new SortByWeightStrategy();
        var items = new List<Item>
        {
            new Weapon("Тяжелый меч", 5.0, ItemRarity.Common, 100, 10, "Рубящее"),
            new Consumable("Зелье", 0.3, ItemRarity.Common, 25, "Исцеление", 1),
            new Armor("Броня", 8.0, ItemRarity.Common, 200, 10, "Металлическая")
        };
        
        // Act
        var sorted = strategy.Organize(items);
        
        // Assert
        Assert.True(sorted[0].Weight <= sorted[1].Weight);
        Assert.True(sorted[1].Weight <= sorted[2].Weight);
    }

    [Fact]
    public void SortByValueStrategy_SortsItemsByValue()
    {
        // Arrange
        var strategy = new SortByValueStrategy();
        var items = new List<Item>
        {
            new Weapon("Дешевый меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее"),
            new Weapon("Дорогой меч", 3.0, ItemRarity.Legendary, 5000, 50, "Божественное"),
            new Weapon("Средний меч", 2.5, ItemRarity.Rare, 500, 15, "Магическое")
        };
        
        // Act
        var sorted = strategy.Organize(items);
        
        // Assert
        Assert.True(sorted[0].Value >= sorted[1].Value);
        Assert.True(sorted[1].Value >= sorted[2].Value);
    }

    [Fact]
    public void GroupByTypeStrategy_GroupsItemsByType()
    {
        // Arrange
        var strategy = new GroupByTypeStrategy();
        var items = new List<Item>
        {
            new Consumable("Зелье", 0.3, ItemRarity.Common, 25, "Исцеление", 1),
            new Weapon("Меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее"),
            new Armor("Броня", 5.0, ItemRarity.Common, 75, 3, "Кожаная"),
            new QuestItem("Ключ", 0.2, ItemRarity.Rare, 0, "Квест", false)
        };
        
        // Act
        var organized = strategy.Organize(items);
        
        // Assert
        Assert.IsType<Weapon>(organized[0]);
        Assert.IsType<Armor>(organized[1]);
        Assert.IsType<Consumable>(organized[2]);
        Assert.IsType<QuestItem>(organized[3]);
    }

    [Fact]
    public void NoSortingStrategy_KeepsOriginalOrder()
    {
        // Arrange
        var strategy = new NoSortingStrategy();
        var items = new List<Item>
        {
            new Weapon("Меч 1", 2.0, ItemRarity.Common, 50, 5, "Рубящее"),
            new Weapon("Меч 2", 3.0, ItemRarity.Rare, 500, 15, "Магическое"),
            new Weapon("Меч 3", 4.0, ItemRarity.Legendary, 5000, 50, "Божественное")
        };
        
        // Act
        var organized = strategy.Organize(items);
        
        // Assert
        Assert.Equal("Меч 1", organized[0].Name);
        Assert.Equal("Меч 2", organized[1].Name);
        Assert.Equal("Меч 3", organized[2].Name);
    }
}
