using System.Linq;
using MOBOT.DigitalAnnotations.Domain.Models;
using MOBOT.DigitalAnnotations.Data.Entities;

namespace MOBOT.DigitalAnnotations.Data.Converters
{
    public static class AnnotationSourceModelConverter
    {
        public static AnnotationSourceModel ToModel(this AnnotationSource entity)
        {
            AnnotationSourceModel model = null;
            if (entity != null) {
                model = new AnnotationSourceModel
                {
                    Id = entity.AnnotationSourceId,
                    RerumStorageUrl = entity.RerumStorageUrl,
                    ImageHeight = entity.ImageHeight,
                    ImageWidth = entity.ImageWidth,                  
                    SourceUrl = entity.SourceUrl,
                    CreatedDate = entity.CreatedDate,
                    CreatedUser = entity.CreatedUser,
                    UpdatedDate = entity.UpdatedDate,
                    UpdatedUser = entity.UpdatedUser,
                    Targets = entity.Targets?.ToList().ConvertAll( t => t.ToModel() )      
                };
            }
            return model;
        }

        public static AnnotationSource ToEntity(this AnnotationSourceModel model, AnnotationSource entity = null)
        {
            AnnotationSource rt = null;
            if (model != null)
            {
                if (entity == null)
                {
                    rt = new AnnotationSource()
                    {
                        AnnotationSourceId = model.Id,                        
                    };
                }
                else rt = entity;

                return PopulateEntity(model, rt);
            }
            return rt;
        }

        public static AnnotationSource PopulateEntity(this AnnotationSourceModel model, AnnotationSource entity)
        {
            entity.RerumStorageUrl = model.RerumStorageUrl;
            entity.ImageHeight = model.ImageHeight;
            entity.ImageWidth = model.ImageWidth;
            entity.SourceUrl = model.SourceUrl;
            entity.CreatedDate = model.CreatedDate;
            entity.CreatedUser = model.CreatedUser;
            entity.UpdatedDate = model.UpdatedDate;
            entity.UpdatedUser = model.UpdatedUser;
            return entity;
        }
    }
}
