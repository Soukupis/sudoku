using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Sudoku.Services
{
    class GameService
    {
        private HelpService helpService;
        private Field[,] field;
        public GameService()
        {
            helpService = new HelpService();
            field = initializeGameField();
        }
        //Vytvoření pole field
        public Field[,] initializeGameField()
        {
            field = new Field[9, 9];
            for (int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                int sqr = 0;
                if (row >= 0 && row <= 3)
                {
                    sqr = col / 3;
                }
                if (row >= 3 && row <= 6)
                {
                    sqr = (col / 3) + 3;
                }
                if (row >= 6 && row <= 9)
                {
                    sqr = (col / 3) + 6;
                }
                field[row, col] = new Field { Value = 0, Writable = false, Square = sqr };
            }
            return field;
        }
        //Zpřístupnění pole field
        public Field[,] getField()
        {
            return field;
        }
        //Metoda, která generuje nový problém
        public void generateNewProblem()
        {
            List<int> numbers;
            Random rnd = new Random();
            for (int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                if (field[row, col].Value == 0)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (numbers.Count > 0)
                        {
                            var value = numbers[rnd.Next(numbers.Count)];
                            if (helpService.CheckRow(value, row, field, "GENERATE"))
                            {
                                if (helpService.CheckCol(value, col, field, "GENERATE"))
                                {
                                    if (helpService.CheckSquare(value, field[row, col], field, "GENERATE"))
                                    {
                                        field[row, col] = new Field { Value = value, Writable = false, Square = field[row, col].Square };
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
                            field = helpService.ClearGameBoard(field, "CHECK");
                            generateNewProblem();
                        }
                    }
                }
            }
            if (helpService.CheckGameBoard(field))
            {
                field = helpService.RemovingFields(field, 45);
            }
            else
            {
                field = helpService.ClearGameBoard(field, "GENERATE");
                generateNewProblem();
            }
        }
        //Metoda, která zkontroluje, zda je řešení zprávné
        public bool checkProblem(Field[,] field)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (field[i, j].Value != 0)
                    {
                        if (helpService.CheckRow(field[i, j].Value, i, field, "CHECK"))
                        {
                            if (helpService.CheckCol(field[i, j].Value, j, field, "CHECK"))
                            {

                                if (helpService.CheckSquare(field[i, j].Value, field[i, j], field, "CHECK"))
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
        //Metoda, která vyřeší daný problém
        public void solveCurrentProblem(Field[,] field)
        {
            List<int>[,] numbers;
            Random rnd = new Random();
            for (int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                numbers = helpService.getPossibilities(field);
                if (field[row, col].Writable == true)
                {
                    if (numbers[row, col].Count > 0)
                    {
                        for (int j = 0; j < numbers[row, col].Count; j++)
                        {
                            var value = numbers[row, col][rnd.Next(numbers[row, col].Count)];
                            if (helpService.CheckRow(value, row, field, "GENERATE"))
                            {
                                if (helpService.CheckCol(value, col, field, "GENERATE"))
                                {
                                    if (helpService.CheckSquare(value, field[row, col], field, ""))
                                    {
                                        field[row, col].Value = value;
                                        field[row, col].Writable = true;
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
            if (helpService.CheckGameBoard(field))
            {
                for (int i = 0; i < 81; i++)
                {
                    int row = i / 9;
                    int col = i % 9;
                    if (field[row, col].Writable == true)
                    {
                        field[row, col].Writable = false;
                        this.field = field;
                    }
                }
            }
            else
            {
                helpService.ClearGameBoard(field, "CHECK");
                solveCurrentProblem(field);
            }

        }

    }
}
