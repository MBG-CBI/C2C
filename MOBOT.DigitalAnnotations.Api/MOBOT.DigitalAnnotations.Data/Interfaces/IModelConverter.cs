namespace MOBOT.DigitalAnnotations.Data.Interfaces
{
    public interface IModelConverter<TEntity, TModel> where TEntity : class, new() where TModel : class, new()
    {
        TModel ToModel(TEntity entity);
        TEntity ToEntity(TModel model);
    }
}
