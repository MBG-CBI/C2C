using MOBOT.DigitalAnnotations.Domain.Models;
using MOBOT.DigitalAnnotations.Data.Entities;

namespace MOBOT.DigitalAnnotations.Data.Converters
{
    public static class AnnotationTargetModelConverter
    {
        public static AnnotationTargetModel ToModel(this AnnotationTarget entity)
        {
            AnnotationTargetModel model = null;
            if (entity != null)
            {
                model = new AnnotationTargetModel
                {
                    Id = entity.AnnotationTargetId,
                    SourceId = entity.AnnotationSourceId,
                    // Source = entity.Source.ToModel(),
                    CoordinateX = entity.CoordinateX,
                    CoordinateY = entity.CoordinateY,
                    Height = entity.Height,
                    Width = entity.Width

                };
            }

            return model;
        }

        public static AnnotationTarget ToEntity(this AnnotationTargetModel model, AnnotationTarget entity = null)
        {
            AnnotationTarget rt = null;

            if (model != null)
            {
                if (entity == null)
                {
                    rt = new AnnotationTarget
                    {
                        AnnotationTargetId = model.Id,
                    };
                }
                else rt = entity;

                return PopulateEntity(model, rt);
            }

            return rt;
        }

        private static AnnotationTarget PopulateEntity(this AnnotationTargetModel model, AnnotationTarget entity)
        {
            if (model != null)
            {               
                entity.CoordinateX = model.CoordinateX;
                entity.CoordinateY = model.CoordinateY;
                entity.Height = model.Height;
                entity.Width = model.Width;
                entity.AnnotationSourceId = model.SourceId;
            }
            return entity;
        }
    }
}
