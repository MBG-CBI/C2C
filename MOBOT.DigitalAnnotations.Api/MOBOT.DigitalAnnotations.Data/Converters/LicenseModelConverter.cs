using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Domain.Models;

namespace MOBOT.DigitalAnnotations.Data.Converters
{
    public static class LicenseModelConverter
    {
        public static LicenseModel ToModel(this License entity) {
            if (entity == null)
                return null;

            var model = new LicenseModel
            {
                Id = entity.LicenseId,
                Code = entity.Code,
                DisplayName = entity.DisplayName,
                IconUrl = entity.IconUrl,
                Sequence = entity.Sequence,
                LicenseUrl = entity.LicenseUrl
                
            };

            return model;
        }
    }
}
