using System;
using System.Collections.Generic;
using System.Linq;
using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Domain.Models;

namespace MOBOT.DigitalAnnotations.Data.Converters
{
    public static class AnnotationModelConverter
    {
        public static AnnotationModel ToModel(this Annotation entity, bool includeChildren = true)
        {
            AnnotationModel model = null;

            if (entity != null)
            {
                model = new AnnotationModel
                {
                    Id = entity.AnnotationId,
                    Body = entity.Body,
                    TargetId = entity.AnnotationTargetId,
                    Target = entity.Target?.ToModel(),
                    CreatedDate = entity.CreatedDate,
                    UpdatedDate = entity.UpdatedDate,
                    CreatedUser = entity.CreatedUser?.Email,
                    UpdatedUser = entity.UpdatedUser?.Email,
                    GroupId = entity.GroupId,
                    GroupName = entity.Group?.GroupName,
                    CreatedUserId = entity.CreatedUserId,
                    UpdatedUserId = entity.UpdatedUserId,
                    LicenseId = entity.LicenseId,
                    License = entity.License?.ToModel(),
                    AnnotationTypeId = entity.AnnotationTypeId,
                    AnnotationType = entity.AnnotationType?.ToModel(),
                    Annotations = includeChildren ? entity.Annotations?.ToList().ConvertAll(a => a.ToModel()) : new List<AnnotationModel>(),
                    ParentId = entity.ParentId,
                    Tags = entity.Tags?.ToList().ConvertAll(t => t.ToTagModel())
                };
            }

            return model;
        }

        public static Annotation ToEntity(this AnnotationModel model, Annotation entity = null)
        {
            Annotation rt = null;

            if (model != null)
            {
                if (entity == null)
                {
                    rt = new Annotation
                    {
                        AnnotationId = model.Id
                    };
                }
                else rt = entity;

                return PopulateEntity(model, rt);
            }

            return rt;
        }

        private static Annotation PopulateEntity(AnnotationModel model, Annotation entity)
        {
            entity.Body = model.Body;
            entity.AnnotationTargetId = model.TargetId > 0 ? model.TargetId : model.Target.Id;
            entity.CreatedDate = model.CreatedDate;
            entity.CreatedUserId = model.CreatedUserId;
            entity.UpdatedDate = model.UpdatedDate;
            entity.UpdatedUserId = model.UpdatedUserId;
            entity.GroupId = model.GroupId;
            entity.LicenseId = model.LicenseId;
            entity.AnnotationTypeId = model.AnnotationTypeId;
            entity.ParentId = model.ParentId;

            
            var modelTags = model.Tags?.ToList();
            if (modelTags == null)
            {
                if (entity.Tags.Count > 1)
                    entity.Tags.ToList().RemoveRange(0, entity.Tags.Count);
            }
            else {
                // delete first all that have been removed
                entity.Tags.ToList().ForEach(t => {
                    if (modelTags.FindIndex(mt => mt.id == t.TagId) < 0)
                        entity.Tags.Remove(t);
                });
                modelTags.ForEach(t =>
                {
                    if (!entity.Tags.Any(e => e.TagId == t.id))
                    {
                        entity.Tags.Add(t.ToAnnotationTagEntity());
                    }
                });
            }


            return entity;
        }
    }
}
