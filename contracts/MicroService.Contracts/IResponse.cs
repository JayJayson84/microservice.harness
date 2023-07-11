namespace MicroService.Contracts;

public interface IResponse
{
    int ResponseCode { get; set; }
    string? ResponseType { get; set; }
    string? Message { get; set; }
}