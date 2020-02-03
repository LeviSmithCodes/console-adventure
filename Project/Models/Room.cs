using System.Collections.Generic;
using ConsoleAdventure.Project.Interfaces;

namespace ConsoleAdventure.Project.Models
{
  public class Room : IRoom
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Item> Items { get; set; }
    public Dictionary<string, IRoom> Exits { get; set; }

    public bool TrapActive { get; set; }

    public Room(string name, string description, bool trapActive)
    {
      Name = name;
      Description = description;
      Items = new List<Item>();
      Exits = new Dictionary<string, IRoom>();
      TrapActive = trapActive;
    }

    public IRoom ChangeRoom(string exitStr)
    {
      IRoom nextRoom;
      if (Exits.TryGetValue(exitStr, out nextRoom))
      {
        return nextRoom;
      }
      else
      { // Will this break things? 
        return null;
      }
    }
  }
}