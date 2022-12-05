using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProShop.Common.EntityFramework;

public static class EfToolkit
{
    public static void RegisterAllEntities(this ModelBuilder builder, Type type)
    {
        var entities = type.Assembly.GetTypes().Where(x => x.BaseType == type);
        foreach (var entity in entities)
            builder.Entity(entity);
    }
}

