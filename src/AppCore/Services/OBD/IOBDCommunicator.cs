using OBD.NET.OBDData;

namespace AppCore.Services.OBD;

// ReSharper disable once InconsistentNaming
public interface IOBDCommunicator
{
    Task<T?> GetData<T>() where T : class, IOBDData, new();
}