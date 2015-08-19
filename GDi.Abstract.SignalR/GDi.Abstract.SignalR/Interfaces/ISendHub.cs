using GDi.Abstract.SignalR.DTO;

namespace GDi.Abstract.SignalR.Interfaces
{
    public interface ISendHub
    {
        void Send_AddMessage(string name, string message);

        void Send_Heartbeat();

        void Send_ExampleDTO(ExampleDTO exampleDto);
    }
}
