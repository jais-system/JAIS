using AppCore.Exceptions;
using AppCore.Services.CoreSystem;
using AppCore.Services.CoreSystem.Entities;
using OBD.NET.Communication;
using OBD.NET.Devices;
using OBD.NET.OBDData;

namespace AppCore.Services.OBD;

// ReSharper disable once InconsistentNaming
public class OBDCommunicator : IOBDCommunicator
{
    private readonly ELM327? _connection;

    public OBDCommunicator(IJaisSystem jaisSystem)
    {
        if (_connection == null)
        {
            string? serialPort = jaisSystem.Configuration.ObdSerialPort;
            
            if (serialPort == null)
            {
                throw new SystemSettingNotSetException(nameof(SystemConfig.ObdSerialPort));
            }
            
            var connection = new SerialConnection(serialPort);
            _connection = new ELM327(connection);
        }
    }

    public Task<T?> GetData<T>() where T : class, IOBDData, new()
    {
        return _connection == null
            ? Task.FromResult<T?>(null)
            : _connection.RequestDataAsync<T>();
    }
}