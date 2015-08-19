using GDi.Abstract.SignalR.DTO;

namespace GDi.Abstract.SignalR.Interfaces
{
    public interface IRecieveHub
    {
        void Recieve_AddMessage(string name, string message);

        void Recieve_Heartbeat();

        void Recieve_ExampleDTO(ExampleDTO exampleDto);
    }
}
