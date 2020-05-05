using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Data.Converters;
using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Data.Interfaces;
using MOBOT.DigitalAnnotations.Domain.Models;
using System.Threading.Tasks;

namespace MOBOT.DigitalAnnotations.Business.Services
{
    public class AuthorizationService: IAuthorizationService
    {
        private readonly ILogger<AuthorizationService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<User> _users;
        private readonly DbSet<Group> _groups;
        private readonly DbSet<UserGroup> _userGroups;

        public AuthorizationService(IUnitOfWork unitOfWork, ILogger<AuthorizationService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _users = _unitOfWork.Set<User>();
            _groups = _unitOfWork.Set<Group>();
            _userGroups = _unitOfWork.Set<UserGroup>();
        }

        public async Task<UserModel> LoginAsync(string userName, string password) {

            var entity = await _users.Include(u => u.Groups).ThenInclude(ug => ug.Group).FirstOrDefaultAsync(u => u.Email == userName && u.Password == password);
            if (entity == null)
                return null;
            var model = entity.ToModel();
            return model;
        
        }
    }
}
