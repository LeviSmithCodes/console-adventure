using System.Collections.Generic;
using ConsoleAdventure.Project.Interfaces;
using ConsoleAdventure.Project.Models;
using System.Linq;

namespace ConsoleAdventure.Project
{
  public class GameService : IGameService
  {
    private IGame _game { get; set; }

    public List<string> Messages { get; set; }
    public GameService()
    {
      _game = new Game();
      Messages = new List<string>();
    }

    public void PrintStart()
    {
      Messages.Add(@"
   _____                         _____ _        _   _             
  / ____|                       / ____| |      | | (_)            
 | (___  _ __   __ _  ___ ___  | (___ | |_ __ _| |_ _  ___  _ __  
  \___ \| '_ \ / _` |/ __/ _ \  \___ \| __/ _` | __| |/ _ \| '_ \ 
  ____) | |_) | (_| | (_|  __/  ____) | || (_| | |_| | (_) | | | |
 |_____/| .__/ \__,_|\___\___| |_____/ \__\__,_|\__|_|\___/|_| |_|
        | |                                                       
        |_|                                                       

      ");
      Messages.Add("You awake to the nausea of extended stasis. Your pod doors open upon sensing your slight activity, and you note that that means power is still flowing in the station. ... You ask yourself, 'What station?' As a matter of fact, you don't know who you are.\n");
    }

    public void PrintRoom()
    {
      Messages.Add($"The room you find yourself in is {_game.CurrentRoom.Description}\n");
      Messages.Add("The items in the room are: ");
      foreach (var item in _game.CurrentRoom.Items)
      {
        Messages.Add(item.Name);
      }
      Messages.Add("\n");
      Messages.Add("Your available exits are: ");
      foreach (var exit in _game.CurrentRoom.Exits)
      {
        Messages.Add($"{exit.Key}");
      }
      Messages.Add("\nType 'Help' for available commands.");
    }
    public void Go(string direction)
    {
      System.Console.Clear();
      if (_game.CurrentRoom.Exits.ContainsKey(direction))
      {
        Messages.Add($"You go {direction}.");
        _game.CurrentRoom = _game.CurrentRoom.Exits[direction];
        return;
      }
      Messages.Add("Invalid input. Please try again.");
    }
    public void Help()
    {
      System.Console.Clear();
      Messages.Add("Available commands:\nGo <Direction> \nUse <Item Name>\nTake <Item Name>\nLook\nInventory\nQuit");
    }

    public void Inventory()
    {
      // System.Console.WriteLine($"{_game.CurrentPlayer.Inventory}");
      if (_game.CurrentPlayer.Inventory.Count == 0)
      {
        Messages.Add("There is nothing in your inventory.");
        return;
      }
      Messages.Add("Your inventory consists of: ");
      foreach (var item in _game.CurrentPlayer.Inventory)
      {
        Messages.Add($"{item.Name}");
        return;
      }
    }

    public void Look()
    {

      System.Console.Clear();
      PrintRoom();
    }

    public bool Quit(bool playing)
    {
      playing = false;
      return playing;
    }
    ///<summary>
    ///Restarts the game 
    ///</summary>
    public void Reset()
    {
      throw new System.NotImplementedException();
    }

    public void Setup(string playerName)
    {

      Player CurrentPlayer = new Player(playerName);
      Messages.Add($"created {CurrentPlayer.Name}");
      Messages.Add($"Has {CurrentPlayer.Inventory}");
    }
    ///<summary>When taking an item be sure the item is in the current room before adding it to the player inventory, Also don't forget to remove the item from the room it was picked up in</summary>
    public void TakeItem(string itemName)
    {
      if (_game.CurrentRoom.Items.Count == 0)
      {
        Messages.Add("No items are in this room.");
      }
      //Enumerable currentItem = _game.CurrentRoom.Items.Where(i => i.Name == itemName);
      if (_game.CurrentRoom.Items.Any(i => i.Name == itemName))
      {
        _game.CurrentPlayer.Inventory.Add(_game.CurrentRoom.Items.FirstOrDefault(i => i.Name == itemName));
        Messages.Add($"You picked up {_game.CurrentRoom.Items.FirstOrDefault(i => i.Name == itemName)}");
        _game.CurrentRoom.Items.Remove(_game.CurrentRoom.Items.FirstOrDefault(i => i.Name == itemName));
      }
      Messages.Add("Invalid item name.");
    }
    ///<summary>
    ///No need to Pass a room since Items can only be used in the CurrentRoom
    ///Make sure you validate the item is in the room or player inventory before
    ///being able to use the item
    ///</summary>
    public bool UseItem(string itemName)
    {
      if (itemName == "airlock" && _game.CurrentRoom.Name == "Room1")
      {
        EndGame("airlock");
        // ends game
        return false;
      }
      if (itemName == "stasis pod" && _game.CurrentRoom.Name == "Room1")
      {
        EndGame("stasis pod");
        return false;
      }
      Messages.Add("Invalid item name.");
      // continues game
      return true;
    }
    public void EndGame(string condition)
    {
      System.Console.Clear();
      if (condition == "airlock")
      {

        System.Console.WriteLine("Fighting every instinct in your body, you open the airlock. Your body is sucked into space, and you slowly suffocate in the absolute stillness of space.");

      }
      if (condition == "stasis pod")
      {
        System.Console.WriteLine("You climb back into the stasis pod, and it automatically activates, dragging you back to unconsciousness. Years pass, and the solar panels eventually fail... you die without ever waking up.");
      }
      System.Console.WriteLine("\nPress enter to continue");
      string result = System.Console.ReadLine();

    }
  }
}