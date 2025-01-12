using BookStoreApp.Application.Helpers;

namespace BookStoreApp.Helpers;

public class RandomHelper : IRandomHelper
{
    public int GenerateRandomNumber(int minValue, int maxValue) => new Random().Next(minValue, maxValue);
}