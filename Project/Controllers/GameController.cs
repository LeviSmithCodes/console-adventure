using System;
using System.Collections.Generic;
using ConsoleAdventure.Project.Interfaces;
using ConsoleAdventure.Project.Models;

namespace ConsoleAdventure.Project.Controllers
{

  public class GameController : IGameController
  {
    public bool playing = true;
    private GameService _gameService = new GameService();

    //NOTE Makes sure everything is called to finish Setup and Starts the Game loop
    public void Run()
    {
      _gameService.Setup("Levi");
      _gameService.PrintStart();
      _gameService.PrintRoom();
      while (playing)
      {
        Print();
        GetUserInput();
      }
      Console.Clear();
      System.Console.WriteLine("Thanks for playing!");
    }

    //NOTE Gets the user input, calls the appropriate command, and passes on the option if needed.
    public void GetUserInput()
    {
      Console.WriteLine("\nWhat would you like to do?");
      string input = Console.ReadLine().ToLower() + " ";
      string command = input.Substring(0, input.IndexOf(" "));
      string option = input.Substring(input.IndexOf(" ") + 1).Trim();
      //NOTE this will take the user input and parse it into a command and option.
      //IE: take silver key => command = "take" option = "silver key"
      switch (command)
      {
        case "go":
          playing = _gameService.Go(option, playing);
          _gameService.PrintRoom();
          break;
        case "take":
          _gameService.TakeItem(option);
          break;
        case "inventory":
          _gameService.Inventory();
          break;
        case "look":
          _gameService.Look();
          break;
        case "quit":
          //Couldn't figure out how to pass to service
          // playing = false;
          playing = _gameService.Quit(playing);
          break;
        case "help":
          _gameService.Help();
          break;
        case "use":
          playing = _gameService.UseItem(option);
          break;
        default:
          System.Console.Clear();
          _gameService.Messages.Add("Invalid input.");
          break;
      }
    }

    //NOTE this should print your messages for the game.
    private void Print()
    {
      foreach (String message in _gameService.Messages)
      {
        System.Console.WriteLine(message);
      }
      _gameService.Messages.Clear();
    }
  }
}