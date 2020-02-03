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
      Messages.Add("You awake to the nausea of extended cryogenics. Your stasis pod doors open upon sensing your slight activity, and you note that that means power is still flowing in the station. ... but then you realize with mounting horror that you don't know what station you're on. As a matter of fact, you don't know who you are.\n");
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
    public bool Go(string direction, bool playing)
    {
      System.Console.Clear();
      if (_game.CurrentRoom.Exits.ContainsKey(direction))
      {
        _game.CurrentRoom = _game.CurrentRoom.Exits[direction];
        if (_game.CurrentRoom.TrapActive && _game.CurrentRoom.Name == "Room3")
        {
          _game.CurrentRoom.TrapActive = false;
          System.Console.WriteLine("As you open the airlock door to the next room, oxygen floods back into it - igniting once it reaches the exposed electrical panel that started the fire that previously charred the room and the plants within. Flames erupt all around you. What do you do?");
          string response = System.Console.ReadLine().ToLower().Trim();
          // TODO Need to check if in inventory
          if (response == "use fire extinguisher" && _game.CurrentPlayer.Inventory.Any(i => i.Name == "Fire Extinguisher"))
          {
            System.Console.Clear();
            System.Console.WriteLine("As you use the fire extinguisher to extinguish the flames, the force of the expelled material pushes you against the wall. You gain a handhold and are able to save yourself from the flames.\nPress any key to continue");
            System.Console.ReadKey();
            System.Console.Clear();
            return true;
          }
          else if (response == "use fire extinguisher")
          {
            System.Console.Clear();
            System.Console.WriteLine("Unfortunately, this item is not in your inventory.\nPress any key to continue");
            System.Console.ReadKey();
            System.Console.Clear();
            return false;
          }
          else
          {
            System.Console.Clear();
            System.Console.WriteLine("Your efforts are ineffective. The flames spread, and your flesh burns. You die an excrutiating death.\nPress any key to continue");

            System.Console.ReadKey();
            System.Console.Clear();
            return false;
          }
        }
        Messages.Add($"You go {direction}.");

        return true;
      }
      Messages.Add("Invalid input. Please try again.\n");
      return true;
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
        System.Console.Clear();
        Messages.Add("There is nothing in your inventory.");
        return;
      }
      System.Console.Clear();
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

      _game.CurrentPlayer = new Player(playerName);
      Messages.Add($"created {_game.CurrentPlayer.Name}");
      Messages.Add($"Has {_game.CurrentPlayer.Inventory}");
    }
    ///<summary>When taking an item be sure the item is in the current room before adding it to the player inventory, Also don't forget to remove the item from the room it was picked up in</summary>
    public void TakeItem(string itemName)
    {
      if (_game.CurrentRoom.Items.Count == 0)
      {
        Messages.Add("No items are in this room.");
        return;
      }
      //Enumerable currentItem = _game.CurrentRoom.Items.Where(i => i.Name == itemName);
      if (_game.CurrentRoom.Items.Any(i => i.Name.ToLower() == itemName && i.Takeable == false))
      {
        System.Console.Clear();
        Messages.Add("You cannot take this item. Try using it instead!");
        return;
      }

      if (_game.CurrentRoom.Items.Any(i => i.Name.ToLower() == itemName))
      {
        _game.CurrentPlayer.Inventory.Add(_game.CurrentRoom.Items.FirstOrDefault(i => i.Name.ToLower() == itemName));
        System.Console.Clear();
        Messages.Add($"You picked up {itemName}");
        _game.CurrentRoom.Items.Remove(_game.CurrentRoom.Items.FirstOrDefault(i => i.Name.ToLower() == itemName));
      }
      else
      {
        Messages.Add("Invalid item name.");
      }
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
      if (itemName == "soyuz controls" && _game.CurrentRoom.Name == "Room4")
      {
        EndGame("soyuz controls");
        return false;
      }
      if (itemName == "telescopes console" && _game.CurrentRoom.Name == "Room2")
      {
        System.Console.Clear();
        Messages.Add("You pull up the most recent images taken from the bank of telescopes, and you watch with horror as nuclear plumes sprout across the surface of the earth. Something went terribly wrong down there... but it's been centuries already. Everyone you ever knew or loved is dead. The realization has only started to penetrate your mind, but you now know your goal: get back down there and see if you can help what's left of humanity.");
        return true;
      }
      if (itemName == "fire extinguisher" && _game.CurrentPlayer.Inventory.Any(i => i.Name == "Fire Extinguisher"))
      {
        System.Console.Clear();
        Messages.Add("You hose down the nearest surfaces with your fire extinguisher. Nothing happens except you're pushed in the opposite direction.");
        return true;
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
      if (condition == "soyuz controls")
      {
        System.Console.WriteLine("You close the airlock door behind you, and muscle memory takes over as you prepare the Soyuz Decent Module for launch. You feel a swell of hope for the first time since waking up... you're going back home. \n\nYou win!");
      }
      System.Console.WriteLine("\nPress any key to continue");
      System.Console.ReadKey();
      System.Console.Clear();

    }
  }
}