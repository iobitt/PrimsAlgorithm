using System;
using System.IO;
using System.Collections.Generic;

namespace PrimsAlgorithm
{
    class Program
    {
        // Точка входа приложения
        static void Main(string[] args)
        {
            Int16 inputMethod = Welcome();
            Int16[,] matrix = null;
            switch (inputMethod)
            {
                case 1:
                    matrix = ConsoleInput();
                    break;
                case 2:
                    matrix = FileInput();
                    break;
            }
            int rows = matrix.GetUpperBound(0) + 1;
            Int16 Vnum = Convert.ToInt16(matrix.Length / rows);
            Int16[,] newMatrix = PrimsAlgorithm(matrix, Vnum);
            Console.WriteLine("\nМатрица, получившаяся в результате работы алгоритма прима:");
            PrintWeightMatrix(newMatrix, Vnum);
            Console.WriteLine("\nПрограмма завершила свою работу. Для продолжения нажмите любую клавишу...");
            Console.ReadKey();
        }

        // Функция начала работы программы
        static Int16 Welcome()
        {
            Console.WriteLine("Для работы программы необходмо указать способ ввода параметров графа:");
            Console.WriteLine("1.Через консоль");
            Console.WriteLine("2.Из файла");
            Console.WriteLine("Укажите способ ввода: ");
            Int16 inputMethod = 0;
            while(inputMethod == 0)
            {
                try
                {
                    inputMethod = Convert.ToInt16(Console.ReadLine());
                    if (inputMethod != 1 && inputMethod != 2)
                    {
                        inputMethod = 0;
                        Console.WriteLine("Ошибка! Необходимо было ввести одну цифру: 1 или 2!");
                        Console.WriteLine("Укажите способ ввода: ");
                    }
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Ошибка! Необходимо было ввести одну цифру: 1 или 2!");
                    Console.WriteLine("Укажите способ ввода: ");
                }
                catch (System.OverflowException ex)
                {
                    Console.Write("Ошибка! ");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Укажите способ ввода: ");
                }
            }
            return inputMethod;
        }

        // Ввод данных пользователем через консоль
        static Int16[,] ConsoleInput()
        {
            Console.WriteLine("Отлично! Введём данные через консоль.");
            Console.WriteLine("Укажите число вершин графа: ");
            Int16 VNum = 0;
            while (VNum == 0)
            {
                try
                {
                    VNum = Convert.ToInt16(Console.ReadLine());
                    if (VNum <= 1)
                    {
                        VNum = 0;
                        Console.WriteLine("Ошибка! Вершин должно быть не меньше двух!");
                        Console.WriteLine("Укажите число вершин графа: ");
                    }
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Ошибка! Вершин должно быть не меньше двух!");
                    Console.WriteLine("Укажите число вершин графа: ");
                }
                catch (System.OverflowException ex)
                {
                    Console.Write("Ошибка! ");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Укажите число вершин графа: ");
                }
            }
            Int16[,] matrix = new Int16[VNum, VNum];
            matrix = FillWeightMatrix(matrix, VNum);
            return matrix;
        }

        // Ввод данных от пользователя из файла
        static Int16[,] FileInput()
        {
            Console.WriteLine("Отлично! Введём данные из файла.");
            Int16[,] matrix = null;
            Int16 VNum = 0;
            bool goodPath = false;
            while(!goodPath)
            {
                try
                {
                    Console.Write("Введите имя файла: ");
                    string path = Console.ReadLine();
                    using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                    {
                        string line = sr.ReadLine();
                        if (line != null)
                        {
                            VNum = Convert.ToInt16(line);
                            matrix = new Int16[VNum, VNum];
                            for (int i = 0; i < VNum; i++)
                            {
                                line = sr.ReadLine();
                                string[] words = line.Split();
                                for (int j = 0; j < words.Length; j++)
                                {
                                    matrix[i, j] = Convert.ToInt16(words[j]);
                                }
                            }
                        }
                    }
                    goodPath = true;
                    PrintWeightMatrix(matrix, VNum);
                }
                catch (System.IO.FileNotFoundException)
                {
                    Console.Write("Ошибка! Файл не найден!\n");
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Ошибка! Файл имеет неверный формат!");
                }
                catch (System.OverflowException ex)
                {
                    Console.Write("Ошибка! ");
                    Console.WriteLine(ex.Message);
                }
                catch(System.NullReferenceException)
                {
                    Console.WriteLine("Ошибка! Файл имеет неверный формат!");
                }
                catch (System.IndexOutOfRangeException)
                {
                    Console.WriteLine("Ошибка! Файл имеет неверный формат!");
                }

            }
            return matrix;
        }

        // Заполняет матрицу, содержащую веса рёбер
        static Int16[,] FillWeightMatrix(Int16[,] matrix, Int16 VNum)
        {
            bool MatrixFilledCorrectly = false;
            while (!MatrixFilledCorrectly)
            {
                Console.WriteLine("\nЗаполним матрицу весов рёбер:");
                for (int i = 0; i < VNum - 1; i++)
                {
                    for (int j = i + 1; j < VNum; j++)
                    {
                        Int16 item = -1;
                        while (item == -1)
                        {
                            try
                            {
                                Console.Write("v" + (i + 1) + "v" + (j + 1) + " = ");
                                item = Convert.ToInt16(Console.ReadLine());
                                if (item < 0)
                                {
                                    item = -1;
                                    Console.WriteLine("Ошибка! Необходимо было ввести не отрицательное число!");
                                }
                            }
                            catch (System.FormatException)
                            {
                                Console.WriteLine("Ошибка! Необходимо было ввести не отрицательное число!");
                            }
                            catch (System.OverflowException ex)
                            {
                                Console.Write("Ошибка! ");
                                Console.WriteLine(ex.Message);
                            }
                        }
                        matrix[i, j] = item;
                        matrix[j, i] = item;
                    }
                    Console.WriteLine();
                }
                PrintWeightMatrix(matrix, VNum);
                string answer = "";
                while (answer == "")
                {
                    Console.WriteLine("\nМатрица заполнена верно?(да/нет)");
                    answer = Console.ReadLine();
                    if (answer != "да" && answer != "нет")
                    {
                        answer = "";
                    }
                    if (answer == "да")
                    {
                        MatrixFilledCorrectly = true;
                    }
                }
            }
            return matrix;
        }

        // Вывести матрицу весов рёбер
        static Int16[,] PrintWeightMatrix(Int16[,] matrix, Int16 VNum)
        {
            Console.WriteLine("\nМатрица весов рёбер:");
            for (int i = 0; i < VNum; i++)
            {
                for (int j = 0; j < VNum; j++)
                {
                    Console.Write(matrix[i, j]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
            return matrix;
        }

        // Реализует Алгоритм Прима
        static Int16[,] PrimsAlgorithm(Int16[,] matrix, Int16 VNum)
        {
            // Матрица смежности графа, полученного из исходного с помощью алгоритма Прима
            Int16[,] newMatrix = new Int16[VNum, VNum];
            // Упорядоченное множество вершин нового графа
            SortedSet<Int16> selectedVertices = new SortedSet<Int16>();
            // Добавим первую вершину в новый граф - вершина v0
            selectedVertices.Add(0);

            // Данный цикл считает количество добавленных рёбер в новом графе
            // Их должно быть ровно на единицу меньше, чем число вершин исходного графа
            for (int k = 0; k < VNum-1; k++)
            {
                // Минимальный вес в матрице весов
                Int16 min = -1;
                // Индексы min: matrix[i, j]
                Int16 minI = -1;
                Int16 minJ = -1;
                // Идём по строчкам матрицы весов
                for (int i = 0; i < VNum; i++)
                {
                    // Если вершина не добалена в новый граф, то пропускаем эту строку == пропускаем итерацию цикла с i
                    if (!selectedVertices.Contains(Convert.ToInt16(i)))
                    {
                        continue;
                    }
                    // Идём по столбцам матрицы весов
                    for (int j = 0; j < VNum; j++)
                    {
                        // Если новый граф уже содержит данную вершину, то нет смысла её рассматривать
                        // То есть пропускаем этот столбец == пропускаем итерацию цикла с j
                        if (selectedVertices.Contains(Convert.ToInt16(j)))
                        {
                            continue;
                        }
                        // Если значение данной ячейки = 0, значит ребра между этими вершинами нет
                        // То есть пропускаем этот столбец == пропускаем итерацию цикла с j
                        if (matrix[i, j] == 0)
                        {
                            continue;
                        }
                        // Если min = -1, значит min не равен ни одному реальному значению
                        // Присваиваем min значение текущей ячейки
                        if (min == -1)
                        {
                            min = matrix[i, j];
                            minI = Convert.ToInt16(i);
                            minJ = Convert.ToInt16(j);
                        }
                        // Если мы нашли значение меньше, чем min, то min присваиваем это значение
                        if (matrix[i, j] < min)
                        {
                            min = matrix[i, j];
                            minI = Convert.ToInt16(i);
                            minJ = Convert.ToInt16(j);
                        }
                    }
                }
                // Добавляем в новую матрицу найденное минимальное ребро
                newMatrix[minI, minJ] = min;
                newMatrix[minJ, minI] = min;
                // Добавляем новую вершину в множество вершин нового графа
                selectedVertices.Add(minJ);

                Console.WriteLine("Добавили новый элемент v" + (minJ + 1) + ", вес ребра равен: " + min);
            }
            // Возвращаем полученную матрицу
            return newMatrix;
        }
    }
}