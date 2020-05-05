using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Domain.Models;
using System.Linq;

namespace MOBOT.DigitalAnnotations.Data.Converters
{
    public static class UserModelConverter
    {
        public static UserModel ToModel(this User entity) {
            if (entity == null)
                return null;

            var model = new UserModel {
                Id = entity.Id,
                Email = entity.Email,
                Groups = entity.Groups.ToList().ConvertAll(ug => ug.ToUserGroupModel())
            };

            return model;

        }
    }
}
