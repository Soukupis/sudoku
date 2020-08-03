using Sudoku.Models;
using Sudoku.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using muxc = Microsoft.UI.Xaml.Controls;

namespace Sudoku.Views
{
    public sealed partial class GamePage : Page
    {
        private Field[,] GameField;
        private TextBox[] TextFieldArray;
        private GameService gameService;
        private HelpService helpService;
        public GamePage()
        {
            this.InitializeComponent();
            gameService = new GameService();
            helpService = new HelpService();
            GameField = gameService.initializeGameField();
            InitializeTextFieldArray();
        }
        public void InitializeTextFieldArray()
        {
            TextFieldArray = new TextBox[]
            {
                x0y0, x0y1, x0y2, x0y3, x0y4, x0y5, x0y6,x0y7, x0y8,
                x1y0, x1y1, x1y2, x1y3, x1y4, x1y5, x1y6,x1y7, x1y8,
                x2y0, x2y1, x2y2, x2y3, x2y4, x2y5, x2y6,x2y7, x2y8,
                x3y0, x3y1, x3y2, x3y3, x3y4, x3y5, x3y6,x3y7, x3y8,
                x4y0, x4y1, x4y2, x4y3, x4y4, x4y5, x4y6,x4y7, x4y8,
                x5y0, x5y1, x5y2, x5y3, x5y4, x5y5, x5y6,x5y7, x5y8,
                x6y0, x6y1, x6y2, x6y3, x6y4, x6y5, x6y6,x6y7, x6y8,
                x7y0, x7y1, x7y2, x7y3, x7y4, x7y5, x7y6,x7y7, x7y8,
                x8y0, x8y1, x8y2, x8y3, x8y4, x8y5, x8y6,x8y7, x8y8
            };
            UpdateGameField("ToText");
        }
        //GAME BUTTONS
        private void ClearGameBoard(object sender, RoutedEventArgs e)
        {
            GameField = helpService.ClearGameBoard(GameField, "CHECK");
            UpdateGameField("ToText");
        }
        private void NewProblem(object sender, RoutedEventArgs e)
        {
            gameService.generateNewProblem();
            UpdateGameField("ToText");
        }
        private async void Check(object sender, RoutedEventArgs e)
        {
            UpdateGameField("ToArray");
            if (gameService.checkProblem(GameField))
            { 
                var messageDialog = new MessageDialog("Úspěšně jste vyřešil problém");
                await messageDialog.ShowAsync();
            }
            else
            {
                var messageDialog = new MessageDialog("Problém nebyl vyřešen úspěšně");
                await messageDialog.ShowAsync();
            }
        }
        private void SolveProblem(object sender, RoutedEventArgs e)
        {
            GameField = helpService.Simplification(GameField);
            if (!helpService.CheckGameBoard(GameField))
            {
                gameService.solveCurrentProblem(GameField);
                GameField = gameService.getField();
            }
            UpdateGameField("ToText");
        }
        //FILE BUTTONS
        private async void UploadTextFile(object sender, RoutedEventArgs e)
        {
            
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".txt");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read); using
                (StreamReader reader = new StreamReader(stream.AsStream()))
                {
                    string line;
                    int lineCounter = 0;
                    int textCounter = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        try
                        {
                            var numbers = line.Split(" ", StringSplitOptions.None);
                            for (int i = 0; i < numbers.Length; i++)
                            {
                                if (numbers[i] != "0")
                                {
                                    GameField[lineCounter, i].Value = Convert.ToInt32(numbers[i]);
                                    GameField[lineCounter, i].Writable = false;
                                }
                                else
                                {
                                    GameField[lineCounter, i].Value = 0;
                                    GameField[lineCounter, i].Writable = true;
                                }
                                textCounter++;
                            }
                            lineCounter++;
                        }
                        catch
                        {
                            var messageDialog = new MessageDialog("Nastal problém s formátem vašeho dokumentu");
                            await messageDialog.ShowAsync();
                        }
                    }
                }
            }
            UpdateGameField("ToText");
        }
        private async void SaveTextFile(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "SudokuProblem";
            string sudokuString = "";
            int counter = 0;
            for(int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                if (counter % 9 == 8)
                {
                    sudokuString += $"{GameField[row, col].Value}\n";
                }
                else
                {
                    sudokuString += $"{GameField[row, col].Value} ";
                }
                counter++;
            }
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                await FileIO.WriteTextAsync(file, sudokuString);
            }
        }
        //MENU
        private void MenuButton(object sender, RoutedEventArgs e)
        {
            if (MenuSplitView.IsPaneOpen == false)
            {
                MenuSplitView.IsPaneOpen = true;
            }
            else if (MenuSplitView.IsPaneOpen == true)
            {
                MenuSplitView.IsPaneOpen = false;
            }
        }
        //UPDATE GAME FIELD
        private async void UpdateGameField(string mode)
        {
            for (int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                if(mode == "ToText")
                {
                    try
                    {
                        if (GameField[row, col].Value == 0)
                        {
                            TextFieldArray[i].Text = "";
                        }
                        else
                        {
                            TextFieldArray[i].Text = GameField[row, col].Value.ToString();
                        }
                        TextFieldArray[i].IsEnabled = GameField[row, col].Writable;
                    }
                    catch
                    {
                        helpService.ClearGameBoard(GameField, "CHECK");
                        var messageDialog = new MessageDialog("Zadané hodnoty neodpovídájí čislicím 1-9");
                        await messageDialog.ShowAsync();
                    }
                }
                if(mode == "ToArray")
                {
                    try
                    {
                        if(TextFieldArray[i].IsEnabled == true)
                        {
                            GameField[row, col].Value = Convert.ToInt32(TextFieldArray[i].Text);
                        }
                    }
                    catch
                    {
                        helpService.ClearGameBoard(GameField, "CHECK");
                        var messageDialog = new MessageDialog("Zadané hodnoty neodpovídájí čislicím 1-9");
                        await messageDialog.ShowAsync();
                    }
                }
            }
        }
    }
}
