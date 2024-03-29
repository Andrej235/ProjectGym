﻿namespace ProjectGym.Services.Mapping
{
    public interface IEntityMapper<TEntity, TDTO>
    {
        TDTO Map(TEntity entity);
        TEntity Map(TDTO dto);
    }
}
