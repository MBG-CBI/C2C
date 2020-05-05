using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Domain.Models;

namespace MOBOT.DigitalAnnotations.Data.Converters
{
    public static class UserGroupModelConverter
    {
        public static UserGroupModel ToUserGroupModel(this UserGroup entity) {

            if (entity == null)
                return null;

            var model = new UserGroupModel
            {
                Id = entity.UserGroupId,
                Group = new GroupModel { Id = entity.Group.GroupId, Name = entity.Group.GroupName }
            };

            return model;
        }
    }
}
