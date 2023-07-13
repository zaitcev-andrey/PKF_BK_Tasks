using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace ServerClient
{
    public class Server
    {
        // создаём серверный сокет для прослушивания
        private Socket serverSocket;
        // и список всех клиентов
        private List<Socket> clients;

        public Server()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8080);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // связываем сокет с локальной точкой ipPoint
            serverSocket.Bind(ipPoint);

            clients = new List<Socket>();
        }

        // основной метод для прослушивания клиентов
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

        public async Task ChatAsync(Socket clientSocket)
        {
            try
            {
                var nameBytes = new byte[512];
                int bytes = clientSocket.Receive(nameBytes);
                string userName = Encoding.UTF8.GetString(nameBytes, 0, bytes);

                string message = $"{userName} вошёл в чат";

                // первым делом посылаем сообщение о входе нового
                // пользователя в чат всем подключенным клиентам
                await BroadcastMessageAsync(message, clientSocket.RemoteEndPoint);
                Console.WriteLine(message); // через консоль отражаем сообщение на сервере

                // и теперь в бесконечном цикле можем отправлять сообщения от клиента всем остальным клиентам и серверу
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

                            // если без исключения ArgumentOutOfRangeException, значит пришёл файл
                            await BroadcastFilesAsync(clientData, receivedByteLen, clientSocket.RemoteEndPoint);
                            Console.WriteLine($"{userName} прислал файл");
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // в случае такого исключения пришло сообщение
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
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // после break в цикле нужно закрыть подключение для клиента
                RemoveConnection(clientSocket.RemoteEndPoint);
            }
        }

        // в этом методе сообщение, полученное от одного клиента, будет
        // отправлено всем остальным клиентам
        public async Task BroadcastMessageAsync(string message, EndPoint id)
        {
            foreach (var client in clients)
            {
                // именно для этого места нам и нужно было Id
                // чтобы не отправить сообщение о подключении самому себе
                if (client.RemoteEndPoint != id)
                {
                    // конвертируем данные в массив байтов
                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    // отправляем данные
                    client.Send(messageBytes);
                }
            }
        }

        public async Task BroadcastFilesAsync(byte[] clientData, int size, EndPoint id)
        {
            foreach (var client in clients)
            {
                if (client.RemoteEndPoint != id)
                {
                    client.Send(clientData, size, SocketFlags.None);
                }
            }
        }

        // метод удаляет клиента, который закрылся, из списка
        public void RemoveConnection(EndPoint id)
        {
            Socket removedClient = clients.FirstOrDefault(cl => cl.RemoteEndPoint == id);
            if (removedClient != null)
                clients.Remove(removedClient);
            // и обязательно запускаем метод на очищение потоков записи и чтения у клиента
            removedClient?.Close();
        }

        // метод, который выполняется после закрытия сервера
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
    }
}
