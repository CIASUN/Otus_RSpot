public class PlaceValidationTests
{
    [Theory]
    [InlineData("Москва", true)]
    [InlineData("", false)]
    public void IsPlaceNameValid(string name, bool expected)
    {
        // Пример валидационной функции
        bool result = !string.IsNullOrWhiteSpace(name);
        Assert.Equal(expected, result);
    }
}
