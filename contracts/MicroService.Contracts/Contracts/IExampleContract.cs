namespace MicroService.Contracts;

public interface IExampleContract : IContract
{
    string? Value { get; set; }
}