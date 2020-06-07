using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Chat;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System.Update;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Sudoku.Views
{
    public sealed partial class GamePage : Page
    {
        Field[,] GameField;
        TextBox[] TextFieldArray;
        int temp;
        public GamePage()
        {
            this.InitializeComponent();
            temp = 0;
            GameField = new Field[9, 9];
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
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    GameField[i, j] = new Field { Value = 0, Writable = false};
                    TextFieldArray[temp].Text = "";
                    TextFieldArray[temp].IsEnabled = GameField[i, j].Writable;
                    temp++;
                }
            }
        }
        //Fill GameField
        public async void FillGameField()
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
            for (int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                try
                {
                    GameField[row, col].Value = Convert.ToInt32(TextFieldArray[i].Text);
                }
                catch
                {
                    ClearGameBoard(1);
                    var messageDialog = new MessageDialog("Něco se pokazilo");
                    await messageDialog.ShowAsync();
                    return;
                }
                
            }
        }
        //Metoda pro vymazání všech uživatelem zadané hodnoty
        public void ClearGameBoard(int mode)
        {
            temp = 0;
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    if(mode == 1)
                    {
                        GameField[i, j].Value = 0;
                        GameField[i, j].Writable = false;
                        TextFieldArray[temp].Text = "";
                        TextFieldArray[temp].IsEnabled = false;
                        temp++;
                    }
                    else
                    {
                        if(GameField[i, j].Writable == true)
                        {
                            GameField[i, j].Value = 0;
                            TextFieldArray[temp].Text = "";
                            
                        }
                        temp++;
                    }
                }
            }
        }
        //Metoda co kontroluje zda je hrací pole plné
        public bool CheckGameBoard(Field[,] FieldArray)
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if(FieldArray[i,j].Value == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //Metoda co kontroluje zda je číslo možné zapsat do dané řady
        //Mode = 1 - Generate 2 - Check
        public bool NotInRow(int number, int row, Field[,] FieldArray,int mode)
        {
            int count = 0;
            for (int i = 0; i < 9; i++)
            {
                if(mode == 1)
                {
                    if (FieldArray[row, i].Value == number)
                    {
                        return false;
                    }
                }
                else
                {
                    if (FieldArray[row, i].Value == number)
                    {
                        count++;
                    }
                }
                
            }
            if(mode == 2)
            {
                if (count > 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
            
        }
        //Metoda co kontroluje zda je číslo možné zapsat do daného sloupce
        //Mode = 1 - Generate 2 - Check
        public bool NotInCol(int number, int col, Field[,] FieldArray, int mode)
        {
            int count = 0;
            for(int i = 0; i < 9; i++)
            {
                if(mode == 1)
                {
                    if (FieldArray[i, col].Value == number)
                    {
                        return false;
                    }
                }
                else
                {
                    if (FieldArray[i, col].Value == number)
                    {
                        count++;
                    }
                }
                
            }
            if (mode == 2)
            {
                if (count > 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
        //Metoda co kontroluje zda je číslo možné zapsat do daného čtverce
        public bool NotInSquare(int number, int col,int row, Field[,] FieldArray, int mode)
        {
            List<int> square = new List<int>();
            //První trojice
            if(row < 3)
            {
                if(col < 3)
                {
                    for (int m = 0; m < 3; m++)
                    {
                        for (int n = 0; n < 3; n++)
                        {
                            square.Add(FieldArray[m, n].Value);
                        }
                    }
                }
                else if(col < 6)
                {
                    for (int m = 0; m < 3; m++)
                    {
                        for (int n = 3; n < 6; n++)
                        {
                            square.Add(FieldArray[m, n].Value);
                        }
                    }
                }
                else
                {
                    for (int m = 0; m < 3; m++)
                    {
                        for (int n = 6; n < 9; n++)
                        {
                            square.Add(FieldArray[m, n].Value);
                        }
                    }
                }
            }
            //Druhá trojice
            else if (row < 6)
            {
                if (col < 3)
                {
                    for (int m = 3; m < 6; m++)
                    {
                        for (int n = 0; n < 3; n++)
                        {
                            square.Add(FieldArray[m, n].Value);
                        }
                    }
                }
                else if (col < 6)
                {
                    for (int m = 3; m < 6; m++)
                    {
                        for (int n = 3; n < 6; n++)
                        {
                            square.Add(FieldArray[m, n].Value);
                        }
                    }
                }
                else
                {
                    for (int m = 3; m < 6; m++)
                    {
                        for (int n = 6; n < 9; n++)
                        {
                            square.Add(FieldArray[m, n].Value);
                        }
                    }
                }
            }
            //Třetí trojice
            else
            {
                if (col < 3)
                {
                    for (int m = 6; m < 9; m++)
                    {
                        for (int n = 0; n < 3; n++)
                        {
                            square.Add(FieldArray[m, n].Value);
                        }
                    }
                }
                else if (col < 6)
                {
                    for (int m = 6; m < 9; m++)
                    {
                        for (int n = 3; n < 6; n++)
                        {
                            square.Add(FieldArray[m, n].Value);
                        }
                    }
                }
                else
                {
                    for (int m = 6; m < 9; m++)
                    {
                        for (int n = 6; n < 9; n++)
                        {
                            square.Add(FieldArray[m, n].Value);
                        }
                    }
                }
            }
            

            if(mode == 1)
            {
                if (square.Contains(number))
                {
                    return false;
                }
            }
            else
            {
                var count = 0;
                foreach(var i in square)
                {
                    if(i == number)
                    {
                        count++;
                    }
                }
                if(count > 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            
            return true;
        }
        //Metoda pro vytvoření kompletního sudoku
        public int GenerateNewProblem(int mode)
        {
            List<int> numbers;
            Random rnd = new Random();
            for (int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                if (GameField[row, col].Value == 0)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if(numbers.Count > 0)
                        {
                            var value = numbers[rnd.Next(numbers.Count)];
                            if (NotInRow(value, row, GameField, 1))
                            {
                                if (NotInCol(value, col, GameField, 1))
                                {
                                    if (NotInSquare(value, col, row, GameField, 1))
                                    {
                                        GameField[row, col] = new Field { Value = value, Writable = false };
                                        TextFieldArray[i].Text = value.ToString();
                                        TextFieldArray[i].IsEnabled = GameField[row, col].Writable;
                                    }
                                    else
                                    {
                                        numbers.Remove(value);
                                    }
                                }
                                else
                                {
                                    numbers.Remove(value);
                                }

                            }
                            else
                            {
                                numbers.Remove(value);
                            }
                        }
                        else
                        {
                            ClearGameBoard(2);
                            GenerateNewProblem(2);
                            return 0;
                        }
                    }
                }
            }
            if (mode == 1)
            {
                if (CheckGameBoard(GameField))
                {
                    RemovingFields(5); //Medium difficulty
                    return 1;
                }
                else
                {
                    ClearGameBoard(1);
                    GenerateNewProblem(1);
                    return 0;
                }
            }
            else
            {
                if (CheckGameBoard(GameField))
                {
                    return 1;
                }
                else
                {
                    ClearGameBoard(2);
                    GenerateNewProblem(2);
                    return 0;
                }
            }


        }
        //Metoda pro odstranění políček
        public void RemovingFields(int difficulty)
        {
            int row;
            int col;
            Random rnd = new Random();
            while (difficulty > 0)
            {
                row = rnd.Next(0, 9);
                col = rnd.Next(0, 9);
                while (GameField[row, col].Value == 0)
                {
                    row = rnd.Next(0, 9);
                    col = rnd.Next(0, 9);
                }
                GameField[row, col].Value = 0;
                GameField[row, col].Writable = true;
                difficulty -= 1;

            }
            for (int i = 0; i < 81; i++)
            {
                int r = i / 9;
                int c = i % 9;
                if(GameField[r, c].Value == 0)
                {
                    TextFieldArray[i].Text = "";
                    TextFieldArray[i].IsEnabled = true;
                }
            }
        }
        //Kontrola vyřešeného problému
        public bool CheckProblem()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if(GameField[i, j].Value != 0)
                    {
                        
                        if(NotInRow(GameField[i, j].Value,i, GameField, 2))
                        {
                            if (NotInCol(GameField[i, j].Value, j, GameField,2))
                            {
                               
                                if(NotInSquare(GameField[i, j].Value, j, i, GameField,2))
                                {
                                    continue;
                                }
                                else
                                {
                                    return false;
                                }
                                
                            }
                            else
                            {
                                return false;
                            }
                            
                        }
                        else
                        {
                            return false;
                        }
                      
                    }
                    
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        public async void SolveCurrentProblem()
        {
            bool notSolved = true;
            int squareCount = 1;
            int a1 = 0;
            int a2 = 0;
            int b1 = 0;
            int b2 = 0;

            while (notSolved)
            {
                switch (squareCount)
                {
                    case 1:
                        a1 = 0;
                        a2 = 2;
                        b1 = 0;
                        b2 = 2;
                        break;
                    case 2:
                        a1 = 0;
                        a2 = 2;
                        b1 = 3;
                        b2 = 5;
                        break;
                    case 3:
                        a1 = 0;
                        a2 = 2;
                        b1 = 6;
                        b2 = 8;
                        break;
                }
                //Možnosti pro čtverec
                var possibleSquareValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                for (int j = a1; j <= a2; j++)
                {
                    for (int k = b1; k <= b2; k++)
                    {
                        if (possibleSquareValues.Contains(GameField[j, k].Value))
                        {
                            possibleSquareValues.Remove(GameField[j, k].Value);
                        }
                    }
                }
                //Možnosti pro pole
                var possibleCellValue = new List<int>[3, 3];
                var temp1 = 0;
                var temp2 = 0;
                for (int j = a1; j <= a2; j++)
                {
                    for (int k = b1; k <= b2; k++)
                    {
                        if(GameField[j, k].Writable == true)
                        {
                            possibleCellValue[temp1, temp2] = new List<int>();
                            foreach (var pv in possibleSquareValues)
                            {
                                if (NotInRow(pv, j, GameField, 1))
                                {
                                    if (NotInCol(pv, k, GameField, 1))
                                    {
                                        possibleCellValue[temp1, temp2].Add(pv);
                                        Info.Text += $"row: {j} coll: {k} = {pv}\n";
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        temp2++;
                    }
                    temp2 = 0;
                    temp1++;
                }
                //Otestovani zda nekde je pouze 1 možnost pro čísla
                foreach(var i in possibleSquareValues)
                {
                    temp1 = 0;
                    temp2 = 0;
                    var pvc = 0;
                    for (int j = a1; j <= a2; j++)
                    {
                        for (int k = b1; k <= b2; k++)
                        {
                            if(possibleCellValue[temp1, temp2] != null)
                            {
                                foreach (var l in possibleCellValue[temp1, temp2])
                                {
                                    if (l == i)
                                    {
                                        pvc++;
                                    }
                                }
                            }
                            temp2++;
                        }
                        temp1++;
                        temp2 = 0;
                    }
                    temp1 = 0;
                    temp2 = 0;
                    if (pvc == 1)
                    {
                        for (int j = a1; j <= a2; j++)
                        {
                            for (int k = b1; k <= b2; k++)
                            {
                                if(possibleCellValue[temp1, temp2] != null)
                                {
                                    foreach (var l in possibleCellValue[temp1, temp2])
                                    {
                                        if (l == i)
                                        {
                                            GameField[j, k].Value = i;
                                            GameField[j, k].Writable = false;
                                        }
                                    }
                                }
                                temp2++;
                            }
                            temp2 = 0;
                            temp1++;
                        }
                    }
                }
                //Otestovaní zda nekde je pouze 1 možnost pro políčka
                temp1 = 0;
                temp2 = 0;
                for (int j = a1; j <= a2; j++)
                {
                    for (int k = b1; k <= b2; k++)
                    {
                        if(possibleCellValue[temp1,temp2] != null)
                        {
                            if (possibleCellValue[temp1, temp2].Count == 1)
                            {
                                GameField[j, k].Value = possibleCellValue[temp1, temp2][0];
                                GameField[j, k].Writable = false;
                            }
                        }
                        temp2++;
                    }
                    temp2 = 0;
                    temp1++;
                }
                //Zapsání do pole
                for (int i = 0; i < 81; i++)
                {
                    int row = i / 9;
                    int col = i % 9;

                    TextFieldArray[i].Text = GameField[row, col].Value.ToString();
                    TextFieldArray[i].IsEnabled = GameField[row, col].Writable;

                }
                if(squareCount == 3)
                {
                   notSolved = false;
                }
                squareCount++;


            }
        }



        private void MenuButton(object sender, RoutedEventArgs e)
        {
            if (MenuSplitView.IsPaneOpen == false)
            {
                MenuSplitView.IsPaneOpen = true;
            }
            else if(MenuSplitView.IsPaneOpen == true)
            {
                MenuSplitView.IsPaneOpen = false;
            }

        }
        private void ClearGameBoard(object sender, RoutedEventArgs e)
        {
            ClearGameBoard(2);
        }
        private void NewProblem(object sender, RoutedEventArgs e)
        {
            ClearGameBoard(1);
            GenerateNewProblem(1);
        }
        private async void Check(object sender, RoutedEventArgs e)
        {
            FillGameField();
            if (CheckProblem())
            {
                var messageDialog = new MessageDialog("Úspěšně jste vyřešil problém");
                await messageDialog.ShowAsync();
            }
            else
            {
                var messageDialog = new MessageDialog("Problěm nebyl vyřešen úspěšně");
                await messageDialog.ShowAsync();
            }
        }
        private void SolveProblem(object sender, RoutedEventArgs e)
        {
            //GenerateNewProblem(2);
            SolveCurrentProblem();
        }
        
        private async void UploadTextFile(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".txt");

            StorageFile file = await openPicker.PickSingleFileAsync();

            if(file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read); using
                (StreamReader reader = new StreamReader(stream.AsStream()))
                {
                    string line;
                    int lineCounter = 0;
                    int textCounter = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            if(!(lineCounter > 8))
                            {
                                var numbers = line.Split(" ", StringSplitOptions.None);
                                var numberCheckList = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                                foreach (var a in numbers)
                                {
                                    if(numbers.Length > 10)
                                    {
                                        ClearGameBoard(1);
                                        var messageDialog = new MessageDialog("Soubor obsahuje neplatný počet znaků");
                                        await messageDialog.ShowAsync();
                                        return;
                                    }
                                    try
                                    { 
                                        
                                        if (a != "")
                                        {
                                            var cell = Convert.ToInt32(a);
                                            if (numberCheckList.Contains((int)cell))
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                throw new SystemException();
                                            }
                                        }  
                                        
                                    }
                                    catch
                                    {
                                        ClearGameBoard(1);
                                        var messageDialog = new MessageDialog("Soubor obsahuje neplatné znaky, prosím zadejte čísla v rozsahu 0-9");
                                        await  messageDialog.ShowAsync();
                                        return;
                                    }

                                }
                                if (numbers[i] != "0")
                                {
                                    GameField[lineCounter, i].Value = Convert.ToInt32(numbers[i]);
                                    GameField[lineCounter, i].Writable = false;
                                    TextFieldArray[textCounter].Text = numbers[i];
                                    TextFieldArray[textCounter].IsEnabled = GameField[lineCounter, i].Writable;
                                }
                                else
                                {
                                    GameField[lineCounter, i].Value = 0;
                                    GameField[lineCounter, i].Writable = true;
                                    TextFieldArray[textCounter].Text = "";
                                    TextFieldArray[textCounter].IsEnabled = GameField[lineCounter, i].Writable;
                                }
                            }
                            else
                            {
                                ClearGameBoard(1);
                                var messageDialog = new MessageDialog("Soubor obsahuje moc velký počet řádků");
                                await messageDialog.ShowAsync();
                                return;
                            }
                            textCounter++;
                        }
                        lineCounter++;
                    }
                }
            }
            
        }

        private async void SaveTextFile(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "New Document";
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
    }
}
