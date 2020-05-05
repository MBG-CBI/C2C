using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Domain.Models;

namespace MOBOT.DigitalAnnotations.Data.Converters
{
    public static class AnnotationTagModelConverter
    {
        public static TagModel ToTagModel(this AnnotationTag entity) {
            if (entity == null || entity.Tag == null)
                return null;

            var model = new TagModel
            {
                id = entity.Tag.TagId,
                Name = entity.Tag.TagName
            };

            return model;
        }

        public static AnnotationTag ToAnnotationTagEntity(this TagModel model) {
            var entity = new AnnotationTag
            {
                TagId = model.id
            };

            return entity;
        }

        public static TagModel ToModel(this Tag entity) {
            if (entity == null)
                return null;

            var model = new TagModel
            {
                id = entity.TagId,
                Name = entity.TagName
            };

            return model;
        }

    }
}
