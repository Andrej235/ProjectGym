[Serializable]
public class UnresolvedDependencyException : Exception
{
    public UnresolvedDependencyException() { }
    public UnresolvedDependencyException(string message) : base(message) { }
    public UnresolvedDependencyException(string message, Exception inner) : base(message, inner) { }
}


/*        static bool IsNullable(Type type)
        {
            if (!type.IsValueType) return true; // Reference type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable value type
            return false; // Non-nullable value type
        }

        static void CheckAllNonNullableProperties(object entity)
        {
            foreach (var property in entity.GetType().GetProperties().Where(x => IsNullable(x.PropertyType)))
            {
                if (property.GetValue(entity) == null)
                {
                    if (property.PropertyType.IsValueType)
                    {
                        property.SetValue(entity, Activator.CreateInstance(property.PropertyType));
                    }
                    else
                    {
                        var defaultConstructor = property.PropertyType.GetConstructor(Type.EmptyTypes);
                        if (defaultConstructor != null)
                        {
                            property.SetValue(entity, Activator.CreateInstance(property.PropertyType));
                        }
                        else
                        {
                            throw new InvalidOperationException($"Cannot create an instance of type {property.PropertyType.Name}. Consider providing a default constructor or handle this case differently.");
                        }
                    }
                }
            }
        }*/