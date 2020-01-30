using ConsoleAdventure.Project.Interfaces;

namespace ConsoleAdventure.Project.Models
{
  public class Game : IGame
  {
    public IRoom CurrentRoom { get; set; }
    public IPlayer CurrentPlayer { get; set; }

    //NOTE Make yo rooms here...
    public void Setup()
    {
      // Create Rooms
      Room Room1 = new Room("Room1", "A cylindrical room containing your now-empty stasis pod, and an airlock with a view of a clouded planet (you assume it's earth).");

      Room Room2 = new Room("Room2", "Another cylindrical room containing a bank of computers, and what looks like several telescopes embedded in the wall.");
      Room Room3 = new Room("Room3", "Yet another cylindrical room, filled with what looks like the charred remains of various kinds of plants.");
      Room Room4 = new Room("Room4", "A cramped compartement containing three harnessess and a wide variety of switches and other control devices.");



      // Create Items
      Item airlock = new Item("Airlock", "A space door that opens to a vast, starry nothing with a planet below.", false, true);
      Item stasisPod = new Item("Stasis Pod", "The cryonic device that you woke up in; the door is now ajar.", false, true);
      Item fireExtinguisher = new Item("Fire Extingusher", "Extinguishes fires. In spaaaaaaace", true, false);
      Item telescopesComputers = new Item("Telescopes Console", "A bank of computers that look to interface with and control the telescopes in the room.", false, true);

      // Establish Exits
      Room1.Exits.Add("east", Room2);
      Room2.Exits.Add("east", Room3);
      Room2.Exits.Add("west", Room1);
      Room3.Exits.Add("east", Room4);
      Room3.Exits.Add("west", Room2);
      Room4.Exits.Add("west", Room3);

      Room1.Items.Add(airlock);
      Room1.Items.Add(stasisPod);
      Room1.Items.Add(fireExtinguisher);
      Room2.Items.Add(telescopesComputers);

      CurrentRoom = Room1;


    }

    public Game()
    {
      Setup();
    }
  }
}