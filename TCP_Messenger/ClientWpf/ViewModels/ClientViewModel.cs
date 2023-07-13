using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientWpf.Model;
using ClientWpf.Commands;

namespace ClientWpf.ViewModels
{
    internal class ClientViewModel
    {
        #region private Members
        private ClientModel _model;
        
        #endregion

        #region Constructors
        public ClientViewModel()
        {
            _model = new ClientModel();
            
        }
        #endregion

        #region public Properties
        public ClientModel Model { get { return _model; } }
        #endregion

        #region private Commands
        private RelayCommand _connectToServerCommand;
        private RelayCommand _sendMessageCommand;
        private RelayCommand _sendFileCommand;
        private RelayCommand _dropPanelCommand;
        #endregion

        #region public Commands
        public RelayCommand ConnectToServerCommand
        {
            get
            {
                return _connectToServerCommand ??
                (_connectToServerCommand = new RelayCommand(obj =>
                {
                    Model.ConnectToServer();
                }));
            }
        }
        public RelayCommand SendMessageCommand
        {
            get
            {
                return _sendMessageCommand ??
                (_sendMessageCommand = new RelayCommand(obj =>
                {
                    Model.SendMessage();
                }));
            }
        }
        public RelayCommand SendFileCommand
        {
            get
            {
                return _sendFileCommand ??
                (_sendFileCommand = new RelayCommand(obj =>
                {
                    Model.SendFile();
                }));
            }
        }
        #endregion
    }
}
