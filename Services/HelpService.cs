using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using Windows.Media.Streaming.Adaptive;

namespace Sudoku.Services
{
    class HelpService
    {
        //Testování sloupce
        public bool CheckCol(int number, int col, Field[,] field, string mode)
        {
            int count = 0;

            for (int i = 0; i < 9; i++)
            {
                if (mode == "GENERATE")
                {
                    if (field[i, col].Value == number)
                    {
                        return false;
                    }
                }
                else
                {
                    if (field[i, col].Value == number)
                    {
                        count++;
                    }
                }

            }
            if (mode == "CHECK")
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
        //Testování řady
        public bool CheckRow(int number, int row, Field[,] FieldArray, string mode)
        {
            int count = 0;
            for (int i = 0; i < 9; i++)
            {
                if (mode == "GENERATE")
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
            if (mode == "CHECK")
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
        //Testování čtverce 3x3
        public bool CheckSquare(int number, Field cell, Field[,] field, string mode)
        {
            List<int> square = new List<int>();

            foreach (var i in field)
            {
                if (i.Square == cell.Square)
                {
                    square.Add(i.Value);
                }
            }
            if (mode == "GENERATE")
            {
                if (square.Contains(number))
                {
                    return false;
                }
            }
            else
            {
                var count = 0;
                foreach (var i in square)
                {
                    if (i == number)
                    {
                        count++;
                    }
                }
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
        //Vymazání pole
        public Field[,] ClearGameBoard(Field[,] field, string mode)
        {
            for (int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                if (mode == "GENERATE")
                {
                    field[row, col].Value = 0;
                    field[row, col].Writable = false;
                }
                if(mode == "CHECK")
                {
                    if (field[row, col].Writable == true)
                    {
                        field[row, col].Value = 0;
                    }
                }
            }
            return field;
        }
        //Testování zda je pole plné
        public bool CheckGameBoard(Field[,] field)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (field[i, j].Value == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //Vymazání políček aby vznikl validní problém
        public Field[,] RemovingFields(Field[,] field,int difficulty)
        {
            int row;
            int col;
            Random rnd = new Random();
            while (difficulty > 0)
            {
                row = rnd.Next(0, 9);
                col = rnd.Next(0, 9);
                while (field[row, col].Value == 0)
                {
                    row = rnd.Next(0, 9);
                    col = rnd.Next(0, 9);
                }
                field[row, col].Value = 0;
                field[row, col].Writable = true;
                difficulty -= 1;
            }
            return field;
        }
        //Odstranění jednoznačných řešení
        public Field[, ] Simplification(Field[,] field)
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
                        if (possibleSquareValues.Contains(field[j, k].Value))
                        {
                            possibleSquareValues.Remove(field[j, k].Value);
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
                        if (field[j, k].Writable == true)
                        {
                            possibleCellValue[temp1, temp2] = new List<int>();
                            foreach (var pv in possibleSquareValues)
                            {
                                if (CheckRow(pv, j, field, "GENERATE"))
                                {
                                    if (CheckCol(pv, k, field, "GENERATE"))
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
                                            if (field[j, k].Value != i)
                                            {
                                                field[j, k].Value = i;
                                                field[j, k].Writable = false;
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
                                if (field[j, k].Value != possibleCellValue[temp1, temp2][0])
                                {
                                    field[j, k].Value = possibleCellValue[temp1, temp2][0];
                                    field[j, k].Writable = false;
                                    newFilledInValue++;
                                }
                            }
                        }
                        temp2++;
                    }
                    temp2 = 0;
                    temp1++;
                }
                if (CheckGameBoard(field) == true)
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
            return field;
        }
        //Metoda, která pro každé políčko vytvoří seznam možných řešení
        public List<int>[,] getPossibilities(Field[,] field)
        {
            if (CheckGameBoard(field) == false)
            {
                var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                var possibilities = new List<int>[9, 9];
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (field[i, j].Writable == true)
                        {
                            possibilities[i, j] = new List<int>();
                            foreach (var pv in numbers)
                            {
                                if (CheckSquare(pv, field[i, j], field, "GENERATE"))
                                {
                                    if (CheckRow(pv, i, field, "GENERATE"))
                                    {
                                        if (CheckCol(pv, j, field, "GENERATE"))
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

    }
}
