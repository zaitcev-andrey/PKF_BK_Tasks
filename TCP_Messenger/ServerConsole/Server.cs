using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerClient
{
    public class Server
    {
        #region private Members
        // создаём серверный сокет для прослушивания
        private Socket serverSocket;
        // и список всех клиентских сокетов
        private List<Socket> clients;
        #endregion

        #region Constructors
        public Server()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8080);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // связываем сокет с локальной точкой ipPoint
            serverSocket.Bind(ipPoint);

            clients = new List<Socket>();
        }
        #endregion

        #region public Methods
        /// <summary>
        /// Основной метод для прослушивания клиентов
        /// </summary>
        /// <returns></returns>
        public async Task ListenAsync()
        {
            try
            {
                serverSocket.Listen(1000);
                Console.WriteLine("Сервер запущен, ожидается подключение клиентов...");

                while (true)
                {
                    // получаем клиента после выполнения у него метода Connect(host, port)
                    Socket clientSocket = await serverSocket.AcceptAsync();

                    clients.Add(clientSocket);
                    Task.Run(() => ChatAsync(clientSocket));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Метод, который выполняется после закрытия сервера
        /// </summary>
        public void Disconnect()
        {
            // сначала закрываем всех клиентов
            foreach (var client in clients)
            {
                client.Close();
            }
            // затем останавливаем сервер
            serverSocket.Close();
        }
        #endregion

        #region private Methods
        /// <summary>
        /// Метод для получения данных от клиента и рассылки этих данных всем остальным клиентам 
        /// кроме того, от которого данные были получены
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <returns></returns>
        private async Task ChatAsync(Socket clientSocket)
        {
            string userName = "";
            try
            {
                var nameBytes = new byte[512];
                int bytes = clientSocket.Receive(nameBytes);
                userName = Encoding.UTF8.GetString(nameBytes, 0, bytes);

                string message = $"{userName} вошёл в чат";

                // Посылаем сообщение о входе нового пользователя в чат всем подключенным клиентам
                await BroadcastMessageAsync(message, clientSocket.RemoteEndPoint);
                // Отражаем сообщение на сервере
                Console.WriteLine(message);

                // Запускаем цикл на получение данных от клиента
                while (true)
                {
                    var clientData = new byte[1024 * 10000]; // до 10мб
                    int receivedByteLen = clientSocket.Receive(clientData);

                    try
                    {
                        if (receivedByteLen > 0)
                        {
                            int fileNameLen = BitConverter.ToInt32(clientData, 0);
                            string fileFullName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);

                            // Если без исключения ArgumentOutOfRangeException, значит пришёл файл
                            await BroadcastFilesAsync(clientData, receivedByteLen, clientSocket.RemoteEndPoint);
                            Console.WriteLine($"{userName} прислал файл");
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // В случае такого исключения пришло сообщение
                        message = $"{userName}: {Encoding.UTF8.GetString(clientData, 0, receivedByteLen)}";
                        Console.WriteLine(message);
                        await BroadcastMessageAsync(message, clientSocket.RemoteEndPoint);
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Удаленный хост принудительно разорвал существующее подключение")
                    Console.WriteLine($"{userName} вышел из чата");
                else Console.WriteLine(ex.Message);
            }
            finally
            {
                // после break в цикле нужно закрыть подключение для клиента
                RemoveConnection(clientSocket.RemoteEndPoint);
            }
        }

        /// <summary>
        /// Метод для ретранслирования сообщения всем остальным клиентам кроме заданного
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task BroadcastMessageAsync(string message, EndPoint id)
        {
            foreach (var client in clients)
            {
                if (client.RemoteEndPoint != id)
                {
                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    client.Send(messageBytes);
                }
            }
        }

        /// <summary>
        /// Метод для ретранслирования файла всем остальным клиентам кроме заданного
        /// </summary>
        /// <param name="clientData"></param>
        /// <param name="size"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task BroadcastFilesAsync(byte[] clientData, int size, EndPoint id)
        {
            foreach (var client in clients)
            {
                if (client.RemoteEndPoint != id)
                {
                    client.Send(clientData, size, SocketFlags.None);
                }
            }
        }

        /// <summary>
        /// Метод удаления клиента из списка после его закрытия
        /// </summary>
        /// <param name="id"></param>
        private void RemoveConnection(EndPoint id)
        {
            Socket removedClient = clients.FirstOrDefault(cl => cl.RemoteEndPoint == id);
            if (removedClient != null)
                clients.Remove(removedClient);

            removedClient?.Close();
        }
        #endregion
    }
}
