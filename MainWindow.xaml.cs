using System;
using System.Linq;
using System.Windows;

namespace _4ISIP_121_Semeniac_Maria_Math
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Метод для решения методом северо-западного угла
        private void SolveNorthwestCorner_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int[] supply = ParseInput(SupplyTextBox.Text);
                int[] demand = ParseInput(DemandTextBox.Text);
                int[,] costMatrix = ParseCostMatrix();

                var result = SolveNorthwestCorner(supply, demand);
                DisplayResult(result, costMatrix);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // Метод для решения методом минимальной стоимости
        private void SolveMinimumCost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int[] supply = ParseInput(SupplyTextBox.Text);
                int[] demand = ParseInput(DemandTextBox.Text);
                int[,] costMatrix = ParseCostMatrix();

                var result = SolveMinimumCost(supply, demand, costMatrix);
                DisplayResult(result, costMatrix);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // Парсинг массива
        private int[] ParseInput(string input)
        {
            return input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse).ToArray();
        }

        // Парсинг матрицы стоимости
        private int[,] ParseCostMatrix()
        {
            string[] rows = CostMatrixTextBox.Text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int rowCount = rows.Length;
            int colCount = rows[0].Split(' ').Length;

            int[,] matrix = new int[rowCount, colCount];
            for (int i = 0; i < rowCount; i++)
            {
                int[] row = rows[i].Split(' ').Select(int.Parse).ToArray();
                for (int j = 0; j < colCount; j++)
                {
                    matrix[i, j] = row[j];
                }
            }

            return matrix;
        }

        // Отображение результата
        private void DisplayResult(int[,] result, int[,] costMatrix)
        {
            int totalCost = 0;
            ResultTextBox.Clear();

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    if (result[i, j] > 0)
                    {
                        int cost = result[i, j] * costMatrix[i, j];
                        totalCost += cost;
                        ResultTextBox.AppendText($"Перевезено {result[i, j]} ед. от Источника {i + 1} к Пункту {j + 1} (Стоимость: {cost})\n");
                    }
                }
            }

            ResultTextBox.AppendText($"\nОбщая стоимость: {totalCost}");
        }

        // Решение методом северо-западного угла
        private int[,] SolveNorthwestCorner(int[] supply, int[] demand)
        {
            int m = supply.Length, n = demand.Length;
            int[,] result = new int[m, n];

            int i = 0, j = 0;
            while (i < m && j < n)
            {
                if (supply[i] < demand[j])
                {
                    result[i, j] = supply[i];
                    demand[j] -= supply[i];
                    supply[i] = 0;
                    i++;
                }
                else
                {
                    result[i, j] = demand[j];
                    supply[i] -= demand[j];
                    demand[j] = 0;
                    j++;
                }
            }

            return result;
        }

        // Решение методом минимальной стоимости
        private int[,] SolveMinimumCost(int[] supply, int[] demand, int[,] costMatrix)
        {
            int m = supply.Length, n = demand.Length;
            int[,] result = new int[m, n];
            bool[,] used = new bool[m, n];

            while (supply.Any(s => s > 0) && demand.Any(d => d > 0))
            {
                int minCost = int.MaxValue, minI = -1, minJ = -1;

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (!used[i, j] && supply[i] > 0 && demand[j] > 0 && costMatrix[i, j] < minCost)
                        {
                            minCost = costMatrix[i, j];
                            minI = i;
                            minJ = j;
                        }
                    }
                }

                int quantity = Math.Min(supply[minI], demand[minJ]);
                result[minI, minJ] = quantity;
                supply[minI] -= quantity;
                demand[minJ] -= quantity;
                used[minI, minJ] = true;
            }

            return result;
        }
    }
}
