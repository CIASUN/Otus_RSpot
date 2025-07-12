public class PlaceValidationTests
{
    [Theory]
    [InlineData("������", true)]
    [InlineData("", false)]
    public void IsPlaceNameValid(string name, bool expected)
    {
        // ������ ������������� �������
        bool result = !string.IsNullOrWhiteSpace(name);
        Assert.Equal(expected, result);
    }
}
