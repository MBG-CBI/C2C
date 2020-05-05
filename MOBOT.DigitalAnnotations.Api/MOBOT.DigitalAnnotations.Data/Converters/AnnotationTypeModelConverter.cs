using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Domain.Models;

namespace MOBOT.DigitalAnnotations.Data.Converters
{
    public static class AnnotationTypeModelConverter
    {
        public static AnnotationTypeModel ToModel(this AnnotationType entity) {
            if (entity == null)
                return null;

            var model = new AnnotationTypeModel
            {
                Id = entity.AnnotationTypeId,
                Name = entity.TypeName
            };

            return model;
        }
    }
}
