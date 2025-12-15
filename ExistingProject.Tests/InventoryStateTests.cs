using System;
using System.Collections.Generic;
using Xunit;
using InventorySystem;namespace ExistingProject.Tests;

public class InventoryStateTests
{
    [Fact]
    public void NormalState_AllowsAddingItems_WhenNotFull()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 100.0, new NoSortingStrategy());
        var item = new Weapon("Меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее");
        var state = new NormalInventoryState();
        
        // Act
        var result = state.TryAddItem(item, inventory);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void NormalState_BlocksAdding_WhenFull()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 500.0, new NoSortingStrategy());
        var state = new NormalInventoryState();
        
        // Добавляем 20 предметов (максимум)
        for (int i = 0; i < 20; i++)
        {
            var item = new Weapon($"Меч {i}", 2.0, ItemRarity.Common, 50, 5, "Рубящее");
            inventory.AddItem(item);
        }
        
        var extraItem = new Weapon("Лишний меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее");
        
        // Act
        var result = inventory.AddItem(extraItem);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OverflowState_BlocksAddingItems()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 100.0, new NoSortingStrategy());
        var item = new Weapon("Меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее");
        var state = new OverflowInventoryState();
        
        // Act
        var result = state.TryAddItem(item, inventory);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void WarehouseState_AllowsUnlimitedAdding()
    {
        // Arrange
        var inventory = new Inventory("Склад", 1000.0, new NoSortingStrategy(), true);
        var state = new WarehouseInventoryState();
        var item = new Weapon("Меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее");
        
        // Act
        var result = state.TryAddItem(item, inventory);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void LockedState_BlocksAddingAndRemoving()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 100.0, new NoSortingStrategy());
        var item = new Weapon("Меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее");
        var state = new LockedInventoryState("Тестовая блокировка");
        
        // Act
        var canAdd = state.TryAddItem(item, inventory);
        var canRemove = state.TryRemoveItem(item, inventory);
        
        // Assert
        Assert.False(canAdd);
        Assert.False(canRemove);
    }
}
