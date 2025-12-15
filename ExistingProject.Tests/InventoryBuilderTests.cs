using System;
using System.Collections.Generic;
using Xunit;
using InventorySystem;namespace ExistingProject.Tests;

public class InventoryBuilderTests
{
    [Fact]
    public void Builder_CreatesInventoryWithDefaultValues()
    {
        // Arrange & Act
        var inventory = new InventoryBuilder().Build();
        
        // Assert
        Assert.NotNull(inventory);
        Assert.Equal(0, inventory.GetItemCount());
    }

    [Fact]
    public void Builder_SetsOwnerName()
    {
        // Arrange & Act
        var inventory = new InventoryBuilder()
            .WithOwnerName("Герой")
            .Build();
        
        // Assert
        Assert.Equal("Герой", inventory.OwnerName);
    }

    [Fact]
    public void Builder_SetsMaxWeight()
    {
        // Arrange & Act
        var inventory = new InventoryBuilder()
            .WithMaxWeight(100.0)
            .Build();
        
        // Assert
        Assert.Equal(100.0, inventory.MaxWeight);
    }

    [Fact]
    public void Builder_ThrowsException_WhenOwnerNameIsEmpty()
    {
        // Arrange
        var builder = new InventoryBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.WithOwnerName(""));
    }

    [Fact]
    public void Builder_ThrowsException_WhenMaxWeightIsNegative()
    {
        // Arrange
        var builder = new InventoryBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.WithMaxWeight(-10));
    }

    [Fact]
    public void Builder_AddsInitialItems()
    {
        // Arrange
        var item1 = new Weapon("Меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее");
        var item2 = new Armor("Броня", 5.0, ItemRarity.Common, 75, 3, "Кожаная");
        
        // Act
        var inventory = new InventoryBuilder()
            .AddInitialItem(item1)
            .AddInitialItem(item2)
            .Build();
        
        // Assert
        Assert.Equal(2, inventory.GetItemCount());
    }

    [Fact]
    public void Builder_SetsWarehouseMode()
    {
        // Arrange & Act
        var inventory = new InventoryBuilder()
            .UseWarehouseMode(true)
            .Build();
        
        // Добавляем много предметов (больше 20)
        for (int i = 0; i < 25; i++)
        {
            var item = new Weapon($"Меч {i}", 1.0, ItemRarity.Common, 50, 5, "Рубящее");
            inventory.AddItem(item);
        }
        
        // Assert
        Assert.Equal(25, inventory.GetItemCount());
    }
}
