namespace RandomNumbersApi.Services;

public interface IRandomService
{
    int NextIntNonNegative();
    int NextIntInclusive(int min, int max);
    double NextUnitIntervalDouble();
    string NextString(int length);
}