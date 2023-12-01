using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    public class ImageController(ICreateService<Image> createService, IUpdateService<Image> updateService, IDeleteService<Image> deleteService, IReadService<Image> readService, IEntityMapper<Image, ImageDTO> mapper) : RepositoryController<Image, ImageDTO>(createService, updateService, deleteService, readService, mapper) { }
}