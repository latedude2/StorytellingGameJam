using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class CommandLineButton : MonoBehaviour
{
    TMP_InputField commandLine;
    TextMeshProUGUI commandScreen;
    Rover rover;
    float commandDelay = 2f;
    string command;

    // Start is called before the first frame update
    void Start()
    {
        rover = GameObject.FindWithTag("Rover").GetComponent<Rover>();

        commandLine = transform.parent.Find("InputField").GetComponent<TMP_InputField>();
        commandScreen = transform.parent.Find("PreviousCommands").GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendCommand();
        }
        //keep focus on the input field
        commandLine.Select();
        commandLine.caretWidth = 10;
        commandLine.ActivateInputField();
    }

    public void SendCommand(){
        PrintMessage("> " + commandLine.text);
        command = commandLine.text;
        commandLine.text = "";
        StartCoroutine(nameof(SendCommandWithDelay));
    }

    IEnumerator SendCommandWithDelay()
    {
        string myCommand = command;
        yield return new WaitForSeconds(commandDelay);
        if(myCommand != "")
        {
            InterpretMessage(myCommand);
        }
    }

    private void InterpretMessage(string message){
        string[] arguments = message.Split(' '); 
        switch (arguments[0]){
            case "move":
                if(CheckArgumentCount(arguments.Length, 2))
                {
                    if(int.TryParse(arguments[1], out _))
                    {
                        int x = Int32.Parse(arguments[1]);
                        if(x >= 0)
                        {
                            MoveCommand(x);
                        }
                        else
                        {
                            PrintMessage("< Cannot move backwards!");
                        }
                    }
                    else
                        PrintMessage("< Incorrect argument format!");
                }
                break;
            case "rotate":
                if(CheckArgumentCount(arguments.Length, 2))
                {
                    if(int.TryParse(arguments[1], out _))
                    {
                        RotateCommand(Int32.Parse(arguments[1]));
                    }
                    else 
                    {
                        PrintMessage("< Incorrect argument format!");
                    }   
                }                
                break;
            case "defend":
                if(CheckArgumentCount(arguments.Length, 1))
                    DefendCommand();
                break;
            case "scan":
                if(CheckArgumentCount(arguments.Length, 1))
                    ScanForMinerals();
                break;
            default:
                PrintMessage("< Command does not exist");
                break;
        }
        
    }

    private void MoveCommand(int x)
    {
        PrintMessage("< Moving rover " + x + " units forward");
        rover.Move(x);
    }

    void RotateCommand(int degrees)
    {
        PrintMessage("< Rotating " + degrees + " degrees!");
        rover.Rotate(degrees);
    }

    private void DefendCommand()
    {
        PrintMessage("< Rover defending.");
        rover.DefensiveStance();
    }
    //Unused
    private void ScanCommand(string scanTarget)
    {
        switch (scanTarget){
            case "lifeform":
                ScanForEnemies();
                break;
            case "water":
                ScanForMinerals();
                break;
            default:
                PrintMessage("< Unknown scan target!");
                break;
        }
    }

    void ScanForEnemies(){
        PrintMessage("< Rover scanning for lifeforms!");
        rover.ScanForEnemies();
    }

    void ScanForMinerals(){
        PrintMessage("< Rover scanning for water!");
        rover.ScanForWater();
    }

    private bool CheckArgumentCount(int actual, int expected)
    {
        if(expected < actual)
        {
            PrintMessage("< Too many arguments!");
            return false;
        }
        else if (expected > actual)
        {
            PrintMessage("< Missing arguments!");
            return false;
        }
        return true;
    }

    public void PrintMessage(string message)
    {
        commandScreen.text += "\n";
        commandScreen.text += message;
    }
}