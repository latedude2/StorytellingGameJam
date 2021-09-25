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
        commandLine.ActivateInputField();
    }

    public void SendCommand(){
        PrintMessage(commandLine.text);
        command = commandLine.text;
        commandLine.text = "";
        StartCoroutine(nameof(SendCommandWithDelay));
    }

    IEnumerator SendCommandWithDelay()
    {
        yield return new WaitForSeconds(commandDelay);
        if(command != "")
        {
            InterpretMessage(command);
        }
    }

    private void InterpretMessage(string message){
        string[] arguments = message.Split(' '); 
        switch (arguments[0]){
            case "move":
                if(CheckArgumentCount(arguments.Length, 3))
                {
                    if(int.TryParse(arguments[1], out _) & int.TryParse(arguments[2], out _))
                    {
                        int x = Int32.Parse(arguments[1]);
                        int y = Int32.Parse(arguments[2]);
                        MoveCommand(x, y);
                    }
                    else
                        PrintMessage("Incorrect argument format!");
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
                        PrintMessage("Incorrect argument format!");
                    }   
                }                
                break;
            case "defend":
                if(CheckArgumentCount(arguments.Length, 1))
                    DefendCommand();
                break;
            case "scan":
                if(CheckArgumentCount(arguments.Length, 2))
                    ScanCommand(arguments[1]);
                break;
            default:
                PrintMessage("Command does not exist");
                break;
        }
        
    }

    private void MoveCommand(int x, int y)
    {
        PrintMessage("Moving rover to: (" + x +  "," + y + ")");
        
    }

    void RotateCommand(int degrees)
    {
        PrintMessage("Rotating " + degrees + " degrees!");
    }

    private void DefendCommand()
    {
        PrintMessage("Rover defending.");
    }

    private void ScanCommand(string scanTarget)
    {
        switch (scanTarget){
            case "enemy":
                ScanForEnemies();
                break;
            case "mineral":
                ScanForMinerals();
                break;
            default:
                PrintMessage("Unknown scan target!");
                break;
        }
    }

    void ScanForEnemies(){
        PrintMessage("Rover Scanning for enemies!");
    }

    void ScanForMinerals(){
        PrintMessage("Rover Scanning for minerals!");
    }

    private bool CheckArgumentCount(int actual, int expected)
    {
        if(expected < actual)
        {
            PrintMessage("Too many arguments!");
            return false;
        }
        else if (expected > actual)
        {
            PrintMessage("Missing arguments!");
            return false;
        }
        return true;
    }

    private void PrintMessage(string message)
    {
        commandScreen.text += "\n";
        commandScreen.text += message;
    }
}