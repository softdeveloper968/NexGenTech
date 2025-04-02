using MedHelpAuthorizations.Application.Features.Documents.Queries.Base;
using MedHelpAuthorizations.Client.Infrastructure.Interfaces;
using MedHelpAuthorizations.Shared.Responses.Identity;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MedHelpAuthorizations.Client.Infrastructure.Authentication
{
    public class ClientInfoStateProvider : IState
	{
        public ClientInfoStateProvider() 
        { 
            _isPopulatingClientinfo = false; 
        }

        private bool _isPopulatingClientinfo { get; set; } = false;
        public bool IsPopulatingClientinfo { get { return _isPopulatingClientinfo; } }
        private ClientInfoResponse? _clientInfo { get; set; }
        public ClientInfoResponse? ClientInfo
        {
            get => _clientInfo;
            private set 
            { 
                if(_clientInfo == null || !_clientInfo.Equals(value))
                {
                    _clientInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        public void SetIsPopulatingClientinfo(bool isPopulatingClientinfo)
        {
            _isPopulatingClientinfo = isPopulatingClientinfo;
        }
        public void SetClientInfo(ClientInfoResponse? clientInfo)
		{
			ClientInfo = clientInfo;
		}

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        public virtual event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); ;
        }

        // Method to update ClientInfo
        public void UpdateClientInfo(ClientInfoResponse? newInfo, bool overwrite = false)
        {
            if (newInfo == null) return;

            if (overwrite || _clientInfo == null || !_clientInfo.Equals(newInfo))
            {
                _clientInfo = newInfo;
            }
        }

        private InputDocumentFileStatusResponse? _inputDocumentFileStatusResponse {  get; set; }

        public InputDocumentFileStatusResponse? InputDocumentFileStatusResponse
        {
            get => _inputDocumentFileStatusResponse;
            private set
            {
                if (_inputDocumentFileStatusResponse == null || !_inputDocumentFileStatusResponse.Equals(value))
                {
                    _inputDocumentFileStatusResponse = value;
                    OnPropertyChanged();
                }
            }
        }

        public void SetInputDocumentFileStatusResponse(InputDocumentFileStatusResponse? documentInfo)
        {
            InputDocumentFileStatusResponse = documentInfo;
        }
    }
}
