using MedHelpAuthorizations.Application.Features.Notes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Notes.Queries.BelongsTo;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetNotesByAuthorizationId;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Notes
{
    public class NoteManager : INoteManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public NoteManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<List<GetNotesByAuthorizationIdResponse>>> GetByAuthIdAsync(int aid)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.NotesEndpoints.GetByAuth(aid));
            return await response.ToResult<List<GetNotesByAuthorizationIdResponse>>();
        }

        public async Task<IResult<GetNotesByIdResponse>> GetById(int id)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.NotesEndpoints.GetById(id));
            return await response.ToResult<GetNotesByIdResponse>();
        }

        public async Task<IResult<NoteBelongsToResponse>> BelongsTo(int id)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.NotesEndpoints.BelongsTo(id));
            return await response.ToResult<NoteBelongsToResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditNotesCommand command)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.NotesEndpoints.Save, command);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{Routes.NotesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }
    }
}
