using System.Collections.Generic;
using ConsoleAdventure.Project.Interfaces;

namespace ConsoleAdventure.Project.Models
{
  public class Item : IItem
  {
    public string Name { get; set; }
    public string Description { get; set; }

    public bool Takeable { get; set; }

    public bool CanUseWithoutTaking { get; set; }

    public Item(string name, string description, bool takeable, bool canUseWithoutTaking)
    {
      Name = name;
      Description = description;
      Takeable = takeable;
      CanUseWithoutTaking = canUseWithoutTaking;
    }
  }
}