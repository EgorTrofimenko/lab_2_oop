using System;
using System.Collections.Generic;
using Xunit;
using InventorySystem;namespace ExistingProject.Tests;

public class ItemTest
{
    [Fact]
    public void Weapon_GetDescription_ReturnsCorrectFormat()
        {
            // Arrange
            var weapon = new Weapon("Меч", 2.5, ItemRarity.Common, 100, 10, "Рубящее");
            
            // Act
            var description = weapon.GetDescription();
            
            // Assert
            Assert.Contains("Меч", description);
            Assert.Contains("10", description);
            Assert.Contains("Рубящее", description);
        }
    [Fact]
    public void Armor_GetDescription_ReturnsCorrectFormat()
    {
        // Arrange
        var armor = new Armor("Кожаная броня", 5.0, ItemRarity.Rare, 200, 15, "Кожаная");
        
        // Act
        var description = armor.GetDescription();
        
        // Assert
        Assert.Contains("Кожаная броня", description);
        Assert.Contains("15", description);
        Assert.Contains("Кожаная", description);
    }

    [Fact]
    public void Consumable_GetDescription_ShowsQuantity()
    {
        // Arrange
        var consumable = new Consumable("Зелье", 0.5, ItemRarity.Common, 50, "Исцеление", 3);
        
        // Act
        var description = consumable.GetDescription();
        
        // Assert
        Assert.Contains("x3", description);
        Assert.Contains("Исцеление", description);
    }
    [Fact]
    public void QuestItem_OptionalItem_ShowsCorrectLabel()
    {
        // Arrange
        var questItem = new QuestItem("Ключ", 0.2, ItemRarity.Rare, 0, "Древний замок", true);
        
        // Act
        var description = questItem.GetDescription();
        
        // Assert
        Assert.Contains("Опционально", description);
    }

    [Fact]
    public void QuestItem_NonOptional_ShowsCorrectLabel()
    {
        // Arrange
        var questItem = new QuestItem("Амулет", 0.3, ItemRarity.Epic, 0, "Спасение мира", false);
        
        // Act
        var description = questItem.GetDescription();
        
        // Assert
        Assert.Contains("Обязательно", description);
    }
}
