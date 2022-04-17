using System.Threading.Tasks;
using JAIS.Services.SystemService.Entities;

namespace JAIS.Services.SystemService;

public interface ISystemService
{
    public string ConfigDirectory { get; }
    public SystemConfig CurrentSystemConfig { get; set; }

    public Task Initialize();
    void ChangeTheme(bool dark);
}