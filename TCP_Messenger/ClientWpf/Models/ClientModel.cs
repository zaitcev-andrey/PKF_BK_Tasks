using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientWpf.Models
{
    internal class ClientModel : BaseModel
    {
        #region private Members
        private string _host;
        private int _port;
        private string _userName;

        private Socket _clientSocket;

        private StringBuilder _chatLogBuffer;
        private string _message;
        private bool _isSendFile;
        private bool _isSendMessage;
        private string _fileName;
        private string _fileShortName;
        #endregion

        #region Constructors
        public ClientModel()
        {
            _host = "127.0.0.1";
            _port = 8080;
            _userName = "";

            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _chatLogBuffer = new StringBuilder();
            _message = "";
            _isSendFile = false;
            _isSendMessage = false;
            _fileName = "";
        }
        #endregion

        #region public Properties
        public string Host
        {
            get { return _host; }
            set
            {
                _host = value;
                OnPropertyChanged("Host");
            }
        }

        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged("Port");
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        public string ChatLog
        {
            get { return _chatLogBuffer.ToString(); }
            set
            {
                _chatLogBuffer.AppendLine(value);
                OnPropertyChanged("ChatLog");
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public string FileShortName
        {
            get { return _fileShortName; }
            set
            {
                _fileShortName = value;
                OnPropertyChanged("FileShortName");
            }
        }
        #endregion

        #region public Methods
        /// <summary>
        /// Метод для подключения клиента к серверу
        /// </summary>
        public void ConnectToServer()
        {
            try
            {
                if (string.IsNullOrEmpty(Host) || Port == 0 || string.IsNullOrEmpty(UserName))
                {
                    ChatLog = "Не хватает данных для подключения, проверьте их и попробуйте снова";
                    return;
                }

                // после выполнения Connect сервер принимает клиента методом AcceptAsync()
                _clientSocket.Connect(Host, Port);

                Task.Run(() => ReceiveDataAsync());
                Task.Run(() => SendMessageAsync());
                Task.Run(() => SendFileAsync());
            }
            catch (Exception ex)
            {
                ChatLog = ex.Message;
            }
        }

        /// <summary>
        /// В этом методе активируется отправка сообщения
        /// </summary>
        public void SendMessage()
        {
            _isSendMessage = true;
        }

        /// <summary>
        /// В этом методе активируется отправка файла
        /// </summary>
        public void SendFile()
        {
            _isSendFile = true;
        }
        #endregion

        #region private Methods
        /// <summary>
        /// Метод для отправки сообщения на сервер
        /// </summary>
        /// <returns></returns>
        private async Task SendMessageAsync()
        {
            // Отправляем имя
            var messageBytes = Encoding.UTF8.GetBytes(UserName);
            _clientSocket.Send(messageBytes);

            ChatLog = "Вы вошли в чат";

            while (true)
            {
                await Task.Delay(10);
                if (_isSendMessage)
                {
                    if (!string.IsNullOrEmpty(Message))
                    {
                        messageBytes = Encoding.UTF8.GetBytes(Message);
                        ChatLog = $"{UserName}: {Message}";

                        _clientSocket.Send(messageBytes);
                        Message = "";
                    }
                    _isSendMessage = false;
                }
            }
        }

        /// <summary>
        /// Метод для отправки файла на сервер
        /// </summary>
        /// <returns></returns>
        private async Task SendFileAsync()
        {
            while (true)
            {
                await Task.Delay(10);
                if (!string.IsNullOrEmpty(_fileName))
                {
                    FileShortName = $"Вы выбрали файл: {Path.GetFileName(_fileName)}";

                    if (_isSendFile)
                    {
                        string fileName = _fileName;
                        byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);
                        byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
                        byte[] fileData = File.ReadAllBytes(fileName);
                        byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];

                        fileNameLen.CopyTo(clientData, 0);
                        fileNameByte.CopyTo(clientData, 4);
                        fileData.CopyTo(clientData, 4 + fileNameByte.Length);

                        
                        ChatLog = "Вы отправили файл";

                        _clientSocket.Send(clientData);
                        _isSendFile = false;
                    }
                }
            }
        }

        /// <summary>
        /// Метод для получения данных от сервера
        /// </summary>
        /// <returns></returns>
        private async Task ReceiveDataAsync()
        {
            while (true)
            {
                await Task.Delay(10);
                var clientData = new byte[1024 * 10000]; // до 10мб
                int receivedByteLen = _clientSocket.Receive(clientData);
                try
                {
                    if (receivedByteLen > 0)
                    {
                        // Если без исключения ArgumentOutOfRangeException, значит пришёл файл
                        int fileNameLen = BitConverter.ToInt32(clientData, 0);
                        string fileFullName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);
                        
                        string fileName = Path.GetFileName(fileFullName);

                        using (var stream = File.Open(fileName, FileMode.Create))
                        {
                            using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8, false))
                            {
                                binaryWriter.Write(clientData, 4 + fileNameLen, receivedByteLen - 4 - fileNameLen);
                                binaryWriter.Close();
                            }
                        }

                        ChatLog = "Вы получили файл, проверьте папку";
                    }                    
                }
                catch (ArgumentOutOfRangeException)
                {
                    // В случае такого исключения пришло сообщение
                    string message = Encoding.UTF8.GetString(clientData, 0, receivedByteLen);
                    ChatLog = message;
                }
                catch
                {
                    break;
                }
            }
        }
        #endregion
    }
}
