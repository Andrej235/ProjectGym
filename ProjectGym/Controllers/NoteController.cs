using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    public class NoteController(ICreateService<Note> createService, IUpdateService<Note> updateService, IDeleteService<Note> deleteService, IReadService<Note> readService, IEntityMapper<Note, NoteDTO> mapper) : RepositoryController<Note, NoteDTO>(createService, updateService, deleteService, readService, mapper) { }
}