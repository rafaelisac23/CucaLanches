using CucaLanches.Application.Clients.DTOs;

namespace CucaLanches.Application.Clients.Interfaces;

public interface IClientService
{
    Task<ClientResponseDTO> IdentifyCLient(IdentifyClientRequestDTO request);
    Task<ClientResponseDTO> CreateClient(ClientRequestDTO request);
    Task<ClientResponseDTO> UpdateClient(int id, ClientUpdateRequestDTO request);
}