using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sudoku.Views
{
    public sealed partial class GamePage : Page
    {
        Field[,] GameField;
        TextBox[] TextFieldArray;
        public GamePage()
        {
            this.InitializeComponent();
            //Sudoku grid 9x9 hodnot (Field)
            GameField = new Field[9, 9];
            //Sudoku grid TextBoxů
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
            //Generace gridu, při zapnutí aplikace
            for (int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                GameField[row, col] = new Field { Value = 0, Writable = false };
                TextFieldArray[i].Text = "";
                TextFieldArray[i].IsEnabled = GameField[row, col].Writable;
            }
            
        }
        //Pokud uživatel řeší problém a chce si ho zkontrolovat, tak touhle metodou se zapíšou do GameField jeho hodnoty
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
                if(TextFieldArray[i].IsEnabled == true && TextFieldArray[i].Text != "")
                {
                    try
                    {
                        GameField[row, col].Value = Convert.ToInt32(TextFieldArray[i].Text);
                    }
                    catch
                    {
                        ClearGameBoard(2);
                        var messageDialog = new MessageDialog("Zadané hodnoty neodpovídájí čislicím 1-9");
                        await messageDialog.ShowAsync();
                        return;
                    }
                }
            }
        }
        /*
         Metoda, která maže hodnoty
         mode 1 - vymažou se všechny hodnoty
         mdoe 2 - vymažou se pouze hodnoty zadaný uživatelem
         */
        public void ClearGameBoard(int mode)
        {
            for(int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                if (mode == 1)
                {
                    GameField[row, col].Value = 0;
                    GameField[row, col].Writable = false;
                    TextFieldArray[i].Text = "";
                    TextFieldArray[i].IsEnabled = false;
                }
                else
                {
                    if (GameField[row, col].Writable == true)
                    {
                        GameField[row, col].Value = 0;
                        TextFieldArray[i].Text = "";
                    }
                }
            }
            int temp = 0;
        }
        //Metoda, která kontroluje, zda je hrací pole plné
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
        /*
         Metoda co kontroluje zda je číslo možné zapsat do dané řady
         mode 1 - použito při generaci, kde číslo nesmí být obsaženo v řadě ani jednou, aby se mohlo zapsat
         mode 2 - použito při kontrole, kde číslo může být obsaženo v řadě pouze jednou, aby řešení bylo validní
        */
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
        /*
         Metoda co kontroluje zda je číslo možné zapsat do daného sloupce
         mode 1 - použito při generaci, kde číslo nesmí být obsaženo ve sloupci ani jednou, aby se mohlo zapsat
         mode 2 - použito při kontrole, kde číslo může být obsaženo ve sloupci pouze jednou, aby řešení bylo validní
        */
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
        /*
         Metoda co kontroluje zda je číslo možné zapsat do daného čtverce
         mode 1 - použito při generaci, kde číslo nesmí být obsaženo v čtverci ani jednou, aby se mohlo zapsat
         mode 2 - použito při kontrole, kde číslo může být obsaženo v čtverci pouze jednou, aby řešení bylo validní
        */
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
        //Metoda, která vygeneruje kompletně vyřešené sudoku a potom vymaže políčka, čím vznikne validní problém
        public int GenerateNewProblem()
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
                            GenerateNewProblem();
                            return 0;
                        }
                    }
                }
            }
            if (CheckGameBoard(GameField))
            {
                RemovingFields(45);
                return 1;
            }
            else
            {
                ClearGameBoard(1);
                GenerateNewProblem();
                return 0;
            }
        }
        //Metoda, která odstraňuje políčka, aby vznikl validní problém
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
        //Metoda, která zkontroluje, zda je problém úspěšně vyřešen
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
        //Metoda, která odstraní jednoznačné řešení políček
        public void Simplification()
        {
            bool notSolved = true;
            int squareCount = 1;
            int newFilledInValue = 0;
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
                    case 4:
                        a1 = 3;
                        a2 = 5;
                        b1 = 0;
                        b2 = 2;
                        break;
                    case 5:
                        a1 = 3;
                        a2 = 5;
                        b1 = 3;
                        b2 = 5;
                        break;
                    case 6:
                        a1 = 3;
                        a2 = 5;
                        b1 = 6;
                        b2 = 8;
                        break;
                    case 7:
                        a1 = 6;
                        a2 = 8;
                        b1 = 0;
                        b2 = 2;
                        break;
                    case 8:
                        a1 = 6;
                        a2 = 8;
                        b1 = 3;
                        b2 = 5;
                        break;
                    case 9:
                        a1 = 6;
                        a2 = 8;
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
                        if (GameField[j, k].Writable == true)
                        {
                            possibleCellValue[temp1, temp2] = new List<int>();
                            foreach (var pv in possibleSquareValues)
                            {
                                if (NotInRow(pv, j, GameField, 1))
                                {
                                    if (NotInCol(pv, k, GameField, 1))
                                    {
                                        possibleCellValue[temp1, temp2].Add(pv);
                                        //Info.Text += $"row: {j} coll: {k} = {pv}\n";
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
                //Otestování jednoznačné možnosti pro číslo
                foreach (var i in possibleSquareValues)
                {
                    temp1 = 0;
                    temp2 = 0;
                    var pvc = 0;
                    for (int j = a1; j <= a2; j++)
                    {
                        for (int k = b1; k <= b2; k++)
                        {
                            if (possibleCellValue[temp1, temp2] != null)
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
                                if (possibleCellValue[temp1, temp2] != null)
                                {
                                    foreach (var l in possibleCellValue[temp1, temp2])
                                    {
                                        if (l == i)
                                        {
                                            if (GameField[j, k].Value != i)
                                            {
                                                GameField[j, k].Value = i;
                                                GameField[j, k].Writable = false;
                                                newFilledInValue++;
                                            }
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
                //Otestování jednoznačné možnosti pro políčko
                temp1 = 0;
                temp2 = 0;
                for (int j = a1; j <= a2; j++)
                {
                    for (int k = b1; k <= b2; k++)
                    {
                        if (possibleCellValue[temp1, temp2] != null)
                        {
                            if (possibleCellValue[temp1, temp2].Count == 1)
                            {
                                if (GameField[j, k].Value != possibleCellValue[temp1, temp2][0])
                                {
                                    GameField[j, k].Value = possibleCellValue[temp1, temp2][0];
                                    GameField[j, k].Writable = false;
                                    newFilledInValue++;
                                }
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
                if (CheckGameBoard(GameField) == true)
                {
                    notSolved = false;
                }
                else if (squareCount == 9 && newFilledInValue == 0)
                {
                    notSolved = false;
                }
                else if (squareCount == 9 && newFilledInValue != 0)
                {
                    squareCount = 0;
                    newFilledInValue = 0;
                }
                squareCount++;
            }
        }
        //Metoda, která vygeneruje 2D pole, které bude obsahovat všechna možná řešení pro dané políčko
        public List<int>[,] getPossibilities()
        {
            if (CheckGameBoard(GameField) == false)
            {
                var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                var possibilities = new List<int>[9, 9];
                for(int i = 0; i < 9; i++)
                {
                    for(int j = 0; j < 9; j++)
                    {
                        if(GameField[i, j].Writable == true)
                        {
                            possibilities[i, j] = new List<int>();
                            foreach(var pv in numbers)
                            {
                                if(NotInSquare(pv, j, i, GameField, 1))
                                {
                                    if (NotInRow(pv, i, GameField, 1))
                                    {
                                        if (NotInCol(pv, j, GameField, 1))
                                        {
                                            possibilities[i, j].Add(pv);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return possibilities;
            }
            return null;
        }
        //Metoda, která vyřeší daný problém
        public void SolveCurrentProblem()
        {
            List<int>[,] numbers;
            Random rnd = new Random();
            for (int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                numbers = getPossibilities();
                if (GameField[row, col].Writable == true)
                {
                    if(numbers[row, col].Count > 0)
                    {
                        for (int j = 0; j < numbers[row, col].Count; j++)
                        {
                            var value = numbers[row, col][rnd.Next(numbers[row, col].Count)];
                            if (NotInRow(value, row, GameField, 1))
                            {
                                if (NotInCol(value, col, GameField, 1))
                                {
                                    if (NotInSquare(value, col, row, GameField, 1))
                                    {
                                        GameField[row, col].Value = value;
                                        GameField[row, col].Writable = true;
                                        TextFieldArray[i].Text = value.ToString();
                                    }
                                    else
                                    {
                                        numbers[row, col].Remove(value);
                                    }
                                }
                                else
                                {
                                    numbers[row, col].Remove(value);
                                }

                            }
                            else
                            {
                                numbers[row, col].Remove(value);
                            }
                        }
                    }
                    
                }
            }
            if (CheckGameBoard(GameField))
            {
                for (int i = 0; i < 81; i++)
                {
                    int row = i / 9;
                    int col = i % 9;
                    if (GameField[row, col].Writable == true)
                    {
                        GameField[row, col].Writable = false;
                        TextFieldArray[i].Text = GameField[row, col].Value.ToString();
                        TextFieldArray[i].IsEnabled = GameField[row, col].Writable;
                    }
                }
            }
            else
            {
                ClearGameBoard(2);
                SolveCurrentProblem();
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
            GenerateNewProblem();
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
                var messageDialog = new MessageDialog("Problém nebyl vyřešen úspěšně");
                await messageDialog.ShowAsync();
            }
        }
        private void SolveProblem(object sender, RoutedEventArgs e)
        {
            Simplification();
            if (!CheckGameBoard(GameField))
            {
                SolveCurrentProblem();
            }
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
